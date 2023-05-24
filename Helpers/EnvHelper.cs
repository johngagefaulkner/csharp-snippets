using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Windows;

namespace Snippets.Helpers;

public static class EnvHelper
{
  // Returns the full path to the LocalAppData folder for the currently signed-in user.
  public static string LocalApplicationDataFolder()
  {
      WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
      SecurityIdentifier currentUserSID = currentUser.User;

      SecurityIdentifier localSystemSID = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);
      if (currentUserSID.Equals(localSystemSID) && UserLocalAppDataPath != string.Empty)
      {
          return UserLocalAppDataPath;
      }
      else
      {
          return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      }
  }

  // Returns the full path to the running instance of the app.
  public static string GetCurrentAppInstallationFolder()
  {
      var settingsPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
      return Directory.GetParent(settingsPath).FullName;
  }
  
  // Launches an exe that's stored in the same folder as the running instance of your app.
  public static void LaunchLocalExeWithArgs(string exeName, string _args)
  {
      try
      {
          var targetAppPath = System.IO.Path.Combine(GetCurrentAppInstallationFolder(), exeName);
          Process.Start(new ProcessStartInfo(targetAppPath) { Arguments = $"{_args}" });
      }
      catch
      {
          // TODO(stefan): Log exception once unified logging is implemented
      }
  }
}
