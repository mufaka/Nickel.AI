namespace Nickel.AI.Logic
{
    public class Biconditional : Sentence
    {
        private Sentence left;
        private Sentence right;

        public Biconditional(Sentence left, Sentence right)
        {
            Sentence.Validate(left);
            Sentence.Validate(right);
            this.left = left;
            this.right = right;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return obj is Biconditional other &&
                   left.Equals(other.left) &&
                   right.Equals(other.right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("biconditional", left.GetHashCode(), right.GetHashCode());
        }

        public override string ToString()
        {
            return $"Biconditional({left}, {right})";
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            return (left.Evaluate(model) && right.Evaluate(model)) ||
                   (!left.Evaluate(model) && !right.Evaluate(model));
        }

        public override string Formula()
        {
            string leftFormula = Sentence.Parenthesize(left.Formula());
            string rightFormula = Sentence.Parenthesize(right.Formula());
            return $"{leftFormula} <=> {rightFormula}";
        }

        public override HashSet<string> Symbols()
        {
            return new HashSet<string>(left.Symbols().Union(right.Symbols()));
        }
    }
}
