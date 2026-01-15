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
    bool inDoubleQuotes = false;

    bool isEscaped = false;

    for (int i = 0; i < input.Length; i++)
    {
        char c = input[i];

        if (c == '\'' && !inSingleQuotes && !inDoubleQuotes)
        {
            inSingleQuotes = true;
            continue;            
        }

        if (c == '"' && !inDoubleQuotes && !inSingleQuotes)
        {
            inDoubleQuotes = true;
            continue;            
        }

        if (inSingleQuotes)
        {
            if (c == '\'')
            {
                inSingleQuotes = false;
                continue;
            }
            current.Append(c);
            continue;
        }
        if (isEscaped)
        {
            current.Append(c);
            isEscaped = false;
            continue;
        }
        if (inDoubleQuotes)
        {
            if (c == '"')
            {
                inDoubleQuotes = false;
                continue;            
            }
            if (c == '\\' && i + 1 < input.Length)
            {
                char next = input[i + 1];
                if (next == '"' || next == '\\' || next == '$' || next == '`')
                {
                    i++;
                    current.Append(next);
                    continue;
                }
} 
        }
        if (!inSingleQuotes && !inDoubleQuotes && c == '\\')
        {
            i++;
            if (i < input.Length)
                current.Append(input[i]);
            continue;
        }
        else if (char.IsWhiteSpace(c) && !inSingleQuotes && !inDoubleQuotes)
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
