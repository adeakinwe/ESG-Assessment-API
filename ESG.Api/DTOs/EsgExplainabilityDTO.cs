namespace ESG.Api.DTOs
{
    public class EsgExplainabilityDTO
    {
        public int LoanApplicationId { get; set; }
        public List<ChecklistExplainabilityItem> ItemDetails { get; set; } = new();
        public List<string> Flags { get; set; } = new();
        public List<string> MitigationHints { get; set; } = new();
    }

    public class ChecklistExplainabilityItem
    {
        public int ChecklistItemId { get; set; }
        public required string Question { get; set; }
        public double Score { get; set; }
        public double Weight { get; set; }
        public double Contribution { get; set; }
        public required string Impact { get; set; }
    }
}