using System;
using System.Collections.Generic;
using System.Text;

namespace codecrafters_shell.Commands
{
public class ParsedCommands
{
    public string Command { get; }
    public List<string> Arguments { get; }

    public ParsedCommands(string input)
    {
        var tokens = Parse(input);

        Command = tokens.Count > 0 ? tokens[0] : "";
        Arguments = tokens.Skip(1).ToList();
    }

private static List<string> Parse(string input)
{
    var tokens = new List<string>();
    var current = new StringBuilder();
    bool inSingleQuotes = false;

    for (int i = 0; i < input.Length; i++)
    {
        char c = input[i];

        if (c == '\'')
        {
            inSingleQuotes = !inSingleQuotes;
            continue;
        }

        if (char.IsWhiteSpace(c) && !inSingleQuotes)
        {
            if (current.Length > 0)
            {
                tokens.Add(current.ToString());
                current.Clear();
            }
        }
        else
        {
            current.Append(c);
        }
    }

    if (current.Length > 0)
        tokens.Add(current.ToString());

    return tokens;
}


        public void Deconstruct(out string cmd, out List<string> args)
        {
            cmd = Command;
            args = Arguments;
        }
    }
}
