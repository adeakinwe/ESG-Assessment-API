namespace ESG.Api.DTOs
{
    public class ChecklistResponseValueDto
    {
        public int ResponseValue { get; set; } // 0/1 or 1/2/3
        public int Score { get; set; }
        public required string Label { get; set; } // Yes, No, Low, Medium, High
    }

    public class ChecklistItemDto
    {
        public int ChecklistItemId { get; set; }
        public required string Item { get; set; }
        public required string Category { get; set; }
        public required string indicatorType { get; set; }
        public int Weight { get; set; }
        public required string ResponseType { get; set; }
        public int ResponseTypeId { get; set; }
        public required List<ChecklistResponseValueDto> Responses { get; set; }
    }
}
