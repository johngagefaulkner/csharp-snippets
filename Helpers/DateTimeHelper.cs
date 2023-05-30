using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;

namespace Snippets.Helpers
{
  public static class DateTimeHelper
  {
  		private static string GetFormattedLogMessage(string logMessage, Action sender) { $"[{DateTime.Now:HH:mm:ss.fff}] [{nameof(sender)}] {logMessage.Trim()}"; }
  }
}
