using FormulaFlow.Server.Intermediate.Formula.Model.Computable;
using System.Text.RegularExpressions;

namespace FormulaFlow.Server.Intermediate.Formula
{

    public class RegexFormulaParsing
    {
        public static readonly UnaryRegexFormula[] NumericUnary = [
                new UnaryRegexFormula { Symbol = "-", RegexSymbol = "-", Description = "Negative Sign", UnaryOperation = n => -n }
            ];

        public static readonly BinaryRegexFormula[] NumericBinary = [
                new BinaryRegexFormula { Symbol = "pow", RegexSymbol = @"pow", Description = "x to POWER of y", BinaryOperation = (n1, n2) => Math.Pow(n1, n2) },
                new BinaryRegexFormula { Symbol = "*", RegexSymbol = @"\*", Description = "Multiply", BinaryOperation = (n1, n2) => n1 * n2 },
                new BinaryRegexFormula { Symbol = "/", RegexSymbol = @"\/", Description = "Divide", BinaryOperation = (n1, n2) => n1 / n2 },
                new BinaryRegexFormula { Symbol = "-", RegexSymbol = @"-", Description = "Subtract", BinaryOperation = (n1, n2) => n1 - n2 },
                new BinaryRegexFormula { Symbol = "+", RegexSymbol = @"\+", Description = "Add", BinaryOperation = (n1, n2) => n1 + n2 },
            ];

        public static readonly BinaryRegexFormula[] TransitionBinary = [
                new BinaryRegexFormula { Symbol = ">", RegexSymbol = @">", Description = "Greater Than", BinaryOperation = (n1, n2) => n1 > n2 },
                new BinaryRegexFormula { Symbol = ">=", RegexSymbol = @">=", Description = "Greater Than or Equal", BinaryOperation = (n1, n2) => n1 >= n2 },
                new BinaryRegexFormula { Symbol = "<", RegexSymbol = @"<", Description = "Less Than", BinaryOperation = (n1, n2) => n1 < n2 },
                new BinaryRegexFormula { Symbol = "<=", RegexSymbol = @"<=", Description = "Less Than or Equal", BinaryOperation = (n1, n2) => n1 <= n2 },
                new BinaryRegexFormula { Symbol = "==", RegexSymbol = @"==", Description = "Equal", BinaryOperation = (n1, n2) => n1 == n2 },
            ];

        public static readonly UnaryRegexFormula[] BooleanUnary = [
                new UnaryRegexFormula { Symbol = "!", RegexSymbol = "!", Description = "Logic NOT", UnaryOperation = b => !b },
            ];

        public static readonly BinaryRegexFormula[] BooleanBinary = [
                new BinaryRegexFormula { Symbol = "&&", RegexSymbol = @"&&", Description = "Logic AND", BinaryOperation = (b1, b2) => b1 && b2 },
                new BinaryRegexFormula { Symbol = "^", RegexSymbol = @"\^", Description = "Exclusive OR", BinaryOperation = (b1, b2) => b1 ^ b2 },
                new BinaryRegexFormula { Symbol = "||", RegexSymbol = @"\|\|", Description = "Logic OR", BinaryOperation = (b1, b2) => b1 || b2 },
            ];

        public static readonly VariableRegexFormula[] Variables = [
                new VariableRegexFormula
                {
                    Symbol = "True/False", RegexSymbol = @"(true|True|false|False)", Description = "Boolean Value",
                    GetComputable = (rawMatch) => {
                        var boolGroupName = "bool";

                        var boolCheck = new Regex($@"^(?<{boolGroupName}>true|True|false|False)$");
                        var boolMatch = boolCheck.Match(rawMatch);

                        if (boolMatch.Success)
                        {
                            var boolStr = boolMatch.Groups[boolGroupName].Value;
                            var boolVal = Boolean.Parse(boolStr);

                            var compute = new ConstantComputable
                            {
                                Value = boolVal,
                            };

                            return compute;
                        }

                        return null;
                    },
                    GetBuffer = (rawMatch) => {
                        return null;
                    }

                },
                new VariableRegexFormula
                {
                    Symbol = "###.###", RegexSymbol = @"(\d+(\.\d+)?)", Description = "Numeric Value",
                    GetComputable = (rawMatch) => {
                        var numericGroupName = "numeric";

                        var numberCheck = new Regex($@"^(?<{numericGroupName}>\d+(\.\d+)?)$");
                        var numberMatch = numberCheck.Match(rawMatch);

                        if (numberMatch.Success)
                        {
                            var numberStr = numberMatch.Groups[numericGroupName].Value;
                            var number = double.Parse(numberStr);

                            var compute = new ConstantComputable
                            {
                                Value = number,
                            };

                            return compute;
                        }

                        return null;
                    },
                    GetBuffer = (rawMatch) => {
                        return null;
                    }

                },
                new VariableRegexFormula
                {
                    Symbol = "a[-1]", RegexSymbol = @"([a-z]((\[(-\d+)\])|(\[(0)\])))", Description = "Sequence Value",
                    GetComputable = (rawMatch) => {
                        var variableGroupName = "variable";
                        var numberGroupName = "number";

                        // sequence
                        var variableCheck = new Regex(@$"^(?<{variableGroupName}>[a-z]((\[(?<{numberGroupName}>-\d+)\])|(\[(?<{numberGroupName}>0)\])))$");
                        var variableMatch = variableCheck.Match(rawMatch);

                        if (variableMatch.Success)
                        {
                            var variableLetter = variableMatch.Groups[variableGroupName].Value[0];
                            var variableNumberStr = variableMatch.Groups[numberGroupName].Value;

                            // letter
                            var letterByte = Convert.ToByte(variableLetter);
                            var aByte = Convert.ToByte('a');

                            var letterDiff = letterByte - aByte;

                            // number
                            var numberIndex = int.Parse(variableNumberStr);

                            var compute = new VariableComputable
                            {
                                SelectorFunction = coll => coll.GetValue(letterDiff, -numberIndex).Value,
                            };

                            return compute;
                        }

                        return null;
                    },
                    GetBuffer = (rawMatch) => {
                        var variableGroupName = "variable";
                        var numberGroupName = "number";

                        // sequence
                        var variableCheck = new Regex(@$"^(?<{variableGroupName}>[a-z]((\[(?<{numberGroupName}>-\d+)\])|(\[(?<{numberGroupName}>0)\])))$");
                        var variableMatch = variableCheck.Match(rawMatch);

                        if (variableMatch.Success)
                        {
                            var variableLetter = variableMatch.Groups[variableGroupName].Value[0];
                            var variableNumberStr = variableMatch.Groups[numberGroupName].Value;

                            // letter
                            var letterByte = Convert.ToByte(variableLetter);
                            var aByte = Convert.ToByte('a');

                            var letterDiff = letterByte - aByte;

                            // number
                            var numberIndex = int.Parse(variableNumberStr);

                            var compute = new VariableComputable
                            {
                                SelectorFunction = coll => coll.GetValue(letterDiff, -numberIndex).Value,
                            };

                            return new Tuple<int, int>(letterDiff, -numberIndex);
                        }

                        return null;
                    }
                },

            ];

        public static readonly VariableRegexFormula[] VariablesFeedback = [
                new VariableRegexFormula
                {
                    Symbol = "feedback", RegexSymbol = @"(feedback)", Description = "Feedback Value",
                    GetComputable = (rawMatch) => {
                        var feedbackGroupName = "fedback";

                        var feedCheck = new Regex($@"(?<{feedbackGroupName}>feedback)");
                        var feedMatch = feedCheck.Match(rawMatch);

                        if (feedMatch.Success)
                        {
                            var compute = new FeedbackComputable();

                            return compute;
                        }

                        return null;
                    },
                    GetBuffer = (rawMatch) => null,
                },
            ];

        public static readonly ContainerRegexFormula[] Containers = [
                new ContainerRegexFormula
                {
                    Symbol = "( )", RegexSymbol = @"(\(.*\))", Description = "Parenthesis",
                    GetSubstance = (rawMatch) => {
                        var groupName = "a";
                        var inDepth = $@"^\((?<{groupName}>.+)\)$";

                        var parenMatch = new Regex(inDepth).Match(rawMatch);

                        if (parenMatch.Success)
                        {
                            var substance = parenMatch.Groups[groupName].Value;

                            return substance;
                        }

                        return null;
                    }
                },
            ];
    }

    public class RegexFormula
    {
        public string Symbol;
        public string RegexSymbol;
        public string Description;
    }

    public class BinaryRegexFormula : RegexFormula
    {
        public Func<dynamic, dynamic, dynamic> BinaryOperation;
    }

    public class UnaryRegexFormula : RegexFormula
    {
        public Func<dynamic, dynamic> UnaryOperation;
    }

    public class VariableRegexFormula : RegexFormula
    {
        public Func<string, IComputable?> GetComputable;
        public Func<string, Tuple<int, int>?> GetBuffer;
    }

    public class ContainerRegexFormula : RegexFormula
    {
        public Func<string, string?> GetSubstance;
    }
}
