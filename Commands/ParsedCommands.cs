using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace codecrafters_shell.Commands
{
    public class ParsedCommands
    {
        public string Command { get; set; }
        public string Argument { get; set; }

        public ParsedCommands(string input)
        {
            var parts = input.Trim().Split(' ', 2);
            Command = parts[0];
            Argument = parts.Length > 1 ? parts[1] : "";
        }
        public void Deconstruct(out string cmd, out string args)
        {
            cmd = Command;
            args = Argument;
        }
    }
}