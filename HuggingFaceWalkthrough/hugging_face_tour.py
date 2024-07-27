# pip install -r requirements.txt
from enum import StrEnum
from transformers import pipeline, AutoTokenizer, AutoModelForSequenceClassification, TFAutoModelForSequenceClassification
from optimum.onnxruntime import ORTModelForSequenceClassification

# map a task to pipeline identifier; make identifier accessible to 
# code completion.
class PipelineIdentifiers(StrEnum):
    TEXT_CLASSIFICATION = "sentiment-analysis"
    TEXT_GENERATION = "text-generation"
    SUMMARIZATION = "summarization"
    IMAGE_CLASSIFICATION = "image-classification"
    IMAGE_SEGMENTATION = "image-segmentation"
    OBJECT_DETECTION = "object-detection"
    AUDIO_CLASSIFICATION = "audio-classification"
    AUTO_SPEECH_RECOGNITION = "automatic-speech-recognition"
    VISUAL_QUESTION_ANSWERING = "vqa"
    DOCUMENT_QUESTION_ANSWERING = "document-question-answering"
    IMAGE_CAPTIONING = "image-to-text"


# following https://huggingface.co/docs/transformers/quicktour
def main():
    sentiment_analysis_test()
    other_model_test()
    summarize()
    #export_model_onnx()


def sentiment_analysis_test():
    classifier = pipeline(PipelineIdentifiers.TEXT_CLASSIFICATION);

    # The pipeline() downloads and caches a default pretrained model and tokenizer for sentiment analysis. 
    # Now you can use the classifier on your target text. NOTE: No model given, there must be default? What is it?
    results = classifier(["We are very happy to show you the Transformers library.", "We hope you don't hate it."])

    # NOTE: No model was supplied, defaulted to distilbert/distilbert-base-uncased-finetuned-sst-2-english and revision af0f99b 
    #   (https://huggingface.co/distilbert/distilbert-base-uncased-finetuned-sst-2-english).

    for result in results:
        print(f"label: {result['label']}, with score: {round(result['score'], 4)}")

    # pipeline() makes things super easy, need to understand what's going on underneath..

    # NOTE: automatically downloads to C:\Users\username\.cache\huggingface\transformers on Windows, but directory empty on my machine
    #   HF_HOME environment variable gives alternate path...
    #   Config files downloaded automatically:
    #        %HF_HOME%hub\<model name>\snapshots\<guid>\config.json
    #        %HF_HOME%hub\<model name>\snapshots\<guid>\model.safetensors
    #        %HF_HOME%hub\<model name>\snapshots\<guid>\tokenizer_config.json
    #        %HF_HOME%hub\<model name>\snapshots\<guid>\vocab.txt


def other_model_test():
    model_name = "nlptown/bert-base-multilingual-uncased-sentiment"

    # NOTE: two different ways depending on PyTorch vs TF.
    model = AutoModelForSequenceClassification.from_pretrained(model_name)
    tokenizer = AutoTokenizer.from_pretrained(model_name)

    classifier = pipeline(PipelineIdentifiers.TEXT_CLASSIFICATION, model=model, tokenizer=tokenizer)

    results = classifier("Nous sommes très heureux de vous présenter la bibliothèque 🤗 Transformers.");

    for result in results:
        print(f"label: {result['label']}, with score: {round(result['score'], 4)}")

# this will export a model to onnx in a subdirectory under the executable. WARN: don't include in git commits...
def export_model_onnx():
    model_checkpoint = "facebook/bart-large-cnn"
    save_directory = "onnx/"

    ort_model = ORTModelForSequenceClassification.from_pretrained(model_checkpoint, export=True)
    tokenizer = AutoTokenizer.from_pretrained(model_checkpoint)

    ort_model.save_pretrained(save_directory)
    tokenizer.save_pretrained(save_directory)


# TODO: Can we replicate the following in ML.Net with the exported onnx model?


def summarize():
    summarizer = pipeline(PipelineIdentifiers.SUMMARIZATION, model="facebook/bart-large-cnn")

    ARTICLE = """ New York (CNN)When Liana Barrientos was 23 years old, she got married in Westchester County, New York.
    A year later, she got married again in Westchester County, but to a different man and without divorcing her first husband.
    Only 18 days after that marriage, she got hitched yet again. Then, Barrientos declared "I do" five more times, sometimes only within two weeks of each other.
    In 2010, she married once more, this time in the Bronx. In an application for a marriage license, she stated it was her "first and only" marriage.
    Barrientos, now 39, is facing two criminal counts of "offering a false instrument for filing in the first degree," referring to her false statements on the
    2010 marriage license application, according to court documents.
    Prosecutors said the marriages were part of an immigration scam.
    On Friday, she pleaded not guilty at State Supreme Court in the Bronx, according to her attorney, Christopher Wright, who declined to comment further.
    After leaving court, Barrientos was arrested and charged with theft of service and criminal trespass for allegedly sneaking into the New York subway through an emergency exit, said Detective
    Annette Markowski, a police spokeswoman. In total, Barrientos has been married 10 times, with nine of her marriages occurring between 1999 and 2002.
    All occurred either in Westchester County, Long Island, New Jersey or the Bronx. She is believed to still be married to four men, and at one time, she was married to eight men at once, prosecutors say.
    Prosecutors said the immigration scam involved some of her husbands, who filed for permanent residence status shortly after the marriages.
    Any divorces happened only after such filings were approved. It was unclear whether any of the men will be prosecuted.
    The case was referred to the Bronx District Attorney\'s Office by Immigration and Customs Enforcement and the Department of Homeland Security\'s
    Investigation Division. Seven of the men are from so-called "red-flagged" countries, including Egypt, Turkey, Georgia, Pakistan and Mali.
    Her eighth husband, Rashid Rajput, was deported in 2006 to his native Pakistan after an investigation by the Joint Terrorism Task Force.
    If convicted, Barrientos faces up to four years in prison.  Her next court appearance is scheduled for May 18.
    """

    print(summarizer(ARTICLE, max_length=130, min_length=30, do_sample=False))


if __name__ == '__main__':
    main()

"""
    In order to get similar functionality in C#, understanding huggingface transformers library is required. There is a lot
    of magic happening that makes what should be simple, actually simple. 

    eg: The example above get's a summarizer using the defined model from the pipeline factory method, calls it with a text parameter and some other
        parameters, and then gets a result.

        Hidden away is what tokenizer is used, the shape of the input(s), converting the text to input, and then converting output to text.

        .NET has a long way to go to achieve this but it's not even clear if ML.Net or even the onnx team is going that direction. 
"""

# https://github.com/huggingface/transformers/blob/c85510f958e6955d88ea1bafb4f320074bfbd0c1/src/transformers/pipelines/__init__.py#L571
"""
Utility factory method to build a [`Pipeline`].

Pipelines are made of:

    - A [tokenizer](tokenizer) in charge of mapping raw textual input to token.
    - A [model](model) to make predictions from the inputs.
    - Some (optional) post processing for enhancing model's output.

Args:
    task (`str`):
        The task defining which pipeline will be returned. Currently accepted tasks are:

        - `"audio-classification"`: will return a [`AudioClassificationPipeline`].
        - `"automatic-speech-recognition"`: will return a [`AutomaticSpeechRecognitionPipeline`].
        - `"depth-estimation"`: will return a [`DepthEstimationPipeline`].
        - `"document-question-answering"`: will return a [`DocumentQuestionAnsweringPipeline`].
        - `"feature-extraction"`: will return a [`FeatureExtractionPipeline`].
        - `"fill-mask"`: will return a [`FillMaskPipeline`]:.
        - `"image-classification"`: will return a [`ImageClassificationPipeline`].
        - `"image-feature-extraction"`: will return an [`ImageFeatureExtractionPipeline`].
        - `"image-segmentation"`: will return a [`ImageSegmentationPipeline`].
        - `"image-to-image"`: will return a [`ImageToImagePipeline`].
        - `"image-to-text"`: will return a [`ImageToTextPipeline`].
        - `"mask-generation"`: will return a [`MaskGenerationPipeline`].
        - `"object-detection"`: will return a [`ObjectDetectionPipeline`].
        - `"question-answering"`: will return a [`QuestionAnsweringPipeline`].
        - `"summarization"`: will return a [`SummarizationPipeline`].
        - `"table-question-answering"`: will return a [`TableQuestionAnsweringPipeline`].
        - `"text2text-generation"`: will return a [`Text2TextGenerationPipeline`].
        - `"text-classification"` (alias `"sentiment-analysis"` available): will return a [`TextClassificationPipeline`].
        - `"text-generation"`: will return a [`TextGenerationPipeline`]:.
        - `"text-to-audio"` (alias `"text-to-speech"` available): will return a [`TextToAudioPipeline`]:.
        - `"token-classification"` (alias `"ner"` available): will return a [`TokenClassificationPipeline`].
        - `"translation"`: will return a [`TranslationPipeline`].
        - `"translation_xx_to_yy"`: will return a [`TranslationPipeline`].
        - `"video-classification"`: will return a [`VideoClassificationPipeline`].
        - `"visual-question-answering"`: will return a [`VisualQuestionAnsweringPipeline`].
        - `"zero-shot-classification"`: will return a [`ZeroShotClassificationPipeline`].
        - `"zero-shot-image-classification"`: will return a [`ZeroShotImageClassificationPipeline`].
        - `"zero-shot-audio-classification"`: will return a [`ZeroShotAudioClassificationPipeline`].
        - `"zero-shot-object-detection"`: will return a [`ZeroShotObjectDetectionPipeline`].

    model (`str` or [`PreTrainedModel`] or [`TFPreTrainedModel`], *optional*):
        The model that will be used by the pipeline to make predictions. This can be a model identifier or an
        actual instance of a pretrained model inheriting from [`PreTrainedModel`] (for PyTorch) or
        [`TFPreTrainedModel`] (for TensorFlow).

        If not provided, the default for the `task` will be loaded.
    config (`str` or [`PretrainedConfig`], *optional*):
        The configuration that will be used by the pipeline to instantiate the model. This can be a model
        identifier or an actual pretrained model configuration inheriting from [`PretrainedConfig`].

        If not provided, the default configuration file for the requested model will be used. That means that if
        `model` is given, its default configuration will be used. However, if `model` is not supplied, this
        `task`'s default model's config is used instead.
    tokenizer (`str` or [`PreTrainedTokenizer`], *optional*):
        The tokenizer that will be used by the pipeline to encode data for the model. This can be a model
        identifier or an actual pretrained tokenizer inheriting from [`PreTrainedTokenizer`].

        If not provided, the default tokenizer for the given `model` will be loaded (if it is a string). If `model`
        is not specified or not a string, then the default tokenizer for `config` is loaded (if it is a string).
        However, if `config` is also not given or not a string, then the default tokenizer for the given `task`
        will be loaded.
    feature_extractor (`str` or [`PreTrainedFeatureExtractor`], *optional*):
        The feature extractor that will be used by the pipeline to encode data for the model. This can be a model
        identifier or an actual pretrained feature extractor inheriting from [`PreTrainedFeatureExtractor`].

        Feature extractors are used for non-NLP models, such as Speech or Vision models as well as multi-modal
        models. Multi-modal models will also require a tokenizer to be passed.

        If not provided, the default feature extractor for the given `model` will be loaded (if it is a string). If
        `model` is not specified or not a string, then the default feature extractor for `config` is loaded (if it
        is a string). However, if `config` is also not given or not a string, then the default feature extractor
        for the given `task` will be loaded.
    framework (`str`, *optional*):
        The framework to use, either `"pt"` for PyTorch or `"tf"` for TensorFlow. The specified framework must be
        installed.

        If no framework is specified, will default to the one currently installed. If no framework is specified and
        both frameworks are installed, will default to the framework of the `model`, or to PyTorch if no model is
        provided.
    revision (`str`, *optional*, defaults to `"main"`):
        When passing a task name or a string model identifier: The specific model version to use. It can be a
        branch name, a tag name, or a commit id, since we use a git-based system for storing models and other
        artifacts on huggingface.co, so `revision` can be any identifier allowed by git.
    use_fast (`bool`, *optional*, defaults to `True`):
        Whether or not to use a Fast tokenizer if possible (a [`PreTrainedTokenizerFast`]).
    use_auth_token (`str` or *bool*, *optional*):
        The token to use as HTTP bearer authorization for remote files. If `True`, will use the token generated
        when running `huggingface-cli login` (stored in `~/.huggingface`).
    device (`int` or `str` or `torch.device`):
        Defines the device (*e.g.*, `"cpu"`, `"cuda:1"`, `"mps"`, or a GPU ordinal rank like `1`) on which this
        pipeline will be allocated.
    device_map (`str` or `Dict[str, Union[int, str, torch.device]`, *optional*):
        Sent directly as `model_kwargs` (just a simpler shortcut). When `accelerate` library is present, set
        `device_map="auto"` to compute the most optimized `device_map` automatically (see
        [here](https://huggingface.co/docs/accelerate/main/en/package_reference/big_modeling#accelerate.cpu_offload)
        for more information).

        <Tip warning={true}>

        Do not use `device_map` AND `device` at the same time as they will conflict

        </Tip>

    torch_dtype (`str` or `torch.dtype`, *optional*):
        Sent directly as `model_kwargs` (just a simpler shortcut) to use the available precision for this model
        (`torch.float16`, `torch.bfloat16`, ... or `"auto"`).
    trust_remote_code (`bool`, *optional*, defaults to `False`):
        Whether or not to allow for custom code defined on the Hub in their own modeling, configuration,
        tokenization or even pipeline files. This option should only be set to `True` for repositories you trust
        and in which you have read the code, as it will execute code present on the Hub on your local machine.
    model_kwargs (`Dict[str, Any]`, *optional*):
        Additional dictionary of keyword arguments passed along to the model's `from_pretrained(...,
        **model_kwargs)` function.
    kwargs (`Dict[str, Any]`, *optional*):
        Additional keyword arguments passed along to the specific pipeline init (see the documentation for the
        corresponding pipeline class for possible values).

Returns:
    [`Pipeline`]: A suitable pipeline for the task.

Examples:

```python
>>> from transformers import pipeline, AutoModelForTokenClassification, AutoTokenizer

>>> # Sentiment analysis pipeline
>>> analyzer = pipeline("sentiment-analysis")

>>> # Question answering pipeline, specifying the checkpoint identifier
>>> oracle = pipeline(
...     "question-answering", model="distilbert/distilbert-base-cased-distilled-squad", tokenizer="google-bert/bert-base-cased"
... )

>>> # Named entity recognition pipeline, passing in a specific model and tokenizer
>>> model = AutoModelForTokenClassification.from_pretrained("dbmdz/bert-large-cased-finetuned-conll03-english")
>>> tokenizer = AutoTokenizer.from_pretrained("google-bert/bert-base-cased")
>>> recognizer = pipeline("ner", model=model, tokenizer=tokenizer)
```"""