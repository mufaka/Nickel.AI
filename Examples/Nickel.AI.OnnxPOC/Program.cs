using Microsoft.ML.OnnxRuntime;
using Newtonsoft.Json;
using Python.Runtime;

namespace Nickel.AI.OnnxPOC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // let's get hacky
            Runtime.PythonDLL = "python312.dll";
            PythonEngine.Initialize();

            // NOTE: Global Interpreter Lock; needs to be managed carefully so how can we
            //       safely provide static bindings?
            using (Py.GIL())
            {
                // how do we import a class from a module?

            }

            return;
            var modelPath = @"E:\Models\bart-large-cnn-onnx\model.onnx";

            // How do we know the inputs/outputs? Netron. Click on input_ids node and Model Properties appear
            /*
                MODEL PROPERTIES
                    format      ONNX v7
                    producer    pythorch 2.3.2
                    version     0
                    imports     ai.onnx v14
                    graph       main_graph

                INPUTS
                    input_ids       tensor:int64[batch_size, sequence_length]
                    attention_mask  tensor:int64[batch_size, sequence_length]

                OUTPUTS
                    logits          float32[batch_size,3]
            */

            //using var runOptions = new RunOptions();

            // InferenceSession - https://onnxruntime.ai/docs/api/csharp/api/Microsoft.ML.OnnxRuntime.InferenceSession.html
            using var session = new InferenceSession(modelPath);

            ShowModelProperties(session);
            ShowModelInputs(session);
            ShowModelOutputs(session);
            /*
                Output from 3 statements above:
                Model Properties
                        Custom Metadata Map:
                        Description:
                        Domain:
                        Graph Description:
                        Graph Name: main_graph
                        Producer Name: pytorch
                        Version: 9223372036854775807
                Input Metadata
                        input_ids
                                Dimensions: -1,-1
                                Element Data Type: Int64
                                Element Type: System.Int64
                                Is String: False
                                Is Tensor: True
                                Onnx Value Type: ONNX_TYPE_TENSOR
                                Symbolic Dimensions: batch_size,sequence_length
                        attention_mask
                                Dimensions: -1,-1
                                Element Data Type: Int64
                                Element Type: System.Int64
                                Is String: False
                                Is Tensor: True
                                Onnx Value Type: ONNX_TYPE_TENSOR
                                Symbolic Dimensions: batch_size,sequence_length
                Output Metadata
                        logits
                                Dimensions: -1,3
                                Element Data Type: Float
                                Element Type: System.Single
                                Is String: False
                                Is Tensor: True
                                Onnx Value Type: ONNX_TYPE_TENSOR
                                Symbolic Dimensions: batch_size,
            */

            // Can we infer (haha) how to prepare the inputs? There are several json configuration files in the onnx model folder
            // that may help. Documentation is hard to come by for those.

            // config.json - https://onnxruntime.ai/docs/genai/reference/config.html#configuration? This doesn't appear to match.
            /*
                https://huggingface.co/docs/transformers/main_classes/configuration#transformers.PretrainedConfig matches. 
                https://github.com/huggingface/transformers/blob/v4.43.3/src/transformers/configuration_utils.py#L50
            */

            var configPath = Path.Combine(Path.GetDirectoryName(modelPath), "config.json");
            var pretrainedConfig = PretrainedConfig.FromFile(configPath);

            Console.WriteLine("Configuration");
            Console.WriteLine(JsonConvert.SerializeObject(pretrainedConfig, Formatting.Indented));
        }


        /// <summary>
        /// Shows the model properties. This is similar to the information
        /// shown in Netron. 
        /// </summary>
        /// <param name="session">The InferenceSession</param>
        private static void ShowModelProperties(InferenceSession session)
        {
            Console.WriteLine("Model Properties");

            // ModelMetaData - https://onnxruntime.ai/docs/api/csharp/api/Microsoft.ML.OnnxRuntime.ModelMetadata.html
            var modelMetaData = session.ModelMetadata;

            Console.WriteLine($"\tCustom Metadata Map: {string.Join(',', modelMetaData.CustomMetadataMap)}");
            Console.WriteLine($"\tDescription: {modelMetaData.Description}");
            Console.WriteLine($"\tDomain: {modelMetaData.Domain}");
            Console.WriteLine($"\tGraph Description: {modelMetaData.GraphDescription}");
            Console.WriteLine($"\tGraph Name: {modelMetaData.GraphName}");
            Console.WriteLine($"\tProducer Name: {modelMetaData.ProducerName}");
            Console.WriteLine($"\tVersion: {modelMetaData.Version}");
        }

        /// <summary>
        /// Shows the model inputs. This is the same information that is shown in 
        /// Netron.
        /// </summary>
        /// <param name="session">The InferenceSession</param>
        private static void ShowModelInputs(InferenceSession session)
        {
            Console.WriteLine("Input Metadata");
            foreach (string key in session.InputMetadata.Keys)
            {
                // The key is the input name
                Console.WriteLine($"\t{key}");

                // NodeMetaData - https://onnxruntime.ai/docs/api/csharp/api/Microsoft.ML.OnnxRuntime.NodeMetadata.html
                var inputMetadata = session.InputMetadata[key];

                // NOTE: Only valid if this is a Tensor
                Console.WriteLine($"\t\tDimensions: {string.Join(',', inputMetadata.Dimensions)}");

                // NOTE: Only valid if this is a Tensor
                Console.WriteLine($"\t\tElement Data Type: {inputMetadata.ElementDataType}");

                // NOTE: Only valid if this is a Tensor
                Console.WriteLine($"\t\tElement Type: {inputMetadata.ElementType}");

                Console.WriteLine($"\t\tIs String: {inputMetadata.IsString}");
                Console.WriteLine($"\t\tIs Tensor: {inputMetadata.IsTensor}");

                Console.WriteLine($"\t\tOnnx Value Type: {inputMetadata.OnnxValueType}");

                // NOTE: Only valid if this is a Tensor
                Console.WriteLine($"\t\tSymbolic Dimensions: {string.Join(',', inputMetadata.SymbolicDimensions)}");
            }
        }

        private static void ShowModelOutputs(InferenceSession session)
        {
            Console.WriteLine("Output Metadata");

            foreach (string key in session.OutputMetadata.Keys)
            {
                // The key is the input name
                Console.WriteLine($"\t{key}");

                // NodeMetaData - https://onnxruntime.ai/docs/api/csharp/api/Microsoft.ML.OnnxRuntime.NodeMetadata.html
                var outputMetadata = session.OutputMetadata[key];

                // NOTE: Only valid if this is a Tensor
                Console.WriteLine($"\t\tDimensions: {string.Join(',', outputMetadata.Dimensions)}");

                // NOTE: Only valid if this is a Tensor
                Console.WriteLine($"\t\tElement Data Type: {outputMetadata.ElementDataType}");

                // NOTE: Only valid if this is a Tensor
                Console.WriteLine($"\t\tElement Type: {outputMetadata.ElementType}");

                Console.WriteLine($"\t\tIs String: {outputMetadata.IsString}");
                Console.WriteLine($"\t\tIs Tensor: {outputMetadata.IsTensor}");

                Console.WriteLine($"\t\tOnnx Value Type: {outputMetadata.OnnxValueType}");

                // NOTE: Only valid if this is a Tensor
                Console.WriteLine($"\t\tSymbolic Dimensions: {string.Join(',', outputMetadata.SymbolicDimensions)}");
            }
        }
    }
}
