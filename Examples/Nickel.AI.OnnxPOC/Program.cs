using Microsoft.ML;
using Microsoft.ML.Transforms.Onnx;

namespace Nickel.AI.OnnxPOC
{
    internal class Program
    {
        // NOTE: Working from https://github.com/dotnet/docs/blob/main/docs/machine-learning/how-to-guides/save-load-machine-learning-models-ml-net.md

        static void Main(string[] args)
        {
            // NOTE: hardcoding onnx model for now
            var onnxPath = @"E:\Models\bart-large-cnn-onnx\model.onnx";
            var mlContext = new MLContext();

            // TODO: Why is this called a scoring estimator? 
            OnnxScoringEstimator estimator = mlContext.Transforms.ApplyOnnxModel(onnxPath);

            // NOTE: Exception: The given version [14] is not supported, only version 1 to 10 is supported in this build.
            //       Unhandled exception. System.TypeInitializationException: The type initializer for
            //       'Microsoft.ML.OnnxRuntime.NativeMethods' threw an exception.

            //       There is an onnxruntime.dll in Windows\System32 that is outdated. Install the Nuget package
            //       for the ONNX runtime. Microsoft.ML.OnnxRuntime. This should have been noted in documentation.

            // OK, estimator is loaded, now what?
        }
    }
}
