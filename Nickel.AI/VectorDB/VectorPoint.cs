namespace Nickel.AI.VectorDB
{
    public class VectorPoint
    {
        // NOTE: Thought about forcing a Guid here because qdrant supports either guid or long for an ID but I think it's
        //       too restrictive considering current knowledge (zero) of other vector db's at the moment. This being an
        //       API kind of class means we need to be less restrictive. Opting for using an Id strategy for now (IIdStrategy).

        public string Id { get; set; }

        // qdrant client uses float
        public float[] Vectors { get; set; }

        // TODO: Is there a better generic representation of a Payload?
        public Dictionary<string, string>? Payload { get; set; }

        // TODO: Using VectorPoint as a result as well as indexing. Should we separate? Score is a property specific to results.
        public float? Score { get; set; }

    }
}
