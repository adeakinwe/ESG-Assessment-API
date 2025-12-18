namespace ESG.Api.DTOs
{
    public class EsgAiRecommendationDTO
    {
        public int Id { get; set; }
        public int LoanApplicationId { get; set; }

        public string Stage { get; set; } = default!; // PRE_SCREEN / FINAL

        public short RiskLevel { get; set; }
        public required string Recommendation { get; set; }

        public decimal Confidence { get; set; }

        public string Payload { get; set; } = default!; // JSON
        public string ModelVersion { get; set; } = "v1.0";
        public int CreatedBy { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required List<ExplainabilityItem> Explainability { get; set; }
    }

    public class ExplainabilityItem
    {
        public string Factor { get; set; } = string.Empty;
        public string Impact { get; set; } = string.Empty; // "Positive", "Neutral", "Negative"
        public string Detail { get; set; } = string.Empty;
    }

    public class EsgFinalRecommendationDTO
    {
        public int LoanApplicationId { get; set; }
        public required string Recommendation { get; set; } // e.g., "Proceed with Conditions"
        public short RiskLevel { get; set; }       // High, Medium, Low
        public required decimal Confidence { get; set; }    // 0–100
        public required List<string> Flags { get; set; }    // High-risk items
        public required List<string> MitigationHints { get; set; } // Guidance
        public required string SummaryText { get; set; }    // Executive-friendly summary
    }
}