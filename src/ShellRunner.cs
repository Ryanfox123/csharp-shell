using System;
using System.Collections.Generic;
using codecrafters_shell.Commands;
using codecrafters_shell.Helpers;
using System.Diagnostics;

namespace codecrafters_shell.src
{
    public class ShellRunner
    {
        private readonly string[] _paths;
        private readonly HashSet<string> _cmds;
        private readonly string _homeEnv;
        public ShellRunner(string[] paths, HashSet<string> cmds, string homeEnv)
        {
            _paths = paths;
            _cmds = cmds;
            _homeEnv = homeEnv;
        }
        public void run()
        {
        while (true) 
            {
                Console.Write("$ ");
                String? input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input) || input.ToLower() == "exit")
                    {
                        break;
                    }
                
                var (cmd, args) = new ParsedCommands(input);
                cmd = cmd.ToLower();

                switch (cmd)
                    {
                        case "cd":
                        changeDir(args);
                        break;
                    }
            }
        }
        public void changeDir(string args)
        {
            if (args == "~")
                {
                    Directory.SetCurrentDirectory(_homeEnv);
                } else 
                {
                    try
                        {
                            Directory.SetCurrentDirectory(args);
                        }
                        catch
                        {
                            Console.WriteLine($"cd: {args}: No such file or directory");
                        }
                }
        }
    }
}