using FormulaFlow.Server.Intermediate.Formula.Model.Computable;
using FormulaFlow.Server.Intermediate.Formula.Model.ComputeRaw;
using System.Text.RegularExpressions;

namespace FormulaFlow.Server.Intermediate.Formula
{
    public class FormulaRegexPatternParser
    {
        private const string _unaryGroupName = "unaryGroup";
        private const string _binaryGroupName = "binaryGroup";
        private const string _variableGroupName = "variableGroup";

        private List<ContainerRegexFormula[]> _containers = new List<ContainerRegexFormula[]>();
        private List<VariableRegexFormula[]> _variableFormulas = new List<VariableRegexFormula[]>();
        private List<UnaryRegexFormula[]> _unaryFormulas = new List<UnaryRegexFormula[]>();
        private List<BinaryRegexFormula[]> _binaryFormulas = new List<BinaryRegexFormula[]>();

        private readonly string _variableComposite;

        private readonly string _startOfString;
        private readonly string _onGoingString;

        public FormulaRegexPatternParser(
                List<ContainerRegexFormula[]> containers,
                List<VariableRegexFormula[]> variables,
                List<UnaryRegexFormula[]> unary,
                List<BinaryRegexFormula[]> binary
            )
        {
            _containers = containers;
            _variableFormulas = variables;
            _unaryFormulas = unary;
            _binaryFormulas = binary;

            _variableComposite = @$"(?<{_variableGroupName}>{string.Join("|", _variableFormulas.SelectMany(arr => arr.Select(v => v.RegexSymbol))
                .Union(_containers.SelectMany(arr => arr.Select(c => c.RegexSymbol)))
                )})";

            var unaryString = $"(?<{_unaryGroupName}>{string.Join("|", _unaryFormulas.SelectMany(arr => arr.Select(u => u.RegexSymbol)))})";
            var binaryString = $"(?<{_binaryGroupName}>{string.Join("|", _binaryFormulas.SelectMany(arr => arr.Select(u => u.RegexSymbol)))})";

            _startOfString = @$"\s*{unaryString}?\s*{_variableComposite}\s*";
            _onGoingString = $@"\s*{binaryString}\s*{unaryString}?\s*{_variableComposite}\s*";
        }

        public IComputable CompileFormula(string formula, out Dictionary<int, int> buffer)
        {
            var raw = CreateRawComputable(formula);

            var compute = CreateCompute(raw, out buffer);

            return compute;
        }

        private IComputable CreateCompute(LinkedList<IComputeRaw> rawCompute, out Dictionary<int, int> bufferDict)
        {
            bufferDict = new Dictionary<int, int>();

            var toRaw = new Dictionary<IComputable, HashSet<IComputeRaw>>();
            var toCompute = new Dictionary<IComputeRaw, IComputable>();

            Action<IComputeRaw, IComputable> addToCollection = (raw, compute) =>
            {
                if (!toRaw.ContainsKey(compute))
                {
                    toRaw.Add(compute, new HashSet<IComputeRaw>());
                }

                toRaw[compute].Add(raw);
                toCompute.Add(raw, compute);
            };

            // parenthesis
            foreach (IComputeRaw raw in rawCompute)
            {
                if (raw is ComputeRawContainer container)
                {
                    var parenthesisCompute = CreateCompute(container.Children, out bufferDict);

                    addToCollection(raw, parenthesisCompute);
                }
            }

            // variables 
            foreach (IComputeRaw raw in rawCompute)
            {
                if (raw is ComputeRawString compStr && compStr.Type == ComputeRawStringType.Variable)
                {
                    var found = false;

                    foreach (var variable in _variableFormulas.SelectMany(v => v))
                    {
                        var buffer = variable.GetBuffer(compStr.Value);

                        if (buffer != null)
                        {
                            if (bufferDict.ContainsKey(buffer.Item1))
                            {
                                var value = bufferDict[buffer.Item1];

                                if (buffer.Item2 > value)
                                {
                                    bufferDict[buffer.Item1] = buffer.Item2;
                                }
                            }
                            else
                            {
                                bufferDict.Add(buffer.Item1, buffer.Item2);
                            }
                        }

                        var compute = variable.GetComputable(compStr.Value);

                        if (compute != null)
                        {
                            addToCollection(raw, compute);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        throw new ArgumentException();
                    }
                }
            }

            Action<IComputable, IComputable> updateTo = (oldComp, newComp) =>
            {
                var oldRaws = toRaw[oldComp];

                foreach (var raw in oldRaws)
                {
                    toCompute[raw] = newComp;
                }

                toRaw.Remove(oldComp);

                if (!toRaw.ContainsKey(newComp))
                {
                    toRaw.Add(newComp, new HashSet<IComputeRaw>());
                }

                foreach (var raw in oldRaws)
                {
                    toRaw[newComp].Add(raw);
                }
            };

            // Order of operations Unary
            foreach (var item in _unaryFormulas.SelectMany(u => u))
            {
                var node = rawCompute.First;

                while (node != null)
                {
                    if (node.Value is ComputeRawString rawString && rawString.Type == ComputeRawStringType.UnaryOperator)
                    {
                        if (Regex.IsMatch(rawString.Value, item.RegexSymbol))
                        {
                            if (node.Next != null && toCompute.ContainsKey(node.Next.Value))
                            {
                                var variableCompute = toCompute[node.Next.Value];

                                var unary = new UnaryOperationComputable
                                {
                                    Function = item.UnaryOperation,
                                    Right = variableCompute,
                                };

                                updateTo(variableCompute, unary);
                                addToCollection(node.Value, unary);
                            }
                            else
                            {
                                throw new ArgumentException();
                            }
                        }
                    }

                    node = node.Next;
                }
            }

            foreach (var item in _binaryFormulas.SelectMany(b => b))
            {
                var node = rawCompute.First;

                while (node != null)
                {
                    if (node.Value is ComputeRawString rawString && rawString.Type == ComputeRawStringType.BinaryOperator)
                    {
                        if (Regex.IsMatch(rawString.Value, item.RegexSymbol))
                        {
                            if (node.Next != null && toCompute.ContainsKey(node.Next.Value) &&
                                node.Previous != null && toCompute.ContainsKey(node.Previous.Value))
                            {
                                var prevVariableCompute = toCompute[node.Previous.Value];
                                var nextVariableCompute = toCompute[node.Next.Value];

                                var unary = new BinaryOperationComputable
                                {
                                    Function = item.BinaryOperation,
                                    Left = prevVariableCompute,
                                    Right = nextVariableCompute,
                                };

                                updateTo(prevVariableCompute, unary);
                                updateTo(nextVariableCompute, unary);
                                addToCollection(node.Value, unary);
                            }
                            else
                            {
                                throw new ArgumentException();
                            }
                        }
                    }

                    node = node.Next;
                }
            }

            if (toRaw.Count != 1)
            {
                throw new ArgumentException();
            }

            return toRaw.Keys.First();
        }

        private LinkedList<IComputeRaw> CreateRawComputable(string formula)
        {
            var computeStringBuilder = new LinkedList<IComputeRaw>();

            var start = new Regex(_startOfString);

            var match = start.Match(formula);

            if (match.Success)
            {
                TryAddUnaryGroup(computeStringBuilder, match);

                if (!TryAddPrimitiveVariableGroup(computeStringBuilder, match, out var newFormula))
                {
                    var parenCompute = CreateRawComputable(newFormula);

                    computeStringBuilder.AddLast(new LinkedListNode<IComputeRaw>(new ComputeRawContainer()
                    {
                        Children = parenCompute,
                    }));
                }
            }
            else
            {
                throw new ArgumentException();
            }

            var index = match.Index + match.Length;
            var next = new Regex(_onGoingString);

            while (index < formula.Length)
            {
                match = next.Match(formula, index);

                if (match.Success)
                {
                    TryAddBinaryGroup(computeStringBuilder, match);

                    TryAddUnaryGroup(computeStringBuilder, match);

                    if (!TryAddPrimitiveVariableGroup(computeStringBuilder, match, out var newFormula))
                    {
                        var parenCompute = CreateRawComputable(newFormula);

                        computeStringBuilder.AddLast(new LinkedListNode<IComputeRaw>(new ComputeRawContainer()
                        {
                            Children = parenCompute,
                        }));
                    }
                }
                else
                {
                    throw new ArgumentException();
                }

                index = match.Index + match.Length;
            }

            return computeStringBuilder;
        }

        private bool TryAddPrimitiveVariableGroup(LinkedList<IComputeRaw> collection, Match match, out string containerSubstance)
        {
            containerSubstance = string.Empty;

            var variable = match.Groups[_variableGroupName].Value;

            if (string.IsNullOrEmpty(variable))
            {
                throw new ArgumentException();
            }

            foreach (var containerArr in _containers)
            {
                foreach (var container in containerArr)
                {
                    var substance = container.GetSubstance(variable);

                    if (substance != null)
                    {
                        containerSubstance += substance;
                        return false;
                    }
                }
            }

            collection.AddLast(new LinkedListNode<IComputeRaw>(new ComputeRawString
            {
                Value = variable,
                Type = ComputeRawStringType.Variable
            }));

            return true;
        }

        private void TryAddUnaryGroup(LinkedList<IComputeRaw> collection, Match match)
        {
            var unary = match.Groups[_unaryGroupName].Value;

            if (!string.IsNullOrEmpty(unary))
            {
                collection.AddLast(new LinkedListNode<IComputeRaw>(new ComputeRawString
                {
                    Value = unary,
                    Type = ComputeRawStringType.UnaryOperator
                }));
            }
        }

        private void TryAddBinaryGroup(LinkedList<IComputeRaw> collection, Match match)
        {
            var binary = match.Groups[_binaryGroupName].Value;

            if (string.IsNullOrEmpty(binary))
            {
                throw new ArgumentException();
            }

            collection.AddLast(new LinkedListNode<IComputeRaw>(new ComputeRawString
            {
                Value = binary,
                Type = ComputeRawStringType.BinaryOperator,
            }));
        }

    }
}
