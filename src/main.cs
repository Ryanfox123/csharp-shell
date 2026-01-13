using codecrafters_shell.src;

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
     var shell = new ShellRunner(paths, cmds, homeEnv);
     shell.run();
    }
}
