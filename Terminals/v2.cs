using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Snippets
{
    public class Terminals
    {
        public enum Shells
        {
            CMD = 0,
            WindowsPowerShell = 1,
            PowerShell7 = 2
        }

        internal class Config
        {
            internal class Paths
            {
                public static string ProgramFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                public static string SystemRootPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
                public static string WindowsDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

                public static string CMD = System.IO.Path.Combine(SystemRootPath, @"\cmd.exe");
                public static string WindowsPowerShell = System.IO.Path.Combine(SystemRootPath, @"\WindowsPowerShell\v1.0\powershell.exe");
                public static string PowerShell7 = System.IO.Path.Combine(ProgramFilesPath, @"\PowerShell\7\pwsh.exe");

                public static Dictionary<Shells, string> ShellPaths = new Dictionary<Shells, string>()
                {
                    { Shells.CMD, Paths.CMD },
                    { Shells.WindowsPowerShell, Paths.WindowsPowerShell },
                    { Shells.PowerShell7, Paths.PowerShell7 },
                };

                public static string GetShellPath(Shells _shell)
                {
                    return ShellPaths[_shell];
                }
            }
        }

        private static Process ProcessBuilder(string _command, Shells _shell, bool _runAsAdmin)
        {
            try
            {
                string shellPath = Config.Paths.GetShellPath(_shell);

                Process _proc = new Process();
                _proc.StartInfo.UseShellExecute = false;
                _proc.StartInfo.RedirectStandardOutput = true;
                _proc.StartInfo.CreateNoWindow = true;
                if (_runAsAdmin) { _proc.StartInfo.Verb = "runas"; }
                _proc.StartInfo.FileName = shellPath;

                switch (_shell)
                {
                    case Shells.CMD:
                        _proc.StartInfo.Arguments = $"/c {_command.Trim()}";
                        break;
                    case Shells.WindowsPowerShell:
                        _proc.StartInfo.Arguments = "-NoProfile -ExecutionPolicy Bypass -NoLogo -Command \"" + _command.Trim() + "\"";
                        break;
                    case Shells.PowerShell7:
                        _proc.StartInfo.Arguments = "-NoProfile -ExecutionPolicy Bypass -NoLogo -Command \"" + _command.Trim() + "\"";
                        break;
                    default:
                        _proc.StartInfo.Arguments = "";
                        break;
                }

                return _proc;
            }

            catch (Exception ex)
            {
                string errorMsg = $"Error: {ex.Message}";
                return null;
            }
        }

        public static string RunCommand(string command, Shells shell, bool runAsAdmin = false)
        {
            try
            {
                var shellProc = ProcessBuilder(command, shell, runAsAdmin);

                shellProc.Start();
                string _output = shellProc.StandardOutput.ReadToEnd();
                shellProc.WaitForExit();

                return _output.ToString().Trim();
            }

            catch (Exception ex)
            {
                return "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + ex.ToString();
            }
        }

        public static async Task<string> RunCommandAsync(string command, Shells shell, bool runAsAdmin = false)
        {
            try
            {
                var shellProc = ProcessBuilder(command, shell, runAsAdmin);

                shellProc.Start();
                string _output = await shellProc.StandardOutput.ReadToEndAsync();
                await shellProc.WaitForExitAsync();

                return _output.ToString().Trim();
            }

            catch (Exception ex)
            {
                return "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + ex.ToString();
            }
        }
    }
}
