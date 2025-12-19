using ESG.Api.DTOs;
using ESG.Api.Interface;

namespace ESG.Api.Services
{
    public class EsgMlSignalService : IEsgMlSignalService
    {
        public Task<EsgMlSignalDTO> EvaluateAsync(EsgMlFeatureDTO f)
        {
            // Proxy anomaly detection
            var anomalyScore = (double)f.ScoreStdDev / 10.0;
            anomalyScore = Math.Min(anomalyScore, 1.0);

            // Risk probability proxy
            var riskProbability = (double)f.NormalizedRiskScore / 100.0;

            var isOutlier =
                f.ScoreStdDev > 2.5m ||
                f.HighRiskItemCount >= 5;

            var band =
                riskProbability >= 0.7 ? "High" :
                riskProbability >= 0.4 ? "Medium" :
                "Low";

            return Task.FromResult(new EsgMlSignalDTO
            {
                AnomalyScore = Math.Round((decimal)anomalyScore, 2),
                RiskProbability = Math.Round((decimal)riskProbability, 2),
                RiskBand = band,
                IsOutlier = isOutlier
            });
        }
    }
}