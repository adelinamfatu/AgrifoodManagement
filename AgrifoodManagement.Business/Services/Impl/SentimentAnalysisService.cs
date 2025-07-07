using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace AgrifoodManagement.Business.Services.Impl
{
    public class SentimentAnalysisService : ISentimentAnalysisService
    {
        private readonly MLContext _ml;
        private ITransformer _model;
        private PredictionEngine<SentimentData, SentimentPrediction> _engine;
        private readonly string _modelPath;
        private readonly string _csvPath;

        public SentimentAnalysisService()
        {
            _ml = new MLContext(seed: 0);

            var baseDir = AppContext.BaseDirectory;
            var dataFolder = Path.Combine(baseDir, "Data", "SentimentAnalysis");
            Directory.CreateDirectory(dataFolder);

            _csvPath = Path.Combine(dataFolder, "training-dataset.csv");
            _modelPath = Path.Combine(dataFolder, "sentiment-model.zip");

            if (File.Exists(_modelPath))
            {
                using var fs = File.OpenRead(_modelPath);
                _model = _ml.Model.Load(fs, out _);
                var emptyData = _ml.Data.LoadFromEnumerable(new List<SentimentData>());
                var schema = _model.GetOutputSchema(emptyData.Schema);
                _engine = _ml.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(_model);
            }
        }

        public async Task TrainAsync(IEnumerable<Review> dbReviews, CancellationToken ct = default)
        {
            var loader = _ml.Data.CreateTextLoader(new TextLoader.Options
            {
                Separators = new[] { ',' },
                HasHeader = true,
                AllowQuoting = true,
                Columns = new[]
                {
                    new TextLoader.Column(
                        name: nameof(SentimentData.Comment),
                        dataKind: DataKind.String,
                        index: 0),

                    new TextLoader.Column(
                        name: nameof(SentimentData.Label),
                        dataKind: DataKind.String,
                        index: 1),
                }
            });

            var csv = loader.Load(_csvPath);
            var seed = _ml.Data.CreateEnumerable<SentimentData>(csv, reuseRowObject: false);

            var fromDb = dbReviews.Select(r => new SentimentData { Comment = r.Comment, Label = MapRatingToLabel(r.Rating) });

            var combined = seed.Concat(fromDb);
            var trainingView = _ml.Data.LoadFromEnumerable(combined.Take(5000));

            var pipeline = _ml.Transforms.Conversion
                    .MapValueToKey("LabelKey", "Label")
                    .Append(_ml.Transforms.Text
                        .FeaturizeText("Features", "Comment"))
                    .Append(_ml.MulticlassClassification
                        .Trainers.SdcaMaximumEntropy(
                           labelColumnName: "LabelKey",
                           featureColumnName: "Features"))
                        .Append(_ml.Transforms.Conversion
                       .MapKeyToValue("PredictedLabel", "PredictedLabel"));

            _model = pipeline.Fit(trainingView);
            _engine = _ml.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(_model);

            using var outFs = File.Create(_modelPath);
            _ml.Model.Save(_model, trainingView.Schema, outFs);
        }

        public (SentimentType Label, float Confidence) Predict(string comment)
        {
            if (_engine == null)
                throw new InvalidOperationException("Model is not trained or loaded.");

            var pred = _engine.Predict(new SentimentData { Comment = comment });
            var bestScore = pred.Score.Max();
            var label = Enum.Parse<SentimentType>(pred.PredictedLabel, ignoreCase: true);
            return (label, bestScore);
        }

        private string MapRatingToLabel(int rating) => rating switch
        {
            1 or 2 => SentimentType.Negative.ToString(),
            3 => SentimentType.Neutral.ToString(),
            4 or 5 => SentimentType.Positive.ToString(),
            _ => SentimentType.Neutral.ToString()
        };
    }
}
