using System;
using System.Diagnostics;

namespace Snippets.Terminals
{
  /* Attempting to keep this code as simple as possible, this class exposes only 2 methods. 
   *  - [1: RunCommand] Synchronously launches a Command Prompt (CMD.exe) instance and executes the command passed-through as an argument. Returns the redirected StandardOutput.
   *  - [2: RunCommandAsync] Asynchronously launches a Command Prompt (CMD.exe) instance and executes the command passed-through as an argument. Awaits then returns the redirected StandardOutput.
   * Default params were purposely kept basic, feel free to expand as needed.
   * - [0: userScript] The command or script to execute.
   */
  
  public class CMD
  {
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
}
