using System;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;
using static Microsoft.ML.Transforms.Text.TextFeaturizingEstimator;
using Microsoft.ML.Model;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace textc
{
    public class SentenceData
    {
        public string Label { get; set; }
        public string Sentence { get; set; }
    }

    public class PredictionData
    {
        public string PredictedLabel { get; set; }
    }
    class Program
    {
        static void Train()
        {
            string dataPath = "train.txt", testDataPath = "test.txt";

            var context = new MLContext();

            // Create textloader for our structure
            var textLoader = context.Data.CreateTextReader(new TextLoader.Arguments()
            {
                Separator = "\t",
                Column = new[] { 
                    new TextLoader.Column("Label", DataKind.Text, 0), 
                    new TextLoader.Column("Sentence", DataKind.Text, 1)
                }
            });

            var trainDataView = textLoader.Read(dataPath);
            var testDataView = textLoader.Read(testDataPath);
            
            // Create data process pipeline
            // First we have to change label value into ML.NET KeyType
            var dataProcessPipeline = context.Transforms.Conversion.MapValueToKey("Label")
                // Then, we have to normalize text
                .Append(context.Transforms.Text.NormalizeText("Sentence", "NormalizedSentence"))
                // Featurize the given text with n-grams
                .Append(context.Transforms.Text.FeaturizeText("NormalizedSentence", "Features"))
                // Give the naive bayes algorithm
                .Append(context.MulticlassClassification.Trainers.NaiveBayes())
                // Convert back the label value to it's origin
                .Append(context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // Create our model with train data
            var model = dataProcessPipeline.Fit(trainDataView);
            // Transform our model with test data
            var testPredictions = model.Transform(testDataView);
            // Evaluate the model and Print the results
            Evaluate(context, testPredictions);

            // Create single prediction engine
            var predictionEngine = model.CreatePredictionEngine<SentenceData, PredictionData>(context);
            // Create the testing data
            var testData = new SentenceData() { Sentence = "Enfunda tu espada, saca tu baraja y prepárate para disfrutar con Hearthstone, un trepidante juego de cartas de estrategia, fácil de aprender y salvajemente divertido. Inicia una partida gratuita y utiliza tus mejores cartas para lanzar hechizos, invocar criaturas y dar órdenes a los héroes de Warcraft en épicos y estratégicos duelos." };
            // Predict the testing data
            var result = predictionEngine.Predict(testData);

            Console.WriteLine("Predicted language {0}", result.PredictedLabel);

            TweetTest.UsersTimelineTest(predictionEngine, "NetflixES", "es", 400);
            TweetTest.UsersTimelineTest(predictionEngine, "netflix", "en", 400);
            TweetTest.UsersTimelineTest(predictionEngine, "netflixturkiye", "tr", 400);
        }

        static void Main(string[] args)
        {
            // var model = Train();
            Train();
        }


        static void Evaluate(MLContext context, IDataView predictions)
        {
            var metrics = context.MulticlassClassification.Evaluate(predictions);
            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"Accuracy Macro: {metrics.AccuracyMacro}");
            Console.WriteLine($"Accuracy Micro: {metrics.AccuracyMicro}");
            // Console.WriteLine($"TopKAccuracy: {metrics.TopKAccuracy}");
            // Console.WriteLine($"LogLoss: {metrics.LogLoss}");
        }
    }
}
