namespace Nickel.AI.Logic
{
    public class And : Sentence
    {
        private List<Sentence> conjuncts;

        public And(params Sentence[] conjuncts)
        {
            foreach (var conjunct in conjuncts)
            {
                Sentence.Validate(conjunct);
            }
            this.conjuncts = new List<Sentence>(conjuncts);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return obj is And and && conjuncts.SequenceEqual(and.conjuncts);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("and", conjuncts.Select(c => c.GetHashCode()).ToArray());
        }

        public override string ToString()
        {
            var conjunctions = string.Join(", ", conjuncts.Select(c => c.ToString()));
            return $"And({conjunctions})";
        }

        public void Add(Sentence conjunct)
        {
            Sentence.Validate(conjunct);
            conjuncts.Add(conjunct);
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            return conjuncts.All(conjunct => conjunct.Evaluate(model));
        }

        public override string Formula()
        {
            if (conjuncts.Count == 1)
            {
                return conjuncts[0].Formula();
            }
            return string.Join(" ∧ ", conjuncts.Select(c => Sentence.Parenthesize(c.Formula())));
        }

        public override HashSet<string> Symbols()
        {
            return new HashSet<string>(conjuncts.SelectMany(c => c.Symbols()));
        }
    }
}
