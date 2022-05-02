using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Calculator.Enums;
using Calculator.Exceptions;

namespace Calculator.Code
{
    public static class Solver
    {
        private static readonly Regex Splitter =
            new(@"(\d+([,.]\d+)?|\+|\-|\*|\/|\(|\)|\^)", RegexOptions.Compiled | RegexOptions.Singleline);

        public static (double result, long time) Solve(string expression)
        {
            var values = new Stack<(string val, Priority priority, double parsed)>();
            var ops = new Stack<(string oper, Priority priority)>();

            var sw = new Stopwatch();
            sw.Start();

            var prevPrior = Priority.Unknown;
            var items = Splitter.Matches(expression);

            foreach (Match item in items)
            {
                var value = item.Value.Replace('.', ',');
                var priority = GetPriority(value);
                var isNumber = priority == Priority.IsNumber;
                var parsed = 0d;
                if (isNumber)
                    parsed = double.Parse(value);

                if (priority > Priority.Parenthesis && prevPrior > Priority.Parenthesis &&
                    (!isNumber && prevPrior != Priority.IsNumber || isNumber && prevPrior == Priority.IsNumber))
                    throw new CalculationException(priority == Priority.IsNumber
                        ? "two numbers without operator between them"
                        : "two operators without number between them");
                prevPrior = priority;

                if (isNumber)
                    values.Push((value, priority, parsed));
                else if (value == "(")
                    ops.Push((value, priority));
                else if (priority > Priority.Parenthesis) //operator
                {
                    if (ops.Count > 0 && ops.Peek().priority >= priority)
                    {
                        var prevOp = ops.Pop();
                        PopOperationToValues(prevOp.oper, values);
                        ops.Push((value, priority));
                    }
                    else
                        ops.Push((value, priority));
                }
                else if (value == ")")
                {
                    if (ops.Count == 0)
                        throw new CalculationException("extra closing parenthesis");
                    while (ops.Peek().oper != "(")
                    {
                        var prevOp = ops.Pop();
                        PopOperationToValues(prevOp.oper, values);
                    }

                    ops.Pop();
                }
            }

            var result = Caculate(ops, values);

            sw.Stop();

            return (result, sw.ElapsedMilliseconds);
        }

        private static Priority GetPriority(string item) =>
            item switch
            {
                "(" or ")" => Priority.Parenthesis,
                "+" or "-" => Priority.AddSub,
                "*" or "/" => Priority.MultDiv,
                "^" => Priority.Pow,
                _ => Priority.IsNumber,
            };

        private static void PopOperationToValues(string oper,
            Stack<(string value, Priority priority, double parsed)> values)
        {
            if (oper == "(" && values.Count < 2)
                throw new CalculationException("absent of closing parenthesis");

            var value2 = values.Pop().parsed;
            var value1 = values.Pop().parsed;
            double result = 0;
            switch (oper)
            {
                case "+":
                    result = value1 + value2;
                    break;
                case "-":
                    result = value1 - value2;
                    break;
                case "*":
                    result = value1 * value2;
                    break;
                case "/":
                    if (Math.Abs(value2) < 0.0000000000000001)
                        throw new CalculationException("divided by zero");
                    result = value1 / value2;
                    break;
                case "^":
                    result = Math.Pow(value1, value2);
                    break;
            }

            values.Push(("", Priority.IsNumber, result));
        }

        private static double Caculate(Stack<(string oper, Priority priority)> ops, Stack<(string val, Priority priority, double parsed)> values)
        {
            while (ops.Count > 0)
            {
                var (operation, _) = ops.Pop();
                PopOperationToValues(operation, values);
            }

            var result = values.Pop().parsed;
            if (values.Count > 0)
                throw new CalculationException("error in expression (numbers)");
            return result;
        }
    }
}