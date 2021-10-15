using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text;

namespace Snippets.Services
{
    public class LogClient
    {
        #region ClientConfiguration
        private string PathToLogFile { get; set; } = null;
        private LogOutputType _logOutputType { get; set; } = LogOutputType.Default;

        public string LogFilePath
        {
            get { return PathToLogFile; }
            set { PathToLogFile = value.Trim(); }
        }

        public LogOutputType LogOutput
        {
            get { return _logOutputType; }
            set { _logOutputType = value; }
        }
        #endregion

        #region Converters-Helpers-Extensions-etc
        private static string GenerateLogFileName()
        {
            string _appName = Process.GetCurrentProcess().ProcessName;
            string _fullPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _appName + ".log");
            return _fullPath.Trim();
        }

        private static string GetTime()
        {
            return "[" + DateTime.Now.ToLongTimeString() + "] ";
        }

        private static string GetSeverity(Severity userSev)
        {
            return "[" + userSev.ToString() + "] ";
        }
        #endregion

        #region Enums
        public enum LogOutputType
        {
            Default = 0, // Console and File
            Console, // Only writes log entries to console
            ConsoleAndFile, // Writes log entries to both the console and the log file
            Debug, // Only writes log entries to the Debug trace listeners
            File, // Only writes log entries to the log file
            All, // Writes log entries to all of the above
            None // Disposes of all log entries.
        }

        private enum Severity
        {
            INFO = 0, // Information
            WARN, // Warning
            ERROR, // Error, Failure
            SUCCESS, // Success
            DEBUG, // Shouldn't be seen by users, only for debugging the application
            VERBOSE // Includes everything
        }
        #endregion

        #region Log-Updates
        private void UpdateLog(Severity logSev, string updateStr)
        {
            string _msg = GetTime() + GetSeverity(logSev) + updateStr.Trim();

            switch (_logOutputType)
            {
                case LogOutputType.Default:
                    Console.WriteLine(_msg);
                    AppendToLog(_msg);
                    break;

                case LogOutputType.Console:
                    Console.WriteLine(_msg);
                    break;

                case LogOutputType.ConsoleAndFile:
                    Console.WriteLine(_msg);
                    AppendToLog(_msg);
                    break;

                case LogOutputType.Debug:
                    Debug.WriteLine(_msg);
                    break;

                case LogOutputType.File:
                    AppendToLog(_msg);
                    break;

                case LogOutputType.All:
                    Debug.WriteLine(_msg);
                    Console.WriteLine(_msg);
                    AppendToLog(_msg);
                    break;

                case LogOutputType.None:
                    break;
            }
        }

        private async Task UpdateLogAsync(Severity logSev, string updateStr, bool updateConsole = true)
        {
            string _msg = GetTime() + GetSeverity(logSev) + updateStr.Trim();

            switch (_logOutputType)
            {
                case LogOutputType.Default:
                    Console.WriteLine(_msg);
                    await AppendToLogAsync(_msg);
                    break;

                case LogOutputType.Console:
                    Console.WriteLine(_msg);
                    break;

                case LogOutputType.ConsoleAndFile:
                    Console.WriteLine(_msg);
                    await AppendToLogAsync(_msg);
                    break;

                case LogOutputType.Debug:
                    Debug.WriteLine(_msg);
                    break;

                case LogOutputType.File:
                    await AppendToLogAsync(_msg);
                    break;

                case LogOutputType.All:
                    Debug.WriteLine(_msg);
                    Console.WriteLine(_msg);
                    await AppendToLogAsync(_msg);
                    break;

                case LogOutputType.None:
                    break;
            }
        }

        public void LogInfo(string logStr)
        {
            UpdateLog(Severity.INFO, logStr);
        }

        public async Task LogInfoAsync(string logStr)
        {
            await UpdateLogAsync(Severity.INFO, logStr);
        }

        public void LogWarning(string warningStr)
        {
            UpdateLog(Severity.WARN, warningStr);
        }

        public async Task LogWarningAsync(string warningStr)
        {
            await UpdateLogAsync(Severity.INFO, warningStr);
        }

        public void LogError(string errorStr, Exception logEx = null)
        {
            string errorMsg = "";

            if (string.IsNullOrEmpty(errorStr) && logEx == null)
            {
                errorMsg = "Error Message and Exception were both null. (?)";
            }

            if (string.IsNullOrEmpty(errorStr) && logEx != null)
            {
                errorMsg = "Message: " + logEx.Message.Trim() + Environment.NewLine + Environment.NewLine +
                    "Error Details: " + logEx.ToString().Trim();
            }

            if (!string.IsNullOrEmpty(errorStr) && logEx == null)
            {
                errorMsg = errorStr.Trim();
            }

            if (!string.IsNullOrEmpty(errorStr) && logEx != null)
            {
                errorMsg = errorStr.Trim() + Environment.NewLine + Environment.NewLine +
                    "Error Message: " + logEx.Message.Trim() + Environment.NewLine + Environment.NewLine +
                    "Error Details: " + logEx.ToString().Trim();
            }

            UpdateLog(Severity.ERROR, errorMsg);
        }

        public async Task LogErrorAsync(string errorStr, Exception logEx = null)
        {
            string errorMsg = "";

            if (string.IsNullOrEmpty(errorStr) && logEx == null)
            {
                errorMsg = "Error Message and Exception were both null. (?)";
            }

            if (string.IsNullOrEmpty(errorStr) && logEx != null)
            {
                errorMsg = "Message: " + logEx.Message.Trim() + Environment.NewLine + Environment.NewLine +
                    "Error Details: " + logEx.ToString().Trim();
            }

            if (!string.IsNullOrEmpty(errorStr) && logEx == null)
            {
                errorMsg = errorStr.Trim();
            }

            if (!string.IsNullOrEmpty(errorStr) && logEx != null)
            {
                errorMsg = errorStr.Trim() + Environment.NewLine + Environment.NewLine +
                    "Error Message: " + logEx.Message.Trim() + Environment.NewLine + Environment.NewLine +
                    "Error Details: " + logEx.ToString().Trim();
            }

            await UpdateLogAsync(Severity.ERROR, errorMsg);
        }

        public void LogSuccess(string successStr)
        {
            UpdateLog(Severity.SUCCESS, successStr);
        }

        public async Task LogSuccessAsync(string successStr)
        {
            await UpdateLogAsync(Severity.SUCCESS, successStr);
        }

        public void LogDebug(string debugStr)
        {
            UpdateLog(Severity.DEBUG, debugStr);
        }

        public async Task LogDebugAsync(string debugStr)
        {
            await UpdateLogAsync(Severity.DEBUG, debugStr);
        }
        #endregion

        #region Log-Appends
        private bool AppendToLog(string appendStr)
        {
            try
            {
                System.IO.File.AppendAllText(PathToLogFile, appendStr.Trim() + Environment.NewLine);
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Failed to append updates to log file: " + ex.Message);
                return false;
            }
        }

        private async Task<bool> AppendToLogAsync(string appendStr)
        {
            try
            {
                await System.IO.File.AppendAllTextAsync(PathToLogFile, appendStr.Trim() + Environment.NewLine);
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Failed to append updates to log file: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Log-Imports
        public string ImportLog(string customFilePath = "")
        {
            if (string.IsNullOrEmpty(customFilePath))
            {
                return File.ReadAllText(PathToLogFile);
            }

            else
            {
                return File.ReadAllText(customFilePath);
            }
        }

        public async Task<string> ImportLogAsync(string customFilePath = "")
        {
            if (string.IsNullOrEmpty(customFilePath))
            {
                return await File.ReadAllTextAsync(PathToLogFile);
            }

            else
            {
                return await File.ReadAllTextAsync(customFilePath);
            }
        }
        #endregion

        #region Log-Exports
        /// <summary>
        /// Creates a copy of the existing log file at the provided file path.
        /// </summary>
        /// <param name="targetPath">Full (absolute) path to where the log file should be copied.</param>
        /// <param name="_overwrite">Whether to overwrite the file if it already exists at the target file path.</param>
        /// <returns>[Bool] True/False, whether export was successful.</returns>
        public bool ExportLog(string targetPath, bool _overwrite = true)
        {
            try
            {
                System.IO.File.Copy(PathToLogFile, targetPath, _overwrite);
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Failed to save log: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Creates a copy of the existing log file at the provided file path using async file IO. Overwrites existing file.
        /// </summary>
        /// <param name="targetPath">Full (absolute) path to where the log file should be copied.</param>
        /// <returns>[Bool] True/False, whether export was successful.</returns>
        public async Task<bool> ExportLogAsync(string targetPath)
        {
            try
            {
                /* Method 1
                 * string currentLogData = await File.ReadAllTextAsync(PathToLogFile);
                 * await File.WriteAllTextAsync(targetPath, currentLogData);
                 */

                // Method 2
                using (FileStream SourceStream = File.Open(PathToLogFile, FileMode.Open))
                {
                    using (FileStream DestinationStream = File.Create(targetPath))
                    {
                        await SourceStream.CopyToAsync(DestinationStream);
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                string errorStr = "[Error] " + ex.Message;
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Creates an interactable logging client to be used throughout your application.
        /// </summary>
        /// <param name="customLogOutput">[Optional] Select how the log output should be handled. Default writes to a log file and the Console.</param>
        /// <param name="customLogPath">[Optional] Set a custom log file path. Must be full (absolute) path.</param>
        /// <returns>Logging client object - a lumberjack to do your logging.</returns>
        public static LogClient Create(LogOutputType customLogOutput = LogOutputType.Default, string customLogPath = null)
        {
            LogClient _result = new();

            // Set log output type
            _result.LogOutput = customLogOutput;

            // No log path specified by user, set log path to use default
            if (string.IsNullOrEmpty(customLogPath)) { _result.LogFilePath = GenerateLogFileName(); }
            // Custom log path specified by user, override default log path
            if (!string.IsNullOrEmpty(customLogPath)) { _result.LogFilePath = customLogPath; }

            try
            {
                _result.LogInfo("LogClient successfully created and initialized! Running on Process: " + Process.GetCurrentProcess().ProcessName);

                // Obtain some basic information about the running application and environment.
                StringBuilder sb = new();
                foreach (var _arg in Environment.GetCommandLineArgs())
                {
                    sb.AppendLine(_arg);
                }

                string appArgs = sb.ToString().Trim();
                _result.LogInfo("[Arguments] " + appArgs);
                _result.LogInfo("[Current Directory] " + Environment.CurrentDirectory);
                _result.LogInfo("[Running as User] " + Environment.UserDomainName.ToString() + @"\" + Environment.UserName.ToString());
                _result.LogInfo("[OS Version] " + Environment.OSVersion.ToString());
                _result.LogInfo("[64-Bit OS] " + Environment.Is64BitOperatingSystem.ToString());
                _result.LogInfo("[64-Bit Process] " + Environment.Is64BitProcess.ToString());
                _result.LogInfo("[Process ID] " + Environment.ProcessId.ToString());
                _result.LogInfo("[Processor Count] " + Environment.ProcessorCount.ToString());

                return _result;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Failed to create LogClient! Error: " + ex.Message);
                Console.WriteLine("[Error Description] " + ex.ToString().Trim());
                return null;
            }
        }
    }
}
