namespace Nickel.AI.Logic
{
    public class Implication : Sentence
    {
        private Sentence antecedent;
        private Sentence consequent;

        public Implication(Sentence antecedent, Sentence consequent)
        {
            Sentence.Validate(antecedent);
            Sentence.Validate(consequent);
            this.antecedent = antecedent;
            this.consequent = consequent;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return obj is Implication other &&
                   antecedent.Equals(other.antecedent) &&
                   consequent.Equals(other.consequent);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("implies", antecedent.GetHashCode(), consequent.GetHashCode());
        }

        public override string ToString()
        {
            return $"Implication({antecedent}, {consequent})";
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            return !antecedent.Evaluate(model) || consequent.Evaluate(model);
        }

        public override string Formula()
        {
            string antecedentFormula = Sentence.Parenthesize(antecedent.Formula());
            string consequentFormula = Sentence.Parenthesize(consequent.Formula());
            return $"{antecedentFormula} => {consequentFormula}";
        }

        public override HashSet<string> Symbols()
        {
            return new HashSet<string>(antecedent.Symbols().Union(consequent.Symbols()));
        }
    }
}
