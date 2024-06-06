# About
This is an attempt to get RAG working in .NET with a local LLM but incrementally building up libraries that can be useful for other AI/ML tasks. It's mainly a learning / proof of concept exercise so not much in the way of developer documentation will be provided.

## TODO
### Large Dataset Handling
  1. ~~Split large csv files into memory manageable 'chunks'~~
  2. Provide an iterator to work with the chunked data as a Dataframe
      1. Add Map/Reduce capabilities

### Text Extraction
  1. ~~File based~~
  2. ~~URL based~~

### Chunking
  1. Explore semantic chunking of extracted text

### Tokenizing
   1. Understand tokenization and how that relates to embedding

### Embedding
  1. ~~Use local LLM for embeddings (chose Ollama for ease of use and available models)~~

### Vector database
  1. Choose between a local vector db (Weviate, Qdrant, Chroma, Milvus, etc)

### Data visualization and wrangling
  1. Visualize vector db
      1. How to represent so many dimensions easily
  2. Visualize large datasets
      1. Map/Reduce for mean/median/sum/unique values etc.
      2. Visual clustering of data with support for different types
          1. Experiment with tokenizing and embedding structured data

### API tying RAG pieces together
API should conform to ChatGPT API but pull semantically related information from vector database to provide as context for subsequent LLM call.

### UI for RAG based Q/A
SPA that uses RAG API, probably in React.

### Semantic Kernel orchestration plugins for AI based actions?
  1. How does Semantic Kernel know what plugins are applicable?

### Imperative Mood parsing
  1. For use in determining action items from a body of text. 
