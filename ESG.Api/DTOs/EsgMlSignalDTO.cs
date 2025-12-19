namespace ESG.Api.DTOs
{
    public class EsgMlSignalDTO
    {
        public decimal AnomalyScore { get; set; }     // 0–1
        public decimal RiskProbability { get; set; }  // 0–1
        public required string RiskBand { get; set; }           // Low / Medium / High
        public bool IsOutlier { get; set; }
    }
}