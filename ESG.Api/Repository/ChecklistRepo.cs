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

        public async Task SubmitChecklistAssessmentAsync(EsgChecklistSubmissionDto dto)
        {
            // Prevent duplicate assessment
            if (await IsLoanExistsAsync(dto.LoanApplicationId))
            {
                await RemoveChecklistAssessmentAsync(dto.LoanApplicationId);
            }

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
            await _context.SaveChangesAsync();
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
    }
}