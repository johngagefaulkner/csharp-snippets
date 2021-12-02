using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace Snippets.Terminals
{
	public class StringEditor
	{
		public static string AddQuotes(string inputStr, bool addSingleQuotesInstead = false)
		{
			if (addSingleQuotesInstead)
			{
				return "\"" + inputStr.Trim() + "\"";
			}

			else
			{
				return "'" + inputStr.Trim() + "'";
			}
		}

		public static string RemoveQuotes(string inputStr, bool removeSingleAndDoubleQuotes = false)
		{
			if (removeSingleAndDoubleQuotes)
			{
				string inputWithoutDoubleQuotes = inputStr.Replace("\"", "");
				return inputWithoutDoubleQuotes.Replace("'", "").Trim();
			}

			else
			{
				return inputStr.Replace("\"", "").Trim();
			}
		}
	}

	public class ExeFinder
	{
		public static Dictionary<PowerShell.PowerShellVersion, string> GetPowerShellPaths()
		{
			try
			{
				Dictionary<PowerShell.PowerShellVersion, string> _result = new();

				//%SystemRoot%\system32\WindowsPowerShell\v1.0\
				string systemRoot = Environment.GetFolderPath(Environment.SpecialFolder.System);
				string winPsExe = System.IO.Path.Combine(systemRoot, "WindowsPowerShell", "v1.0", "powershell.exe");
				if (File.Exists(winPsExe))
				{
					_result.Add(PowerShell.PowerShellVersion.WindowsPowerShell, Path.GetFullPath(winPsExe));
				}

				// "C:\Program Files\PowerShell\7\pwsh.exe"
				// "C:\Program Files\PowerShell\7\pwsh.exe" -WorkingDirectory ~
				string programFilesDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
				string ps7Exe = Path.Combine(programFilesDir, "PowerShell", "7", "pwsh.exe");
				if (File.Exists(ps7Exe))
				{
					_result.Add(PowerShell.PowerShellVersion.PowerShell7, Path.GetFullPath(ps7Exe));
				}

				// Determine whether dictionary was populated
				if (_result.Count == 0)
				{
					return null;
				}

				return _result;
			}

			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				return null;
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

		public enum PowerShellVersion
		{
			WindowsPowerShell = 0,
			PowerShell7
		}

		private static bool IsPowerShellFound()
		{
			try
			{
				if (PowerShellExePaths == null)
				{
					var myPaths = ExeFinder.GetPowerShellPaths();

					if (myPaths == null)
					{
						return false;
					}

					else
					{
						PowerShellExePaths = myPaths;
						return true;
					}
				}

				else
				{
					return true;
				}
			}

			catch (Exception ex)
			{
				return false;
			}
		}

		public static Dictionary<PowerShellVersion, string> PowerShellExePaths { get; set; } = null;

		/// <summary>
		/// Launches a PowerShell instance and executes the specified command or script.
		/// </summary>
		/// <param name="psCommand">The command or script to execute in the PowerShell instance.</param>
		/// <param name="psPolicy">The ExecutionPolicy assigned when launching the PowerShell instance. Default: Bypass.</param>
		/// <param name="psPath">The full path to the PowerShell executable. Useful if targeting specific versions. Default: powershell.exe</param>
		/// <returns>[string] Redirects and returns the StandardOutput into a single trimmed string (or, alternatively, the Exception as a string.)</returns>
		public static string ExecuteCommand(string psCommand, ExecutionPolicies psPolicy = ExecutionPolicies.Bypass, PowerShellVersion psVersion = PowerShellVersion.WindowsPowerShell)
		{
			try
			{
				if (IsPowerShellFound() == false)
				{
					return "PowerShell could not be found on this system. Please submit an issue on GitHub!";
				}

				string _policy = psPolicy.ToString().Trim();

				Process _powerShell = new Process();
				_powerShell.StartInfo.UseShellExecute = false;
				_powerShell.StartInfo.RedirectStandardOutput = true;
				_powerShell.StartInfo.CreateNoWindow = true;
				_powerShell.StartInfo.Verb = "runas";
				_powerShell.StartInfo.FileName = PowerShellExePaths[psVersion];
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
		public static async Task<string> ExecuteCommandAsync(string psCommand, ExecutionPolicies psPolicy = ExecutionPolicies.Bypass, PowerShellVersion psVersion = PowerShellVersion.WindowsPowerShell)
		{
			try
			{
				if (IsPowerShellFound() == false)
				{
					return "PowerShell could not be found on this system. Please submit an issue on GitHub!";
				}

				string _policy = psPolicy.ToString().Trim();

				Process _powerShell = new Process();
				_powerShell.StartInfo.UseShellExecute = false;
				_powerShell.StartInfo.RedirectStandardOutput = true;
				_powerShell.StartInfo.CreateNoWindow = true;
				_powerShell.StartInfo.Verb = "runas";
				_powerShell.StartInfo.FileName = PowerShellExePaths[psVersion];
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
		public static string ExecutePS1(string filePath, PowerShellVersion psVersion = PowerShellVersion.WindowsPowerShell)
		{
			try
			{
				if (IsPowerShellFound() == false)
				{
					return "PowerShell could not be found on this system. Please submit an issue on GitHub!";
				}

				string cleansedPath = StringEditor.RemoveQuotes(filePath, true);
				string psCommand = "Invoke-Command -FilePath " + StringEditor.AddQuotes(cleansedPath, true);

				Process _powerShell = new Process();
				_powerShell.StartInfo.UseShellExecute = false;
				_powerShell.StartInfo.RedirectStandardOutput = true;
				_powerShell.StartInfo.CreateNoWindow = true;
				_powerShell.StartInfo.Verb = "runas";
				_powerShell.StartInfo.FileName = PowerShellExePaths[psVersion];
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
		public static async Task<string> ExecutePS1Async(string filePath, PowerShellVersion psVersion = PowerShellVersion.WindowsPowerShell)
		{
			try
			{
				if (IsPowerShellFound() == false)
				{
					return "PowerShell could not be found on this system. Please submit an issue on GitHub!";
				}

				string cleansedPath = StringEditor.RemoveQuotes(filePath, true);
				string psCommand = "Invoke-Command -FilePath " + StringEditor.AddQuotes(cleansedPath, true);

				Process _powerShell = new Process();
				_powerShell.StartInfo.UseShellExecute = false;
				_powerShell.StartInfo.RedirectStandardOutput = true;
				_powerShell.StartInfo.CreateNoWindow = true;
				_powerShell.StartInfo.Verb = "runas";
				_powerShell.StartInfo.FileName = PowerShellExePaths[psVersion];
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
