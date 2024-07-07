using System;
using System.Threading;
using System.Windows;

namespace Snippets.Services
{
	public class DispatcherQueueService
	{
		// Ensures that the command/action/task below (in this case, "AddLogEntry") is ran on the UI thread
		public static void OutputMessage(string message)
		{
			DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
			{
				AddLogEntry(logMessage, LogSeverity.Debug);
			});
		}
	}
}
