namespace FormulaFlow.Server.Intermediate.Formula
{
    public class FormulaRegexPatternBuilder
    {
        private List<ContainerRegexFormula[]> _containerFormulas = new List<ContainerRegexFormula[]>();
        private List<VariableRegexFormula[]> _variableFormulas = new List<VariableRegexFormula[]>();
        private List<UnaryRegexFormula[]> _unaryFormulas = new List<UnaryRegexFormula[]>();
        private List<BinaryRegexFormula[]> _binaryFormulas = new List<BinaryRegexFormula[]>();

        public void IncludeFormulas(ContainerRegexFormula[] container)
        {
            _containerFormulas.Add(container);
        }
        public void IncludeFormulas(VariableRegexFormula[] variable)
        {
            _variableFormulas.Add(variable);
        }
        public void IncludeFormulas(UnaryRegexFormula[] unary)
        {
            _unaryFormulas.Add(unary);
        }
        public void IncludeFormulas(BinaryRegexFormula[] binary)
        {
            _binaryFormulas.Add(binary);
        }

        public FormulaRegexPatternParser Build()
        {
            return new FormulaRegexPatternParser(
                    _containerFormulas,
                    _variableFormulas,
                    _unaryFormulas,
                    _binaryFormulas
                );
        }
    }
}
