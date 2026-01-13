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
                            case "pwd":
                                Console.WriteLine(Directory.GetCurrentDirectory());
                                break;
                            case "echo":
                                Console.WriteLine($"{args}");
                                break;
                            case "type":
                                typeCommand(args);
                                break;
                            default:
                                findAndRunExecutable(args, cmd, input);
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
        public void typeCommand(string args)
        {
            if (_cmds.Contains(args.ToLower()))
            {
                Console.WriteLine($"{args} is a shell builtin");
            }
            else
            {
                var isFound = false;

                foreach (var dir in _paths)
                {
                    var currSearch = Path.Join(dir, args);
                    if (File.Exists(currSearch) && ExecutableHelpers.IsExecutable(currSearch))
                    {
                        isFound = true;
                        Console.WriteLine($"{args} is {currSearch}");
                        break;
                    }
                }
                if (!isFound)
                {
                Console.WriteLine($"{args}: not found");
                }
            }
        }
        public void findAndRunExecutable(string args, string cmd, string input)
        {
                            var isFound = false;
                foreach (var dir in _paths)
                    {
                        var currSearch = Path.Join(dir, cmd);
                        if (File.Exists(currSearch) && ExecutableHelpers.IsExecutable(currSearch))
                        {
                            isFound = true;
                            ProcessStartInfo start = new ProcessStartInfo();
                            start.Arguments = args;
                            start.FileName = cmd;
                            start.RedirectStandardOutput = true;
                            start.RedirectStandardError = true; 
                            start.UseShellExecute = false;
                            
                            using (Process process = new Process())
                            {
                                process.StartInfo = start;
                                process.Start();

                                string output = process.StandardOutput.ReadToEnd();

                                process.WaitForExit();

                                Console.Write(output);
                            }
                        }
                    }
                if (!isFound) 
                {
                    Console.WriteLine($"{input}: command not found");
                }
        }
    }
}