using Microsoft.ML.Data;

namespace AgrifoodManagement.Util.Models
{
    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; }
        public float[] Score { get; set; }
    }
}
