using Newtonsoft.Json;

namespace Nickel.AI.OnnxPOC
{
    public class PretrainedConfig
    {
        private Dictionary<string, object> _configDictionary;

        public PretrainedConfig(Dictionary<string, object> config)
        {
            _configDictionary = config;
        }

        public object? GetConfig(string key)
        {
            if (_configDictionary.TryGetValue(key, out var value)) return value;
            return null;
        }

        public static PretrainedConfig FromFile(string configPath)
        {
            var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(configPath));

            if (config != null)
            {
                return new PretrainedConfig(config);
            }
            else
            {
                throw new ArgumentException("Unable to load PretrainedConfig from given path.", nameof(configPath));
            }
        }

        private static string StringValue(object? val, string defaultVal)
        {
            var strVal = val as string;
            if (strVal == null) return defaultVal;
            return strVal;
        }

        private static int IntegerValue(object? val, int defaultVal)
        {
            var strVal = val as string;
            if (int.TryParse(strVal, out int value)) return value;
            return defaultVal;
        }

        private static bool BooleanValue(object? val, bool defaultVal)
        {
            var strVal = val as string;
            if (strVal == null) return defaultVal;
            return strVal.Trim().ToLower().Equals("true");
        }

        //-------------------- PARAMETERS
        /// <summary>
        /// name_or_path (str, optional, defaults to "") — Store the string that was passed to PreTrainedModel.from_pretrained() 
        /// or TFPreTrainedModel.from_pretrained() as pretrained_model_name_or_path if the configuration was created 
        /// with such a method.
        /// </summary>
        public string? NameOrPath { get { return StringValue(GetConfig("_name_or_path"), ""); } }

        /// <summary>
        /// output_hidden_states (bool, optional, defaults to False) — Whether or not the model should return all hidden-states.
        /// </summary>
        public bool OutputHiddenStates { get { return BooleanValue(GetConfig("output_hidden_states"), false); } }

        /// <summary>
        /// output_attentions (bool, optional, defaults to False) — Whether or not the model should returns all attentions.
        /// </summary>
        public bool OutputAttentions { get { return BooleanValue(GetConfig("output_attentions"), false); } }

        /// <summary>
        /// return_dict (bool, optional, defaults to True) — Whether or not the model should return a ModelOutput instead of 
        /// a plain tuple.
        /// </summary>
        public bool ReturnDict { get { return BooleanValue(GetConfig("return_dict"), false); } }

        /// <summary>
        /// is_encoder_decoder (bool, optional, defaults to False) — Whether the model is used as an encoder/decoder or not.
        /// </summary>
        public bool IsEncoderDecoder { get { return BooleanValue(GetConfig("is_encoder_decoder"), false); } }

        /// <summary>
        /// is_decoder (bool, optional, defaults to False) — Whether the model is used as decoder or not (in which case it’s used as an encoder).
        /// </summary>
        public bool IsDecoder { get { return BooleanValue(GetConfig("is_decoder"), false); } }

        // cross_attention_hidden_size is omitted because it's documented as a bool but clearly a conditional size and
        // the usage isn't clear (currently)

        /// <summary>
        /// add_cross_attention (bool, optional, defaults to False) — Whether cross-attention layers should be added to the model. 
        /// Note, this option is only relevant for models that can be used as decoder models within the EncoderDecoderModel class, 
        /// which consists of all models in AUTO_MODELS_FOR_CAUSAL_LM.
        /// </summary>
        public bool AddCrossAttention { get { return BooleanValue(GetConfig("add_cross_attention"), false); } }

        /// <summary>
        /// tie_encoder_decoder (bool, optional, defaults to False) — Whether all encoder weights should be tied to their equivalent 
        /// decoder weights. This requires the encoder and decoder model to have the exact same parameter names.
        /// </summary>
        public bool TieEncoderDecoder { get { return BooleanValue(GetConfig("tie_encoder_decoder"), false); } }

        /// <summary>
        /// NOT IMPLEMENTED
        /// prune_heads (Dict[int, List[int]], optional, defaults to {}) — Pruned heads of the model. The keys are the selected layer 
        /// indices and the associated values, the list of heads to prune in said layer.
        /// 
        /// For instance {1: [0, 2], 2: [2, 3]} will prune heads 0 and 2 on layer 1 and heads 2 and 3 on layer 2.
        /// </summary>
        public Dictionary<int, List<int>> PruneHeads
        {
            get
            {
                // TODO: Implement reading val as dictionary
                return new Dictionary<int, List<int>>();
            }
        }

        /// <summary>
        /// chunk_size_feed_forward (int, optional, defaults to 0) — The chunk size of all feed forward layers in the residual attention 
        /// blocks. A chunk size of 0 means that the feed forward layer is not chunked. A chunk size of n means that the feed forward layer 
        /// processes n &lt; sequence_length embeddings at a time. 
        /// 
        /// For more information on feed forward chunking, see https://huggingface.co/docs/transformers/glossary.html#feed-forward-chunking.
        /// </summary>
        public int ChunkSizeFeedForward { get { return IntegerValue(GetConfig("chunk_size_feed_forward"), 0); } }
    }
}
