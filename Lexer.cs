using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Compiler
{
    public static class Lexer
    {
        private static bool isNumber(char ch) => new Regex(@"^\d+$").IsMatch(ch.ToString());

        private static bool isChar(char ch) => new Regex(@"^[A-Za-z$]+$").IsMatch(ch.ToString());

        private static bool isOperator(char ch)
        {
            var keys = new List<char> { '=', '+', '-', '*', '/', '%', '^', '(', ')', ';', '{', '}', '[', ']', '<', '>', '\'', '"' };
            return keys.Contains(ch);
        }

        private static bool isBlank(char ch) => ch.ToString().Trim() == string.Empty;

        public static List<Token> Tokenizer(string expression)
        {
            var to = new List<Token>();
            var ch = '\0';
            for (var i = 0; i < expression.Length;)
            {
                var ti = new Token();
                ch = expression[i];
                if (isBlank(ch))
                {
                    i++;
                    continue;
                }

                if (isNumber(ch) || ch == '.')
                {
                    ti.Text += ch;
                    i++;
                    if (i >= expression.Length)
                    {
                        ti.Type = Tokens.Number;
                        ti.EndLocation = i - 1;
                        to.Add(ti);
                        return to;
                    }
                    ch = expression[i];
                    while (isNumber(ch) || ch == '.')
                    {
                        ti.Text += ch;
                        i++;
                        if (i >= expression.Length)
                        {
                            ti.Type = Tokens.Number;
                            ti.EndLocation = i - 1;
                            to.Add(ti);
                            return to;
                        }
                        ch = expression[i];
                    }
                    ti.Type = Tokens.Number;
                    to.Add(ti);
                    ti.EndLocation = i - 1;
                }
                else if (isOperator(ch))
                {
                    ti.Text += ch;
                    ti.Type = Token.GenerateType(ch.ToString());
                    to.Add(ti);
                    i++;
                    ti.EndLocation = i - 1;
                }
                else
                {
                    throw new Exception("Undefined token: " + ch);
                }
            }
            return to;
        }
    }
}