namespace ESG.Api.DTos
{
    public class EsgChecklistSubmissionDto
    {
        public int LoanApplicationId { get; set; }
        public string Comment { get; set; } = null!;
        public List<EsgChecklistResponseDto> Items { get; set; } = [];
    }

    public class EsgChecklistResponseDto
    {
        public int ChecklistItemId { get; set; }
        public int ResponseTypeId { get; set; }
        public int Score { get; set; }
        public int Weight { get; set; }
        public string? Comment { get; set; }
    }

    public class EsgChecklistSummaryDto
    {
        public int loanApplicationId { get; set; }
        public int ratingId { get; set; }
        public double totalScore { get; set; }
        public double totalWeight { get; set; }
        public double averageScore { get; set; }
        public string? rating { get; set; }
        public string? comment { get; set; }
    }
}