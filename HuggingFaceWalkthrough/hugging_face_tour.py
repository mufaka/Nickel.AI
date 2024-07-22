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


if __name__ == '__main__':
    main()
