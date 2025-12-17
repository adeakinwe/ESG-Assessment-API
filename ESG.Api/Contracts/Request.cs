namespace ESG.Api.Contracts
{
    public record PreScreenRequest(
        int loanApplicationId,
        int sectorId,
        string? country,
        decimal loanAmount,
        int productId
    );

    public record FinalAssessmentRequest(
        int loanApplicationId,
        decimal averageScore,
        decimal totalScore,
        decimal totalWeight,
        string analystComment
    );
}