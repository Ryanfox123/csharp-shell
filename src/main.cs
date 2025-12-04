
using codecrafters_shell.Commands;
using codecrafters_shell.Helpers;
using System.Diagnostics;

class Program
{
    static void Main()
    {
    
    var pathEnv = System.Environment.GetEnvironmentVariable("PATH");
    string[] paths = pathEnv.Split(Path.PathSeparator);
    var homeEnv = System.Environment.GetEnvironmentVariable("HOME");

    var cmds = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "echo",
        "type",
        "exit",
        "pwd",
        "cd"
    };
     while (true) 
        {
        Console.Write("$ ");
        String? input = Console.ReadLine()?.Trim();
        var (cmd, args) = new ParsedCommands(input);


        if (string.IsNullOrEmpty(input) || input.ToLower() == "exit")
            {
                break;
            }
        else if (cmd.ToLower() == "cd")
            {
                if (args == "~")
                {
                    Directory.SetCurrentDirectory(homeEnv);
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
        else if (cmd.ToLower() == "pwd")
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
            }
        else if (cmd.ToLower() == "echo")
            {
                Console.WriteLine($"{args}");
            }
        else if (cmd.ToLower() == "type")
            {
                if (cmds.Contains(args.ToLower()))
                {
                    Console.WriteLine($"{args} is a shell builtin");
                }
                else
                {
                    var isFound = false;

                    foreach (var dir in paths)
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
        else
            {
                var isFound = false;
                foreach (var dir in paths)
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
}
