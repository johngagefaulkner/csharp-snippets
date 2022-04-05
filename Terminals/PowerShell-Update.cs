using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using static Snippets.Terminals;

namespace Snippets
{
    public class Terminals
    {
        public enum ExecutionPolicy
        {
            AllSigned = 0,
            Bypass,
            Default,
            RemoteSigned,
            Restricted,
            Undefined,
            Unrestricted
        }

        public enum PSVersion
        {
            WindowsPowerShell = 0,
            PowerShell7 = 1
        }

        public class Config
        {
            public class CustomProfiles
            {
                public class CMDProfile
                {
                    public string? ExecutablePath { get; set; }
                    public bool RunAsAdministrator { get; set; }

                    public static CMDProfile Create(string exePath, bool runAsAdmin)
                    {
                        CMDProfile cmdProfile = new CMDProfile();
                        cmdProfile.ExecutablePath = exePath;
                        cmdProfile.RunAsAdministrator = runAsAdmin;
                        return cmdProfile;
                    }
                }

                public class PowerShellProfile
                {
                    public string? ExecutablePath { get; set; }
                    public bool RunAsAdministrator { get; set; }
                    public ExecutionPolicy ExecutionPolicy { get; set; }
                    public PSVersion PSVersion { get; set; }

                    public static PowerShellProfile Create(string exePath, bool runAsAdmin, ExecutionPolicy exePolicy, PSVersion _version)
                    {
                        PowerShellProfile _profile = new();
                        _profile.ExecutablePath = exePath;
                        _profile.RunAsAdministrator = runAsAdmin;
                        _profile.ExecutionPolicy = exePolicy;
                        _profile.PSVersion = _version;
                        return _profile;
                    }
                }
            }
        }

        internal class Tools
        {
            internal static string? GetExecutionPolicyFromEnum(Terminals.ExecutionPolicy _enum)
            {
                return Enum.GetName(_enum);
            }

            internal static Process ProcessBuilder(string _filePath, string _args, bool _runAsAdmin)
            {
                Process _process = new();
                _process.StartInfo.UseShellExecute = false;
                _process.StartInfo.RedirectStandardOutput = true;
                _process.StartInfo.CreateNoWindow = true;
                _process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                if (_runAsAdmin) { _process.StartInfo.Verb = "runas"; }
                _process.StartInfo.FileName = _filePath;
                _process.StartInfo.Arguments = _args;
                return _process;
            }

            internal static string PSCommandBuilder(Terminals.ExecutionPolicy _executionPolicy, string _targetCommand)
            {
                string _policy = _executionPolicy.ToString().Trim();
                string _command = $"-ExecutionPolicy {_policy} -NoProfile -NonInteractive -Command \"{_targetCommand}\"";
                Console.WriteLine("[COMMAND] " + _command);
                return _command;
            }

            internal static string CMDCommandBuilder(string _targetCommand)
            {
                string _command = $"/c {_targetCommand}";
                return _command;
            }
        }

        internal class StringEditor
        {
            public enum ScriptChars
            {
                DoubleQuote = 0,
                SingleQuote = 1,
                Backtick = 2
            }

            internal static string GetScriptChar(ScriptChars _scriptChar)
            {
                if (_scriptChar == ScriptChars.DoubleQuote)
                {
                    return "\"";
                }
                else if (_scriptChar == ScriptChars.SingleQuote)
                {
                    return "'";
                }
                else if (_scriptChar == ScriptChars.Backtick)
                {
                    return "`";
                }
                else
                {
                    throw new Exception("Invalid script char");
                }
            }

            public static string WrapScript(string inputScript, ScriptChars scriptChar)
            {
                string scriptCharString = GetScriptChar(scriptChar);
                return $"{scriptCharString}{inputScript}{scriptCharString}";
            }

            public static string RemoveCharFromScript(string inputScript, ScriptChars scriptChar)
            {
                string scriptCharString = GetScriptChar(scriptChar);
                return inputScript.Replace(scriptCharString, "").Trim();
            }

            public static string RemoveAllCharsFromScript(string inputScript)
            {
                string doubleQuotesRemoved = RemoveCharFromScript(inputScript, ScriptChars.DoubleQuote);
                string singleQuotesRemoved = RemoveCharFromScript(doubleQuotesRemoved, ScriptChars.SingleQuote);
                string backtickRemoved = RemoveCharFromScript(singleQuotesRemoved, ScriptChars.Backtick);
                return backtickRemoved.Trim();
            }
        }

        public class CMD
        {
            private static string ExePath = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\cmd.exe";

            /// <summary>
            /// Launches a Command Prompt (CMD) instance and synchronously executes the specified command.
            /// </summary>
            /// <param name="cmdCommand">The command to execute in the Command Prompt (CMD) instance.</param>
            /// <param name="runAsAdmin">Defines whether to use the verb "runas" when launching the process. Default: false.</param>
            /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
            public static string RunCommand(string cmdCommand, bool runAsAdmin = false)
            {
                try
                {
                    string _command = Tools.CMDCommandBuilder(cmdCommand);
                    Process _cmd = Tools.ProcessBuilder(ExePath, _command, runAsAdmin);
                    _cmd.Start();

                    string _output = _cmd.StandardOutput.ReadToEnd();
                    _cmd.WaitForExit();

                    return _output.ToString().Trim();
                }

                catch (Exception ex)
                {
                    return "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Exception: " + ex.ToString();
                }
            }

            /// <summary>
            /// Launches a Command Prompt (CMD) instance and asynchronously executes the specified command.
            /// </summary>
            /// <param name="cmdCommand">The command to execute in the Command Prompt (CMD) instance.</param>
            /// <param name="runAsAdmin">Defines whether to use the verb "runas" when launching the process. Default: false.</param>
            /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
            public static async Task<string> RunCommandAsync(string cmdCommand, bool runAsAdmin = false)
            {
                try
                {
                    string _command = Tools.CMDCommandBuilder(cmdCommand);
                    Process _cmd = Tools.ProcessBuilder(ExePath, _command, runAsAdmin);
                    _cmd.Start();

                    string _output = await _cmd.StandardOutput.ReadToEndAsync();
                    await _cmd.WaitForExitAsync();

                    return _output.ToString().Trim();
                }

                catch (Exception ex)
                {
                    return "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Exception: " + ex.ToString();
                }
            }
        }

        public class PowerShell
        {
            private static Dictionary<PSVersion, string> ExePaths = new()
            {
                { PSVersion.WindowsPowerShell, Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\WindowsPowerShell\v1.0\powershell.exe" },
                { PSVersion.PowerShell7, Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\PowerShell\7\pwsh.exe" }
            };


            /// <summary>
            /// Launches a PowerShell instance and synchronously executes the specified command.
            /// </summary>
            /// <param name="psCommand">The command to execute in the PowerShell instance.</param>
            /// <param name="psRunAsAdmin">Defines whether to use the verb "runas" when launching the process. Default: false.</param>
            /// <param name="psPolicy">The ExecutionPolicy assigned when launching the PowerShell instance. Default: Uses Windows default settings.</param>
            /// <param name="psVersion">Determines whether the command should be executed using Windows PowerShell (5.1) or PowerShell 7. Default: Windows PowerShell.</param>
            /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
            public static string ExecuteCommand(string psCommand, bool psRunAsAdmin = false, ExecutionPolicy psPolicy = ExecutionPolicy.Default, PSVersion psVersion = PSVersion.WindowsPowerShell)
            {
                try
                {
                    string _command = Tools.PSCommandBuilder(psPolicy, psCommand);
                    Process _powerShell = Tools.ProcessBuilder(ExePaths[psVersion], _command, psRunAsAdmin);
                    _powerShell.Start();

                    string _output = _powerShell.StandardOutput.ReadToEnd();
                    _powerShell.WaitForExit();

                    return _output.ToString().Trim();
                }

                catch (Exception ex)
                {
                    return "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Exception: " + ex.ToString();
                }
            }

            /// <summary>
            /// Launches a PowerShell instance using a Custom Profile and synchronously executes the specified command.
            /// </summary>
            /// <param name="psCommand">The command to execute in the PowerShell instance.</param>
            /// <param name="psProfile">The Custom Profile object containing predefined values for the required parameters/arguments.</param>
            /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
            public static string ExecuteCommand(string psCommand, Config.CustomProfiles.PowerShellProfile psProfile)
            {
                try
                {
                    string _command = Tools.PSCommandBuilder(psProfile.ExecutionPolicy, psCommand);
                    Process _powershell = Tools.ProcessBuilder(ExePaths[psProfile.PSVersion], _command, psProfile.RunAsAdministrator);
                    _powershell.Start();

                    string _output = _powershell.StandardOutput.ReadToEnd();
                    _powershell.WaitForExit();

                    return _output.ToString().Trim();
                }

                catch (Exception ex)
                {
                    return "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Exception: " + ex.ToString();
                }
            }


            /// <summary>
            /// Launches a PowerShell instance and asynchronously executes the specified command.
            /// </summary>
            /// <param name="psCommand">The command to execute in the PowerShell instance.</param>
            /// <param name="psRunAsAdmin">Defines whether to use the verb "runas" when launching the process. Default: false.</param>
            /// <param name="psPolicy">The ExecutionPolicy assigned when launching the PowerShell instance. Default: Uses Windows default settings.</param>
            /// <param name="psVersion">Determines whether the command should be executed using Windows PowerShell (5.1) or PowerShell 7. Default: Windows PowerShell.</param>
            /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
            public static async Task<string> ExecuteCommandAsync(string psCommand, bool psRunAsAdmin = false, ExecutionPolicy psPolicy = ExecutionPolicy.Default, PSVersion psVersion = PSVersion.WindowsPowerShell)
            {
                try
                {
                    string _command = Tools.PSCommandBuilder(psPolicy, psCommand);
                    Process _powerShell = Tools.ProcessBuilder(ExePaths[psVersion], _command, psRunAsAdmin);
                    _powerShell.Start();

                    string _output = await _powerShell.StandardOutput.ReadToEndAsync();
                    await _powerShell.WaitForExitAsync();

                    return _output.ToString().Trim();
                }

                catch (Exception ex)
                {
                    return "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Exception: " + ex.ToString();
                }
            }

            /// <summary>
            /// Launches a PowerShell instance using a Custom Profile and asynchronously executes the specified command.
            /// </summary>
            /// <param name="psCommand">The command to execute in the PowerShell instance.</param>
            /// <param name="psProfile">The Custom Profile object containing predefined values for the required parameters/arguments.</param>
            /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
            public static async Task<string> ExecuteCommandAsync(string psCommand, Config.CustomProfiles.PowerShellProfile psProfile)
            {
                try
                {
                    string _command = Tools.PSCommandBuilder(psProfile.ExecutionPolicy, psCommand);
                    Process _powerShell = Tools.ProcessBuilder(ExePaths[psProfile.PSVersion], _command, psProfile.RunAsAdministrator);
                    _powerShell.Start();

                    string _output = await _powerShell.StandardOutput.ReadToEndAsync();
                    await _powerShell.WaitForExitAsync();

                    return _output.ToString().Trim();
                }

                catch (Exception ex)
                {
                    return "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Exception: " + ex.ToString();
                }
            }

            /// <summary>
            /// Launches a PowerShell instance and synchronously executes the script located at the provided file path.
            /// </summary>
            /// <param name="filePath">Full path to the .ps1 script.</param>
            /// <param name="psRunAsAdmin">Defines whether to use the verb "runas" when launching the process. Default: false.</param>
            /// <param name="psPolicy">The ExecutionPolicy assigned when launching the PowerShell instance. Default: Uses Windows default settings.</param>
            /// <param name="psVersion">Determines whether the command should be executed using Windows PowerShell (5.1) or PowerShell 7. Default: Windows PowerShell.</param>
            /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
            public static string ExecutePS1(string filePath, bool psRunAsAdmin = false, ExecutionPolicy psPolicy = ExecutionPolicy.Default, PSVersion psVersion = PSVersion.WindowsPowerShell)
            {
                try
                {
                    string cleansedPath = StringEditor.RemoveAllCharsFromScript(filePath);
                    string newPath = StringEditor.WrapScript(cleansedPath, StringEditor.ScriptChars.SingleQuote);
                    string _command = Tools.PSCommandBuilder(psPolicy, $"Invoke-Command -FilePath {newPath}");

                    Process _powerShell = Tools.ProcessBuilder(ExePaths[psVersion], _command, psRunAsAdmin);
                    _powerShell.Start();

                    string _output = _powerShell.StandardOutput.ReadToEnd();
                    _powerShell.WaitForExit();

                    return _output.ToString().Trim();
                }

                catch (Exception ex)
                {
                    return "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Exception: " + ex.ToString();
                }
            }

            /// <summary>
            /// Launches a PowerShell instance and asynchronously executes the script located at the provided file path.
            /// </summary>
            /// <param name="filePath">Full path to the .ps1 script.</param>
            /// <param name="psRunAsAdmin">Defines whether to use the verb "runas" when launching the process. Default: false.</param>
            /// <param name="psPolicy">The ExecutionPolicy assigned when launching the PowerShell instance. Default: Uses Windows default settings.</param>
            /// <param name="psVersion">Determines whether the command should be executed using Windows PowerShell (5.1) or PowerShell 7. Default: Windows PowerShell.</param>
            /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
            public static async Task<string> ExecutePS1Async(string filePath, bool psRunAsAdmin = false, ExecutionPolicy psPolicy = ExecutionPolicy.Default, PSVersion psVersion = PSVersion.WindowsPowerShell)
            {
                try
                {
                    string cleansedPath = StringEditor.RemoveAllCharsFromScript(filePath);
                    string newPath = StringEditor.WrapScript(cleansedPath, StringEditor.ScriptChars.SingleQuote);
                    string _command = Tools.PSCommandBuilder(psPolicy, $"Invoke-Command -FilePath {newPath}");

                    Process _powerShell = Tools.ProcessBuilder(ExePaths[psVersion], _command, psRunAsAdmin);
                    _powerShell.Start();

                    string _output = await _powerShell.StandardOutput.ReadToEndAsync();
                    await _powerShell.WaitForExitAsync();

                    return _output.ToString().Trim();
                }

                catch (Exception ex)
                {
                    return "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Exception: " + ex.ToString();
                }
            }
        }
    }
}
