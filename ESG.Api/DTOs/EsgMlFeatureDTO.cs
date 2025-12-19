namespace ESG.Api.DTOs
{
    public class EsgMlFeatureDTO
    {
        // Metadata (not fed into model directly if undesired)
        public int LoanApplicationId { get; set; }

        // Core Aggregates
        public decimal AverageScore { get; set; }
        public int TotalScore { get; set; }
        public int TotalWeight { get; set; }
        public decimal NormalizedRiskScore { get; set; } // 0–100

        // Category Aggregates
        public decimal EnvironmentAvgScore { get; set; }
        public decimal SocialAvgScore { get; set; }
        public decimal GovernanceAvgScore { get; set; }

        public decimal EnvironmentWeightRatio { get; set; }
        public decimal SocialWeightRatio { get; set; }
        public decimal GovernanceWeightRatio { get; set; }

        // Distribution Signals
        public decimal ScoreStdDev { get; set; }
        public decimal MaxScore { get; set; }
        public decimal MinScore { get; set; }

        public int HighRiskItemCount { get; set; }  // score ≥ 8
        public int LowRiskItemCount { get; set; }   // score ≤ 2

        // Explainability Signals
        public int PositiveImpactCount { get; set; }
        public int NeutralImpactCount { get; set; }
        public int NegativeImpactCount { get; set; }

        // Context
        public int SectorId { get; set; }
        public int? CountryId { get; set; }
    }
}