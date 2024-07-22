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
