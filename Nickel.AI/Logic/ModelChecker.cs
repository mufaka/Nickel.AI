namespace Nickel.AI.Logic
{
    public static class ModelChecker
    {
        public static bool ModelCheck(Sentence knowledge, Sentence query)
        {
            bool CheckAll(Sentence knowledge, Sentence query, HashSet<string> symbols, Dictionary<string, bool> model)
            {
                if (!symbols.Any())
                {
                    if (knowledge.Evaluate(model))
                    {
                        return query.Evaluate(model);
                    }
                    return true;
                }
                else
                {
                    var remaining = new HashSet<string>(symbols);
                    string p = remaining.First();
                    remaining.Remove(p);

                    var modelTrue = new Dictionary<string, bool>(model) { [p] = true };
                    var modelFalse = new Dictionary<string, bool>(model) { [p] = false };

                    return CheckAll(knowledge, query, remaining, modelTrue) &&
                           CheckAll(knowledge, query, remaining, modelFalse);
                }
            }

            var symbols = new HashSet<string>(knowledge.Symbols().Union(query.Symbols()));
            return CheckAll(knowledge, query, symbols, new Dictionary<string, bool>());
        }
    }
}
