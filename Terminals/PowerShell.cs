using System;
using System.Diagnostics;

namespace Snippets.Terminals
{
  // Attempting to keep this code as simple as possible, this method simply runs a PowerShell instance as an asyncronous process then awaits and returns the redirected StandardOutput.
  // The default/standard/included arguments are very basic, only requesting the command you want to run in PowerShell, the Execution Policy, and the path to the PowerShell executable (useful if you want to target different versions.)  
  public class PowerShell
  {
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
  }
}
