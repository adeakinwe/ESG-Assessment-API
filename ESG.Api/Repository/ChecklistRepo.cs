using ESG.Api.Data;
using ESG.Api.DTos;
using ESG.Api.DTOs;
using ESG.Api.Interface;
using ESG.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ESG.Api.Repository
{
    public class ChecklistRepo : IChecklistRepo
    {
        private readonly AppDbContext _context;

        public ChecklistRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ChecklistItemDto>> GetChecklistAsync()
        {
            var items = await _context.ESG_CHECKLIST_ITEM
                .Select(item => new ChecklistItemDto
                {
                    ChecklistItemId = item.CHECKLISTITEMID,
                    Item = item.CHECKLISTITEM,
                    Category = item.CATEGORY,
                    indicatorType = item.INDICATORTYPE,
                    Weight = item.WEIGHT,
                    ResponseTypeId = item.RESPONSETYPEID,
                    ResponseType = item.RESPONSETYPEID == 1 ? "Yes/No" : "Low/Medium/high",
                    Responses = _context.ESG_CHECKLIST_ITEM_SCORE.Where(s => s.CHECKLISTITEMID == item.CHECKLISTITEMID)
                        .OrderBy(s => s.CHECKLISTITEMID)
                        .Select(s => new ChecklistResponseValueDto
                        {
                            ResponseValue = s.RESPONSETYPEVALUE,
                            Score = s.SCORE,
                            Label = item.RESPONSETYPEID == 1
                                ? (s.RESPONSETYPEVALUE == 1 ? "Yes" : "No")
                                : (s.RESPONSETYPEVALUE == 1 ? "Low"
                                    : s.RESPONSETYPEVALUE == 2 ? "Medium" : "High")
                        }).ToList()
                })
                .ToListAsync();

            return items;
        }

        public async Task<bool> IsLoanExistsAsync(int loanApplicationId)
        {
            return await _context.ESG_CHECKLIST_ASSESSMENT
                .AnyAsync(x => x.LOANAPPLICATIONID == loanApplicationId);
        }

        private async Task<bool> IsLoanSubmittedForAppraisalAsync(int loanApplicationId)
        {
            return await _context.LOAN_APPLICATION
                .AnyAsync(x => x.LOANAPPLICATIONID == loanApplicationId && x.SUBMITTEDFORAPPRAISAL == true);
        }

        public async Task SubmitChecklistAssessmentAsync(EsgChecklistSubmissionDto dto)
        {
            if (await IsLoanSubmittedForAppraisalAsync(dto.LoanApplicationId))
            {
                throw new InvalidOperationException("Cannot modify assessment for a loan application that has been submitted for appraisal.");
            }
            // Prevent duplicate assessment
            if (await IsLoanExistsAsync(dto.LoanApplicationId))
            {
                await RemoveChecklistAssessmentAsync(dto.LoanApplicationId);
                // Also remove existing summary if needed
                var existingSummary = await _context.ESG_CHECKLIST_SUMMARY
                    .Where(s => s.LOANAPPLICATIONID == dto.LoanApplicationId)
                    .FirstOrDefaultAsync();
                if (existingSummary != null)
                {
                    _context.ESG_CHECKLIST_SUMMARY.Remove(existingSummary);
                }
            }

            // 1️⃣ Save individual checklist assessments
            var entities = dto.Items.Select(i => new ESG_CHECKLIST_ASSESSMENT
            {
                CHECKLISTITEMID = i.ChecklistItemId,
                LOANAPPLICATIONID = dto.LoanApplicationId,
                RESPONSETYPEID = i.ResponseTypeId,
                SCORE = i.Score,
                WEIGHT = i.Weight,
                COMMENT_ = i.Comment ?? string.Empty
            });

            _context.ESG_CHECKLIST_ASSESSMENT.AddRange(entities);

            // 2️⃣ Calculate summary
            double totalScore = dto.Items.Sum(i => i.Score * i.Weight);
            double totalWeight = dto.Items.Sum(i => i.Weight * 10); //weight * max score (10)
            double averageScore = totalWeight > 0 ? Math.Round(totalScore / totalWeight * 100, 2) : 0;

            int ratingId = averageScore >= 65 ? 3  // High
                          : averageScore >= 35 && averageScore < 65 ? 2 // Medium
                          : 1;                     // Low

            // Calculate confidence
            int totalItems = 30; // total checklist items
            int answeredItems = dto.Items.Count;
            decimal answeredWeight = dto.Items.Sum(i => i.Weight * 10); // weight * max score (10)
            List<decimal> scores = dto.Items.Select(i => (decimal)i.Score).ToList();
            
            decimal confidence = CalculateSummaryConfidence(
                totalItems,
                answeredItems,
                (decimal)totalWeight,
                (decimal)answeredWeight,
                scores);

            var summary = new ESG_CHECKLIST_SUMMARY
            {
                LOANAPPLICATIONID = dto.LoanApplicationId,
                TOTALSCORE = totalScore,
                TOTALWEIGHT = totalWeight,
                AVGSCORE = averageScore,
                RATINGID = ratingId,
                COMMENT_ = dto.Comment,
                CONFIDENCE = confidence
            };

            _context.ESG_CHECKLIST_SUMMARY.Add(summary);

            // 3️⃣ Save all changes in one transaction
            await _context.SaveChangesAsync();
        }

        public decimal CalculateSummaryConfidence(
            int totalItems,
            int answeredItems,
            decimal totalWeight,
            decimal answeredWeight,
            List<decimal> scores)
        {
            if (totalItems == 0 || answeredItems == 0)
                return 0.00m;

            // 1️⃣ Completion
            decimal completionScore = Math.Min(
                (decimal)answeredItems / totalItems, 1.0m);

            // 2️⃣ Weight Coverage
            decimal weightCoverageScore = totalWeight == 0
                ? 0
                : Math.Min(answeredWeight / totalWeight, 1.0m);

            // 3️⃣ Consistency (variance)
            decimal variance = CalculateVariance(scores);
            decimal consistencyScore = variance switch
            {
                <= 2 => 1.0m,
                <= 5 => 0.85m,
                <= 10 => 0.65m,
                _ => 0.40m
            };

            // 4️⃣ Data Sufficiency
            decimal dataSufficiencyScore = answeredItems switch
            {
                >= 30 => 1.0m,
                >= 20 => 0.85m,
                >= 10 => 0.65m,
                _ => 0.40m
            };

            // Final Confidence
            decimal confidence =
                (completionScore * 0.35m) +
                (weightCoverageScore * 0.25m) +
                (consistencyScore * 0.25m) +
                (dataSufficiencyScore * 0.15m);

            return Math.Round(confidence, 2);
        }

        private decimal CalculateVariance(List<decimal> scores)
        {
            if (!scores.Any()) return 0;

            decimal avg = scores.Average();
            return scores.Average(s => (s - avg) * (s - avg));
        }

        public async Task<EsgChecklistSubmissionDto> GetChecklistAssessmentByLoanIdAsync(int loanApplicationId)
        {
            var assessments = await _context.ESG_CHECKLIST_ASSESSMENT
                .Where(x => x.LOANAPPLICATIONID == loanApplicationId)
                .ToListAsync();


            if (assessments.Count > 0)
            {
                var data = new EsgChecklistSubmissionDto
                {
                    LoanApplicationId = loanApplicationId,
                    Items = assessments.Select(a => new EsgChecklistResponseDto
                    {
                        ChecklistItemId = a.CHECKLISTITEMID,
                        ResponseTypeId = a.RESPONSETYPEID,
                        Score = a.SCORE,
                        Weight = a.WEIGHT,
                        Comment = a.COMMENT_
                    }).ToList()
                };

                return data;
            }

            return new EsgChecklistSubmissionDto();
        }

        public async Task<bool> RemoveChecklistAssessmentAsync(int loanApplicationId)
        {
            var assessments = await _context.ESG_CHECKLIST_ASSESSMENT
                .Where(x => x.LOANAPPLICATIONID == loanApplicationId)
                .ToListAsync();

            if (assessments.Count > 0)
            {
                _context.ESG_CHECKLIST_ASSESSMENT.RemoveRange(assessments);
                return await _context.SaveChangesAsync() != 0;
            }

            return false;
        }

        public async Task<EsgChecklistSummaryDto> GetChecklistAssessmentSummaryByLoanIdAsync(int loanApplicationId)
        {
            var existingSummary = await _context.ESG_CHECKLIST_SUMMARY
                    .Where(s => s.LOANAPPLICATIONID == loanApplicationId)
                    .FirstOrDefaultAsync();

            if (existingSummary != null)
            {
                var data = new EsgChecklistSummaryDto
                {
                    loanApplicationId = existingSummary.LOANAPPLICATIONID,
                    ratingId = existingSummary.RATINGID,
                    totalScore = existingSummary.TOTALSCORE,
                    totalWeight = existingSummary.TOTALWEIGHT,
                    averageScore = existingSummary.AVGSCORE,
                    rating = existingSummary.RATINGID == 3 ? "High"
                             : existingSummary.RATINGID == 2 ? "Medium"
                             : "Low",
                    comment = existingSummary.COMMENT_
                };

                return data;
            }

            return new EsgChecklistSummaryDto();
        }
    }
}