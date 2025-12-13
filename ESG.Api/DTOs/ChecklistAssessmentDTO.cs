namespace ESG.Api.DTos
{
    public class EsgChecklistSubmissionDto
    {
        public int LoanApplicationId { get; set; }
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
}