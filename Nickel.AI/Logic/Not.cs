namespace Nickel.AI.Logic
{
    public class Not : Sentence
    {
        public Sentence Operand { get; }

        public Not(Sentence operand)
        {
            Sentence.Validate(operand);
            Operand = operand;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return obj is Not not && Operand.Equals(not.Operand);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("not", Operand.GetHashCode());
        }

        public override string ToString()
        {
            return $"Not({Operand})";
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            return !Operand.Evaluate(model);
        }

        public override string Formula()
        {
            return "¬" + Sentence.Parenthesize(Operand.Formula());
        }

        public override HashSet<string> Symbols()
        {
            return Operand.Symbols();
        }
    }
}
