
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
        string[] parts = input.Split(' ', 2);
        string command = parts[0];
        string argument = parts.Length > 1 ? parts[1] : "";


        if (string.IsNullOrEmpty(input) || input.ToLower() == "exit")
            {
                break;
            }
        else if (command.ToLower() == "cd")
            {
                if (argument == "~")
                {
                    Directory.SetCurrentDirectory(homeEnv);
                } else 
                {
                    try
                        {
                            Directory.SetCurrentDirectory(argument);
                        }
                        catch
                        {
                            Console.WriteLine($"cd: {argument}: No such file or directory");
                        }
                }
            }
        else if (command.ToLower() == "pwd")
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
            }
        else if (command.ToLower() == "echo")
            {
                Console.WriteLine($"{argument}");
            }
        else if (command.ToLower() == "type")
            {
                if (cmds.Contains(argument.ToLower()))
                {
                    Console.WriteLine($"{argument} is a shell builtin");
                }
                else
                {
                    var isFound = false;

                    foreach (var dir in paths)
                    {
                        var currSearch = Path.Join(dir, argument);
                        if (File.Exists(currSearch) && ExecutableHelpers.IsExecutable(currSearch))
                        {
                            isFound = true;
                            Console.WriteLine($"{argument} is {currSearch}");
                            break;
                        }
                    }
                    if (!isFound)
                    {
                    Console.WriteLine($"{argument}: not found");
                    }
                }
            } 
        else
            {
                var isFound = false;
                foreach (var dir in paths)
                    {
                        var currSearch = Path.Join(dir, command);
                        if (File.Exists(currSearch) && ExecutableHelpers.IsExecutable(currSearch))
                        {
                            isFound = true;
                            ProcessStartInfo start = new ProcessStartInfo();
                            start.Arguments = argument;
                            start.FileName = command;
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
