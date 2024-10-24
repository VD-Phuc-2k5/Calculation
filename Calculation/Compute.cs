using System;
using System.Collections.Generic;

namespace Calculation
{
    public class ComputeH
    {
        public ComputeH() { }
        public static double Compute(char op, double b, double a = 1)
        {
            switch (op)
            {
                case '+': return a + b;
                case '-': return a - b;
                case '*': return a * b;
                case '/': return a / b;
                case 'S': return Math.Sqrt(b);
                case 'P': return Math.Pow(a, b);
                case 's': return Math.Sin(DegreeToRadian(b));
                case 'C': return Math.Cos(DegreeToRadian(b));
                case 'T': 
                    if (b != 90 && b != -90)                    
                        return Math.Tan(DegreeToRadian(b));
                    throw new InvalidOperationException($"Invalid operator: {op}.");
                case 'L':
                    if (a > 0 && b > 0 && a != 1)
                        return Math.Log(b, a);
                    throw new InvalidOperationException($"Invalid operator: {op}.");
                default: throw new InvalidOperationException($"Invalid operator: {op}.");
            }
        }

        private static double DegreeToRadian(double degree)
        {
            return degree * (Math.PI / 180);
        }

        private static int Precedence(char op)
        {
            switch (op)
            {
                case '(': return 0;
                case '+':
                case '-': return 1;
                case '*':
                case '/': return 2;
                case 's':
                case 'S':
                case 'C':
                case 'T':
                case 'L':
                case 'P': return 3;
                default: return 4;
            }
        }

        private static bool IsUnaryOperation(char c)
        {
            return c == 's' || c == 'S' || c == 'C' || c == 'T';
        }

        public static double CalculateExpression(string expression)
        {
            Stack<double> values = new Stack<double>();
            Stack<char> operations = new Stack<char>();
            expression = expression
                .Replace("x", "*")
                .Replace("÷", "/")
                .Replace("Sqrt", "S")
                .Replace("Sin", "s")
                .Replace("Pow", "P")
                .Replace("Cos", "C")
                .Replace("Tan", "T")
                .Replace("Log", "L");

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == ',')
                    continue;

                if (char.IsDigit(expression[i]) || expression[i] == '.')
                {
                    string number = string.Empty;
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        number += expression[i++];
                    }
                    values.Push(double.Parse(number));
                    i--;
                }
                else if (operations.Count == 0 || expression[i] == '(')
                {
                    operations.Push(expression[i]);
                }
                else
                {
                    while (operations.Count > 0 && Precedence(expression[i]) <= Precedence(operations.Peek()))
                    {
                        if (operations.Peek() == '(')
                            throw new InvalidOperationException("Mismatched parentheses.");

                        double value = IsUnaryOperation(operations.Peek())
                            ? Compute(operations.Pop(), values.Pop())
                            : Compute(operations.Pop(), values.Pop(), values.Pop());

                        values.Push(value);
                    }

                    if (expression[i] == ')')
                    {
                        while (operations.Count > 0 && operations.Peek() != '(')
                        {
                            double value = IsUnaryOperation(operations.Peek())
                                ? Compute(operations.Pop(), values.Pop())
                                : Compute(operations.Pop(), values.Pop(), values.Pop());

                            values.Push(value);
                        }

                        if (operations.Count == 0)
                            throw new InvalidOperationException("Mismatched parentheses.");

                        operations.Pop(); 
                    }
                    else
                    {
                        operations.Push(expression[i]);
                    }
                }
            }

            while (operations.Count > 0)
            {
                if (operations.Peek() == '(')
                    throw new InvalidOperationException("Mismatched parentheses.");

                double value = IsUnaryOperation(operations.Peek())
                    ? Compute(operations.Pop(), values.Pop())
                    : Compute(operations.Pop(), values.Pop(), values.Pop());

                values.Push(value);
            }

            if (values.Count != 1)
                throw new InvalidOperationException("Invalid expression. Please check your input.");

            return values.Pop();
        }
    }
}
