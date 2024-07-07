using System.Reflection;
using Microsoft.UI.Dispatching;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Snippets.Services.LogService
{
	public static class Log
	{
		private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
		private static readonly string Version = FileVersionInfo.GetVersionInfo(Assembly.Location).ProductVersion;
		private static readonly string AppName = (AppInfo.Current.DisplayInfo.DisplayName).Replace(" ", "");

		private static string GetCallerInfo()
		{
			StackTrace stackTrace = new();

			var methodName = stackTrace.GetFrame(3)?.GetMethod();
			var className = methodName?.DeclaringType.Name;
			return className + "::" + methodName?.Name;
		}

		private static string GetLogTimeStamp() => DateTime.Now.ToString("s");
		public static string LogFilePath()
		{
			var logDir = Path.Join(AppDataPaths.GetDefault().LocalAppData, @"\Logs");
			var mainLog = Path.Join(logDir, @$"\{AppName}-Main.log");

			if (!Directory.Exists(logDir))
			{
				Directory.CreateDirectory(logDir);
			}

			if (!File.Exists(mainLog))
			{
				StringBuilder sb = new();
				sb.AppendLine($"[{GetLogTimeStamp()}] [INFO] Log file created for {AppName}");
				sb.AppendLine($"[{GetLogTimeStamp()}] [INFO] Assembly: {Assembly}");
				sb.AppendLine($"[{GetLogTimeStamp()}] [INFO] Version: {Version}");
				sb.AppendLine("");
				string initialLogData = sb.ToString();

				File.WriteAllText(mainLog, initialLogData);
			}

			return mainLog;
		}

		public enum LogSeverity
		{
			Information = 0,
			Warning = 1,
			Error = 2,
			Fatal = 3,
			Debug = 4,
			Verbose = 5
		}

		private static Dictionary<LogSeverity, string> logSeverityToStringData = new()
		{
			{ LogSeverity.Information, "INFO" },
			{ LogSeverity.Warning, "WARN" },
			{ LogSeverity.Error, "ERROR" },
			{ LogSeverity.Fatal, "FATAL" },
			{ LogSeverity.Debug, "DEBUG" },
			{ LogSeverity.Verbose, "VERBOSE" }
		};

		/// <summary>
		/// Appends the provided <see langword="string"/> to the log file.
		/// </summary>
		/// <param name="_logMessage">The text <see cref="string"/> to add to the log file.</param>
		/// <param name="_logSeverity">The severity of the message being logged.</param>
		private static void AddLogEntry(string _logMessage, LogSeverity _logSeverity)
		{
			var logSeverityString = logSeverityToStringData[_logSeverity];
			var fullLogMessage = $"[{GetLogTimeStamp()}] [{logSeverityString}] [{GetCallerInfo()}] {_logMessage}";
			var finalLogMessage = fullLogMessage.Trim() + Environment.NewLine;

			File.AppendAllText(LogFilePath(), finalLogMessage);
		}

		private static async Task AddLogEntryAsync(string _logMessage, LogSeverity _logSeverity)
		{
			var logSeverityString = logSeverityToStringData[_logSeverity];
			var fullLogMessage = $"[{GetLogTimeStamp()}] [{logSeverityString}] {_logMessage}";
			var finalLogMessage = fullLogMessage.Trim() + Environment.NewLine;

			await File.AppendAllTextAsync(LogFilePath(), finalLogMessage, Encoding.UTF8, CancellationToken.None);
		}

		/// <summary>
		/// Logs an informational message.
		/// </summary>
		/// <param name="logMessage">The informational message as a <see cref="string"/>.</param>
		public static void Information(string logMessage) => AddLogEntry(logMessage, LogSeverity.Information);

		/// <summary>
		/// Logs a warning message.
		/// </summary>
		/// <param name="logMessage">The warning message as a <see cref="string"/>.</param>
		public static void Warning(string logMessage) => AddLogEntry(logMessage, LogSeverity.Warning);
		public static void Error(string logMessage) => AddLogEntry(logMessage, LogSeverity.Error);

		/// <summary>
		/// Logs an error message with included Exception data.
		/// </summary>
		/// <param name="logEx">The <see cref="Exception"/> that was thrown.</param>
		/// <param name="logMessage">Custom <see cref="string"/> data, generally used to provide some context.</param>
		public static void Error(Exception logEx, string logMessage)
		{
			StringBuilder sb = new();
			sb.AppendLine(logMessage);
			sb.AppendLine($"[ Exception Message ] {logEx.Message}");

			if (logEx.InnerException is not null && logEx.InnerException.Message is not null)
			{
				sb.AppendLine($"[  Inner Exception  ] {logEx.InnerException.Message}");
			}

			if (logEx.StackTrace is not null)
			{
				sb.AppendLine($"[    StackTrace     ] {logEx.StackTrace}");
			}

			var fullLogMessage = sb.ToString().Trim();
			AddLogEntry(fullLogMessage, LogSeverity.Error);
		}

		public static void Fatal(Exception logEx, string logMessage) => Error(logEx, logMessage);
		public static void Debug(string logMessage)
		{
			bool isSuccess = false;

			try
			{
				AddLogEntry(logMessage, LogSeverity.Debug);
				isSuccess = true;
			}

			catch (Exception ex)
			{
				Error(ex, "Failed to add a [DEBUG] log entry!");
				isSuccess = false;

				try
				{
					DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
					{
						AddLogEntry(logMessage, LogSeverity.Debug);
					});

					isSuccess = true;
				}

				catch (Exception _ex)
				{
					Error(_ex, "Failed to add a [DEBUG] log entry using DispatcherQueue!");
					isSuccess = false;
				}
			}

			finally
			{
				if (isSuccess == false)
				{
					Console.WriteLine($"[ERROR] Failed to write a [DEBUG] log entry! Message: {logMessage}");
				}
			}
		}

		public static void Verbose(string logMessage) => AddLogEntry(logMessage, LogSeverity.Verbose);
	}
}
