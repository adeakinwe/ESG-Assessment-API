namespace ESGG.Api.Enums
{
    public enum EsgAiRiskLevelEnum
    {
        Low,
        Medium,
        High
    }

    public enum EsgAiRecommendationEnum
    {
        ProceedProvisionally = 1,
        ProceedWithConditionsAndSecondaryReview = 2,
        EnhancedDueDiligenceAndMitigationRequired = 3,
        EscalateForComprehensiveEsgReview = 4
    }
}