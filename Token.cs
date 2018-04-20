using System;

namespace Compiler
{
    public class Token
    {
        public string Text { get; set; } = string.Empty;
        public Tokens Type { get; set; } = Tokens.Undefined;
        public int StartLocation { get => EndLocation - Text.Length + 1; }
        public int EndLocation { get; set; }

        public static Tokens GenerateType(string value)
        {
            switch (value)
            {
                case "+":
                    return Tokens.Plus;
                case "-":
                    return Tokens.Minus;
                case "*":
                    return Tokens.Multiply;
                case "/":
                    return Tokens.Divition;
                case "(":
                    return Tokens.LeftBracket;
                case ")":
                    return Tokens.RightBracket;
                case "#":
                    return Tokens.LexEnd;
                case "":
                    return Tokens.Epsilon;
            }
            throw new Exception("Unexpected Token: " + value);
        }

        public override string ToString()
        {
            var baseStr = string.Format("{0}[{1}] @ (", Type, Text);
            var locationStr = "{0},{1})";
            if (StartLocation == EndLocation)
            {
                locationStr = "{0})";
            }
            return baseStr + string.Format(locationStr, StartLocation, EndLocation);
        }
    }
}
