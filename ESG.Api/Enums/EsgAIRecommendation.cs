namespace ESGG.Api.Enums
{
    public enum EsgAiRiskLevel
    {
        Low,
        Medium,
        High
    }

    public enum EsgAiRecommendation
    {
        Proceed,
        ProceedWithConditions,
        HumanReviewRequired,
        Reject
    }
}