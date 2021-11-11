using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snippets.Terminals
{
    public class CMD
    {

        /* Attempting to keep this code as simple as possible, this class exposes only 2 methods. 
        *  - [1: RunCommand] Synchronously launches a Command Prompt (CMD.exe) instance and executes the command passed-through as an argument. Returns the redirected StandardOutput.
        *  - [2: RunCommandAsync] Asynchronously launches a Command Prompt (CMD.exe) instance and executes the command passed-through as an argument. Awaits then returns the redirected StandardOutput.
        * Default params were purposely kept basic, feel free to expand as needed.
        * - [0: userScript] The command or script to execute.
        */

        /// <summary>
        /// Synchronously launches a Command Prompt (CMD.exe) instance and executes the specified command or script.
        /// </summary>
        /// <param name="userScript">The command or script to execute in the CMD instance.</param>
        /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
        public static string RunCommand(string userScript)
        {
            try
            {
                var _cmd = new System.Diagnostics.Process();
                _cmd.StartInfo.UseShellExecute = false;
                _cmd.StartInfo.RedirectStandardOutput = true;
                _cmd.StartInfo.CreateNoWindow = true;
                _cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                _cmd.StartInfo.Verb = "runas";
                _cmd.StartInfo.FileName = "cmd.exe";
                _cmd.StartInfo.Arguments = "/c " + userScript.Trim();
                _cmd.Start();

                string _output = _cmd.StandardOutput.ReadToEnd();
                _cmd.WaitForExit();

                return _output.ToString().Trim();
            }

            catch (Exception ex)
            {
                return "[Error] " + ex.Message;
            }
        }

        /// <summary>
        /// Asynchronously launches a Command Prompt (CMD.exe) instance and executes the specified command or script.
        /// </summary>
        /// <param name="userScript">The command or script to execute in the CMD instance.</param>
        /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
        public static async Task<string> RunCommandAsync(string userScript)
        {
            try
            {
                var _cmd = new System.Diagnostics.Process();
                _cmd.StartInfo.UseShellExecute = false;
                _cmd.StartInfo.RedirectStandardOutput = true;
                _cmd.StartInfo.CreateNoWindow = true;
                _cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                _cmd.StartInfo.Verb = "runas";
                _cmd.StartInfo.FileName = "cmd.exe";
                _cmd.StartInfo.Arguments = "/c " + userScript.Trim();
                _cmd.Start();

                string _output = await _cmd.StandardOutput.ReadToEndAsync();
                await _cmd.WaitForExitAsync();

                return _output.ToString().Trim();
            }

            catch (Exception ex)
            {
                return "[Error] " + ex.Message;
            }
        }
    }

    public class PowerShell
    {
        /* Attempting to keep this code as simple as possible, this class exposes only 4 methods. 
        *  - [1: ExecuteCommand] Synchronously launches a  PowerShell (powershell.exe) instance and executes the command passed-through as an argument. Returns the redirected StandardOutput.
        *  - [2: ExecuteCommandAsync] Asynchronously launches a PowerShell (powershell.exe) instance and executes the command passed-through as an argument. Awaits then returns the redirected StandardOutput.
        * - Pending
        * - Pending
        * Default params were purposely kept basic, feel free to expand as needed.
        * - [0: psCommand] The command or script to execute in the PowerShell instance.
        * - [1: psPolicy] The ExecutionPolicy assigned when launching the PowerShell instance. Default: Bypass.
        * - [2: psPath] The full path to the PowerShell executable. Useful if targeting specific versions. Default: powershell.exe
        */

        public enum ExecutionPolicies
        {
            AllSigned = 0,
            Bypass,
            Default,
            RemoteSigned,
            Restricted,
            Undefined,
            Unrestricted
        }

        /// <summary>
        /// Launches a PowerShell instance and executes the specified command or script.
        /// </summary>
        /// <param name="psCommand">The command or script to execute in the PowerShell instance.</param>
        /// <param name="psPolicy">The ExecutionPolicy assigned when launching the PowerShell instance. Default: Bypass.</param>
        /// <param name="psPath">The full path to the PowerShell executable. Useful if targeting specific versions. Default: powershell.exe</param>
        /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
        public static string ExecuteCommand(string psCommand, ExecutionPolicies psPolicy = ExecutionPolicies.Bypass, string psPath = "powershell.exe")
        {
            try
            {
                string _policy = psPolicy.ToString().Trim();

                Process _powerShell = new Process();
                _powerShell.StartInfo.UseShellExecute = false;
                _powerShell.StartInfo.RedirectStandardOutput = true;
                _powerShell.StartInfo.CreateNoWindow = true;
                _powerShell.StartInfo.Verb = "runas";
                _powerShell.StartInfo.FileName = "powershell.exe";
                _powerShell.StartInfo.Arguments = $"-NoProfile -ExecutionPolicy {_policy} -NoLogo -Command {psCommand}";
                _powerShell.Start();

                string _output = _powerShell.StandardOutput.ReadToEnd();
                _powerShell.WaitForExit();

                return _output.ToString().Trim();
            }

            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        /// <summary>
        /// Asynchronously launches a PowerShell instance and executes the specified command or script.
        /// </summary>
        /// <param name="psCommand">The command or script to execute in the PowerShell instance.</param>
        /// <param name="psPolicy">The ExecutionPolicy assigned when launching the PowerShell instance. Default: Bypass.</param>
        /// <param name="psPath">The full path to the PowerShell executable. Useful if targeting specific versions. Default: powershell.exe</param>
        /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
        public static async Task<string> ExecuteCommandAsync(string psCommand, ExecutionPolicies psPolicy = ExecutionPolicies.Bypass, string psPath = "powershell.exe")
        {
            try
            {
                string _policy = psPolicy.ToString().Trim();

                Process _powerShell = new Process();
                _powerShell.StartInfo.UseShellExecute = false;
                _powerShell.StartInfo.RedirectStandardOutput = true;
                _powerShell.StartInfo.CreateNoWindow = true;
                _powerShell.StartInfo.Verb = "runas";
                _powerShell.StartInfo.FileName = psPath;
                _powerShell.StartInfo.Arguments = $"-NoProfile -ExecutionPolicy {_policy} -NoLogo -Command {psCommand}";
                _powerShell.Start();

                string _output = await _powerShell.StandardOutput.ReadToEndAsync();
                await _powerShell.WaitForExitAsync();

                return _output.ToString().Trim();
            }

            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        /// <summary>
        /// Synchronously launches a PowerShell instance and executes the script located at the provided file path.
        /// </summary>
        /// <param name="filePath">Full path to the .ps1 script.</param>
        /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
        public static string ExecutePS1(string filePath)
        {
            try
            {
                string pathWithoutDoubleQuotes = filePath.Replace('"', ' ').Trim();
                string pathWithoutSingleQuotes = pathWithoutDoubleQuotes.Replace("'", "").Trim();
                char quoteChar = '"';
                string quoteStr = quoteChar.ToString();
                string psCommand = "Invoke-Command -FilePath " + quoteStr + pathWithoutSingleQuotes + quoteStr;

                Process _powerShell = new Process();
                _powerShell.StartInfo.UseShellExecute = false;
                _powerShell.StartInfo.RedirectStandardOutput = true;
                _powerShell.StartInfo.CreateNoWindow = true;
                _powerShell.StartInfo.Verb = "runas";
                _powerShell.StartInfo.FileName = "powershell.exe";
                _powerShell.StartInfo.Arguments = $"-NoProfile -ExecutionPolicy Bypass -NoLogo -Command {psCommand}";
                _powerShell.Start();

                string _output = _powerShell.StandardOutput.ReadToEnd();
                _powerShell.WaitForExit();

                return _output.ToString().Trim();
            }

            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        /// <summary>
        /// Asynchronously launches a PowerShell instance and executes the script located at the provided file path.
        /// </summary>
        /// <param name="filePath">Full path to the .ps1 script.</param>
        /// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
        public static async Task<string> ExecutePS1Async(string filePath)
        {
            try
            {
                string pathWithoutDoubleQuotes = filePath.Replace('"', ' ').Trim();
                string pathWithoutSingleQuotes = pathWithoutDoubleQuotes.Replace("'", "").Trim();
                char quoteChar = '"';
                string quoteStr = quoteChar.ToString();
                string psCommand = "Invoke-Command -FilePath " + quoteStr + pathWithoutSingleQuotes + quoteStr;

                Process _powerShell = new Process();
                _powerShell.StartInfo.UseShellExecute = false;
                _powerShell.StartInfo.RedirectStandardOutput = true;
                _powerShell.StartInfo.CreateNoWindow = true;
                _powerShell.StartInfo.Verb = "runas";
                _powerShell.StartInfo.FileName = "powershell.exe";
                _powerShell.StartInfo.Arguments = $"-NoProfile -ExecutionPolicy Bypass -NoLogo -Command {psCommand}";
                _powerShell.Start();

                string _output = await _powerShell.StandardOutput.ReadToEndAsync();
                await _powerShell.WaitForExitAsync();

                return _output.ToString().Trim();
            }

            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
