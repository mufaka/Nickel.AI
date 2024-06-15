namespace Nickel.AI.Logic
{
    public class Or : Sentence
    {
        private List<Sentence> disjuncts;

        public Or(params Sentence[] disjuncts)
        {
            foreach (var disjunct in disjuncts)
            {
                Sentence.Validate(disjunct);
            }
            this.disjuncts = new List<Sentence>(disjuncts);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return obj is Or or && disjuncts.SequenceEqual(or.disjuncts);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("or", disjuncts.Select(d => d.GetHashCode()).ToArray());
        }

        public override string ToString()
        {
            var disjunctsStr = string.Join(", ", disjuncts.Select(d => d.ToString()));
            return $"Or({disjunctsStr})";
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            return disjuncts.Any(disjunct => disjunct.Evaluate(model));
        }

        public override string Formula()
        {
            if (disjuncts.Count == 1)
            {
                return disjuncts[0].Formula();
            }
            return string.Join(" ∨ ", disjuncts.Select(d => Sentence.Parenthesize(d.Formula())));
        }

        public override HashSet<string> Symbols()
        {
            return new HashSet<string>(disjuncts.SelectMany(d => d.Symbols()));
        }
    }
}
