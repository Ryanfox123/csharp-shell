using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace codecrafters_shell.Helpers
{
    public class ExecutableHelpers
    {
         public static bool IsExecutable(string path)
    {
        if (!File.Exists(path))
            return false;

        if (OperatingSystem.IsWindows())
        {
            return IsWindowsExecutable(path);
        }
        else
        {
            return IsUnixExecutable(path);
        }
    }

    private static bool IsWindowsExecutable(string path)
    {
        string ext = Path.GetExtension(path).ToLowerInvariant();
        return ext is ".exe" or ".bat" or ".cmd" or ".com" or ".ps1";
    }

    private static bool IsUnixExecutable(string path)
    {
        var mode = File.GetUnixFileMode(path);

        return mode.HasFlag(UnixFileMode.UserExecute) ||
               mode.HasFlag(UnixFileMode.GroupExecute) ||
               mode.HasFlag(UnixFileMode.OtherExecute);
    }
    }
}