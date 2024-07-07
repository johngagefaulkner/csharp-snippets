using System.Reflection;
using Windows.Storage;

namespace Snippets.Services
{
	public static class MicrosoftLogger
	{
		private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
		private static readonly string Version = FileVersionInfo.GetVersionInfo(Assembly.Location).ProductVersion;

		private static readonly string Error = "Error";
		private static readonly string Warning = "Warning";
		private static readonly string Info = "Info";
		private static readonly string Debug = "Debug";
		private static readonly string TraceFlag = "Trace";

		/// <summary>
		/// Initializes the logger and sets the path for logging.
		/// </summary>
		/// <example>InitializeLogger("\\<see cref="Assembly.FullName"/>\\Logs")</example>
		/// <param name="applicationLogPath">The path to the log files folder.</param>
		/// <param name="isLocalLow">If the process using Logger is a low-privilege process.</param>
		public static void InitializeLogger(string applicationLogPath, bool isLocalLow = false)
		{
			try
			{
				if (isLocalLow)
				{
					applicationLogPath = @$"{Environment.GetEnvironmentVariable("userprofile")}\AppData\LocalLow\WaferLauncher\{Version}";
				}

				else
				{
					applicationLogPath = Path.Combine(AppDataPaths.GetDefault().LocalAppData, applicationLogPath, @"\", Version);
				}

				if (!Directory.Exists(applicationLogPath))
				{
					Directory.CreateDirectory(applicationLogPath);
				}

				var logFilePath = Path.Combine(applicationLogPath, "Log_" + DateTime.Now.ToString(@"yyyy-MM-dd", CultureInfo.InvariantCulture) + ".log");

				Trace.Listeners.Add(new TextWriterTraceListener(logFilePath));
				Trace.AutoFlush = true;
			}

			catch (Exception ex)
			{
				Console.WriteLine($"[ERROR] Failed to initialize logger with error message: {ex.Message}");
			}
		}

		public static void LogError(string message, Exception ex)
		{
			StringBuilder sb = new();
			sb.AppendLine(message);
			sb.AppendLine($"[ Exception Message ] {ex.Message}");
			sb.AppendLine($"[  Inner Exception  ] {ex.InnerException.Message}");
			sb.AppendLine($"[    StackTrace     ] {ex.StackTrace}");
			var logStr = sb.ToString();

			Log(logStr, Error);
		}

		public static void LogError(string message) => Log(message, Error);
		public static void LogWarning(string message) => Log(message, Warning);
		public static void LogInfo(string message) => Log(message, Info);
		public static void LogDebug(string message) => Log(message, Debug);
		public static void LogTrace() => Log(string.Empty, TraceFlag);

		private static void Log(string message, string type)
		{
			Trace.WriteLine($"[{DateTime.Now.TimeOfDay}] [{type}] {GetCallerInfo()}");
			Trace.Indent();
			if (message != string.Empty)
			{
				Trace.WriteLine(message);
			}

			Trace.Unindent();
		}

		private static string GetCallerInfo()
		{
			StackTrace stackTrace = new();

			var methodName = stackTrace.GetFrame(3)?.GetMethod();
			var className = methodName?.DeclaringType.Name;
			return className + "::" + methodName?.Name;
		}
	}
}
