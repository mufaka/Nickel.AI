using Newtonsoft.Json;

namespace Nickel.AI.OnnxPOC
{
    /// <summary>
    /// Implementation of Hugging Face transformers PretrainedConfig for reading config.json files.
    /// https://huggingface.co/docs/transformers/main_classes/configuration#transformers.PretrainedConfig
    /// </summary>
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

        // NOTE: string is a reference type, so we can't overload StringValue with a
        //       nullable; have to name the method differently.

        private static string? StringValueOptional(object? val, string? defaultVal)
        {
            var strVal = val as string;
            if (strVal == null) return defaultVal;
            return strVal;
        }

        private static string StringValue(object? val, string defaultVal)
        {
            var strVal = val as string;
            if (strVal == null) return defaultVal;
            return strVal;
        }

        private static int? IntegerValue(object? val, int? defaultVal)
        {
            var strVal = val as string;
            if (int.TryParse(strVal, out int value)) return value;
            return defaultVal;
        }

        private static int IntegerValue(object? val, int defaultVal)
        {
            var strVal = val as string;
            if (int.TryParse(strVal, out int value)) return value;
            return defaultVal;
        }

        private static float? FloatValue(object? val, float? defaultVal)
        {
            var strVal = val as string;
            if (float.TryParse(strVal, out float value)) return value;
            return defaultVal;
        }

        private static float FloatValue(object? val, float defaultVal)
        {
            var strVal = val as string;
            if (float.TryParse(strVal, out float value)) return value;
            return defaultVal;
        }

        private static bool? BooleanValue(object? val, bool? defaultVal)
        {
            var strVal = val as string;
            if (strVal == null) return defaultVal;
            if (strVal.Trim().ToLower().Equals("true")) return true;
            if (strVal.Trim().ToLower().Equals("yes")) return true;
            if (strVal.Trim().ToLower().Equals("1")) return true;
            return false;
        }

        private static bool BooleanValue(object? val, bool defaultVal)
        {
            var strVal = val as string;
            if (strVal == null) return defaultVal;
            if (strVal.Trim().ToLower().Equals("true")) return true;
            if (strVal.Trim().ToLower().Equals("yes")) return true;
            if (strVal.Trim().ToLower().Equals("1")) return true;
            return false;
        }

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
        /// 
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

        /// <summary>
        /// max_length (int, optional, defaults to 20) — Maximum length that will be used by default in the generate method of the model.
        /// </summary>
        public int MaxLength { get { return IntegerValue(GetConfig("max_length"), 20); } }

        /// <summary>
        /// min_length (int, optional, defaults to 0) — Minimum length that will be used by default in the generate method of the model.
        /// </summary>
        public int MinLength { get { return IntegerValue(GetConfig("min_length"), 0); } }

        /// <summary>
        /// do_sample (bool, optional, defaults to False) — Flag that will be used by default in the generate method of the model. 
        /// Whether or not to use sampling ; use greedy decoding otherwise.
        /// </summary>
        public bool DoSample { get { return BooleanValue(GetConfig("do_sample"), false); } }

        /// <summary>
        /// early_stopping (bool, optional, defaults to False) — Flag that will be used by default in the generate method of the model. 
        /// Whether to stop the beam search when at least num_beams sentences are finished per batch or not.
        /// </summary>
        public bool EarlyStopping { get { return BooleanValue(GetConfig("early_stopping"), false); } }

        /// <summary>
        /// num_beams (int, optional, defaults to 1) — Number of beams for beam search that will be used by default in the generate 
        /// method of the model. 1 means no beam search.
        /// </summary>
        public int NumBeams { get { return IntegerValue(GetConfig("num_beams"), 1); } }

        /// <summary>
        /// num_beam_groups (int, optional, defaults to 1) — Number of groups to divide num_beams into in order to ensure diversity among 
        /// different groups of beams that will be used by default in the generate method of the model. 1 means no group beam search.
        /// </summary>
        public int NumBeamGroups { get { return IntegerValue(GetConfig("num_beams_groups"), 1); } }

        /// <summary>
        /// diversity_penalty (float, optional, defaults to 0.0) — Value to control diversity for group beam search. that will be used by 
        /// default in the generate method of the model. 0 means no diversity penalty. The higher the penalty, the more diverse are the outputs.
        /// </summary>
        public float DiversityPenalty { get { return FloatValue(GetConfig("diversity_penalty"), 0.0f); } }

        /// <summary>
        /// temperature (float, optional, defaults to 1.0) — The value used to module the next token probabilities that will be used by 
        /// default in the generate method of the model. Must be strictly positive.
        /// </summary>
        public float Temperature { get { return FloatValue(GetConfig("temperature"), 1.0f); } }

        /// <summary>
        /// top_k (int, optional, defaults to 50) — Number of highest probability vocabulary tokens to keep for top-k-filtering that will 
        /// be used by default in the generate method of the model.
        /// </summary>
        public int TopK { get { return IntegerValue(GetConfig("top_k"), 50); } }

        /// <summary>
        /// top_p (float, optional, defaults to 1) — Value that will be used by default in the generate method of the model for top_p. If set 
        /// to float < 1, only the most probable tokens with probabilities that add up to top_p or higher are kept for generation.
        /// </summary>
        public float TopP { get { return FloatValue(GetConfig("top_p"), 1.0f); } }

        /// <summary>
        /// typical_p (float, optional, defaults to 1) — Local typicality measures how similar the conditional probability of predicting a target 
        /// token next is to the expected conditional probability of predicting a random token next, given the partial text already generated. 
        /// If set to float < 1, the smallest set of the most locally typical tokens with probabilities that add up to typical_p or higher are kept 
        /// for generation. See https://arxiv.org/pdf/2202.00666.pdf.
        /// </summary>
        public float TypicalP { get { return FloatValue(GetConfig("typical_p"), 1.0f); } }

        /// <summary>
        /// repetition_penalty (float, optional, defaults to 1) — Parameter for repetition penalty that will be used by default in the generate 
        /// method of the model. 1.0 means no penalty.
        /// </summary>
        public float RepetitionPenalty { get { return FloatValue(GetConfig("repetition_penalty"), 1.0f); } }

        /// <summary>
        /// length_penalty (float, optional, defaults to 1) — Exponential penalty to the length that is used with beam-based generation. It is 
        /// applied as an exponent to the sequence length, which in turn is used to divide the score of the sequence. Since the score is the log 
        /// likelihood of the sequence (i.e. negative), length_penalty > 0.0 promotes longer sequences, while length_penalty < 0.0 encourages 
        /// shorter sequences.
        /// </summary>
        public float LengthPenalty { get { return FloatValue(GetConfig("length_penalty"), 1.0f); } }

        /// <summary>
        /// no_repeat_ngram_size (int, optional, defaults to 0) — Value that will be used by default in the — generate method of the model for 
        /// no_repeat_ngram_size. If set to int > 0, all ngrams of that size can only occur once.
        /// </summary>
        public int NoRepeatNgramSize { get { return IntegerValue(GetConfig("no_repeat_ngram_size"), 0); } }

        /// <summary>
        /// encoder_no_repeat_ngram_size (int, optional, defaults to 0) — Value that will be used by — default in the generate method of the model 
        /// for encoder_no_repeat_ngram_size. If set to int > 0, all ngrams of that size that occur in the encoder_input_ids cannot occur in the 
        /// decoder_input_ids.
        /// </summary>
        public int EncoderNoRepeatNgramSize { get { return IntegerValue(GetConfig("encoder_no_repeat_ngram_size"), 0); } }

        /// <summary>
        /// NOT IMPLEMENTED
        /// 
        /// bad_words_ids (List[int], optional) — List of token ids that are not allowed to be generated that will be used by default in the generate 
        /// method of the model. In order to get the tokens of the words that should not appear in the generated text, use 
        /// tokenizer.encode(bad_word, add_prefix_space=True).
        /// </summary>
        public List<int> BadWordsIds
        {
            get
            {
                // TODO: Implement reading List
                return new List<int>();
            }
        }

        /// <summary>
        /// num_return_sequences (int, optional, defaults to 1) — Number of independently computed returned sequences for each element in the batch 
        /// that will be used by default in the generate method of the model.
        /// </summary>
        public int NumReturnSequences { get { return IntegerValue(GetConfig("num_return_sequences"), 1); } }

        /// <summary>
        /// output_scores (bool, optional, defaults to False) — Whether the model should return the logits when used for generation.
        /// </summary>
        public bool OutputScores { get { return BooleanValue(GetConfig("output_scores"), false); } }

        /// <summary>
        /// return_dict_in_generate (bool, optional, defaults to False) — Whether the model should return a ModelOutput instead of a torch.LongTensor.
        /// </summary>
        public bool ReturnDictInGenerate { get { return BooleanValue(GetConfig("return_dict_in_generate"), false); } }

        /// <summary>
        /// forced_bos_token_id (int, optional) — The id of the token to force as the first generated token after the decoder_start_token_id. Useful 
        /// for multilingual models like mBART where the first generated token needs to be the target language token.
        /// </summary>
        public int? ForcedBosTokenId { get { return IntegerValue(GetConfig("forced_bos_token_id"), null); } }

        /// <summary>
        /// forced_eos_token_id (int, optional) — The id of the token to force as the last generated token when max_length is reached.
        /// </summary>
        public int? ForcedEosTokenId { get { return IntegerValue(GetConfig("forced_eos_token_id"), null); } }

        /// <summary>
        /// remove_invalid_values (bool, optional) — Whether to remove possible nan and inf outputs of the model to prevent the generation 
        /// method to crash. Note that using remove_invalid_values can slow down generation.
        /// </summary>
        public bool? RemoveInvalidValues { get { return BooleanValue(GetConfig(""), null); } }

        /// <summary>
        /// NOT IMPLEMENTED
        /// 
        /// architectures (List[str], optional) — Model architectures that can be used with the model pretrained weights.
        /// </summary>
        public List<string>? Architectures
        {
            /*
              "architectures": [
                "BartForConditionalGeneration"
              ],
            */
            get
            {
                // TODO: read array into list of string
                return new List<string>();
            }
        }

        /// <summary>
        /// finetuning_task (str, optional) — Name of the task used to fine-tune the model. This can be used when converting from an 
        /// original (TensorFlow or PyTorch) checkpoint.
        /// </summary>
        public string? FinetuningTask { get { return StringValueOptional(GetConfig("finetuning_task"), null); } }

        /// <summary>
        /// NOT IMPLEMENTED
        /// 
        /// id2label (Dict[int, str], optional) — A map from index (for instance prediction index, or target index) to label.
        /// </summary>
        public Dictionary<int, string>? Id2Label
        {
            /*
              "id2label": {
                "0": "LABEL_0",
                "1": "LABEL_1",
                "2": "LABEL_2"
              },
            */
            get
            {
                // TODO: implement reading dictionary
                return new Dictionary<int, string>();
            }
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// 
        /// label2id (Dict[str, int], optional) — A map from label to index for the model.
        /// </summary>
        public Dictionary<int, string>? Label2Id
        {
            get
            {
                // TODO: implement reading dictionary
                return new Dictionary<int, string>();
            }
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// 
        /// task_specific_params (Dict[str, Any], optional) — Additional keyword arguments to store for the current task.
        /// </summary>
        public Dictionary<string, object>? TaskSpecificParams
        {
            /*
                NOTE: These parameters seem to override base parameters as well as, possibly, define their own. eg:

                 "task_specific_params": {
                    "summarization": {
                      "early_stopping": true,
                      "length_penalty": 2.0,
                      "max_length": 142,
                      "min_length": 56,
                      "no_repeat_ngram_size": 3,
                      "num_beams": 4
                    }
                  },
            */
            get
            {
                return new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// problem_type (str, optional) — Problem type for XxxForSequenceClassification models. Can be one of "regression", 
        /// "single_label_classification" or "multi_label_classification".
        /// </summary>
        public string? ProblemType { get { return StringValueOptional(GetConfig("problem_type"), null); } }

        /// <summary>
        /// tokenizer_class (str, optional) — The name of the associated tokenizer class to use (if none is set, will use the tokenizer associated to the model by default).
        /// </summary>
        public string? TokenizerClass { get { return StringValueOptional(GetConfig("tokenizer_class"), null); } }

        /// <summary>
        /// prefix (str, optional) — A specific prompt that should be added at the beginning of each text before calling the model.
        /// </summary>
        public string? Prefix { get { return StringValueOptional(GetConfig("prefix"), null); } }

        /// <summary>
        /// bos_token_id (int, optional) — The id of the beginning-of-stream token.
        /// </summary>
        public int? BosTokenId { get { return IntegerValue(GetConfig("bos_token_id"), null); } }

        /// <summary>
        /// pad_token_id (int, optional) — The id of the padding token.
        /// </summary>
        public int? PadTokenId { get { return IntegerValue(GetConfig("pad_token_id"), null); } }

        /// <summary>
        /// eos_token_id (int, optional) — The id of the end-of-stream token.
        /// </summary>
        public int? EosTokenId { get { return IntegerValue(GetConfig("eos_token_id"), null); } }

        /// <summary>
        /// decoder_start_token_id (int, optional) — If an encoder-decoder model starts decoding with a different token than bos, the id of that token.
        /// </summary>
        public int? DecoderStartTokenId { get { return IntegerValue(GetConfig("decoder_start_token_id"), null); } }

        /// <summary>
        /// sep_token_id (int, optional) — The id of the separation token.
        /// </summary>
        public int? SepTokenId { get { return IntegerValue(GetConfig("sep_token_id"), null); } }

        /// <summary>
        /// torchscript (bool, optional, defaults to False) — Whether or not the model should be used with Torchscript.
        /// </summary>
        public bool Torchscript { get { return BooleanValue(GetConfig("torchscript"), false); } }

        /// <summary>
        /// tie_word_embeddings (bool, optional, defaults to True) — Whether the model’s input and output word embeddings should be tied. 
        /// Note that this is only relevant if the model has a output word embedding layer.
        /// </summary>
        public bool TieWordEmbeddings { get { return BooleanValue(GetConfig("tie_word_embeddings"), true); } }

        /// <summary>
        /// torch_dtype (str, optional) — The dtype of the weights. This attribute can be used to initialize the model to a non-default dtype 
        /// (which is normally float32) and thus allow for optimal storage allocation. For example, if the saved model is float16, ideally we 
        /// want to load it back using the minimal amount of memory needed to load float16 weights. Since the config object is stored in plain text, 
        /// this attribute contains just the floating type string without the torch. prefix. For example, for torch.float16 `torch_dtype is the 
        /// "float16" string.
        /// 
        /// This attribute is currently not being used during model loading time, but this may change in the future versions.But we can already start 
        /// preparing for the future by saving the dtype with save_pretrained.
        /// </summary>
        public string? TorchDType { get { return StringValueOptional(GetConfig("torch_dtype"), null); } }

        /// <summary>
        /// use_bfloat16 (bool, optional, defaults to False) — Whether or not the model should use BFloat16 scalars (only used by some TensorFlow models).
        /// </summary>
        public bool UseBFloat16 { get { return BooleanValue(GetConfig("use_bfloat16"), false); } }

        /// <summary>
        /// tf_legacy_loss (bool, optional, defaults to False) — Whether the model should use legacy TensorFlow losses. Legacy losses have variable output 
        /// shapes and may not be XLA-compatible. This option is here for backward compatibility and will be removed in Transformers v5.
        /// </summary>
        public bool TFLegacyLoss { get { return BooleanValue(GetConfig("tf_legacy_loss"), false); } }


        // NOTE: The below properties aren't necessarily in the configuration file. I think model_type has to be, but is_composition, keys_to_ignore_at_inference
        //       and attribute_map don't. They are overriden in the model specific configuration classes (eg: BartConfig)
        //
        //       For a list of all supported models and configurations:
        //       https://github.com/huggingface/transformers/blob/44f6fdd74f84744b159fa919474fd3108311a906/src/transformers/models/auto/configuration_auto.py

        /// <summary>
        /// model_type (str) — An identifier for the model type, serialized into the JSON file, and used to recreate the correct object in AutoConfig.
        /// </summary>
        public string ModelType { get { return StringValue(GetConfig("model_type"), string.Empty); } }

        /// <summary>
        /// is_composition (bool) — Whether the config class is composed of multiple sub-configs. In this case the config has to be initialized from 
        /// two or more configs of type PretrainedConfig like: EncoderDecoderConfig or ~RagConfig.
        /// </summary>
        public bool IsComposition { get { return BooleanValue(GetConfig("is_composition"), false); } }

        /// <summary>
        /// NOT IMPLEMENTED
        /// 
        /// keys_to_ignore_at_inference (List[str]) — A list of keys to ignore by default when looking at dictionary outputs of the model during inference.
        /// </summary>
        public List<string> KeysToIgnoreAtInference
        {
            get
            {
                // TODO: implement reading list of string
                return new List<string>();
            }
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// 
        /// attribute_map (Dict[str, str]) — A dict that maps model specific attribute names to the standardized naming of attributes.
        /// </summary>
        public Dictionary<string, string> AttributeMap
        {
            get
            {
                // TODO: implement reading dictionary
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// vocab_size (int) — The number of tokens in the vocabulary, which is also the first dimension of the embeddings matrix 
        /// (this attribute may be missing for models that don’t have a text modality like ViT).
        /// </summary>
        public int VocabSize { get { return IntegerValue(GetConfig("vocab_size"), 0); } }

        /// <summary>
        /// hidden_size (int) — The hidden size of the model.
        /// </summary>
        public int HiddenSize { get { return IntegerValue(GetConfig("hidden_size"), 0); } }

        /// <summary>
        /// num_attention_heads (int) — The number of attention heads used in the multi-head attention layers of the model.
        /// </summary>
        public int NumAttentionHeads { get { return IntegerValue(GetConfig("num_attention_heads"), 0); } }

        /// <summary>
        /// num_hidden_layers (int) — The number of blocks in the model.
        /// </summary>
        public int NumHiddenLayers { get { return IntegerValue(GetConfig("num_hidden_layers"), 0); } }
    }
}
