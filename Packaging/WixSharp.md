# WixSharp
> **Requires:** [WixSharp](https://github.com/oleg-shilo/wixsharp)

This is the bare minimum documentation because I didn't have time to copy everything from my phone earlier.

```csharp
using System;
using System.IO;
using System.Diagnostics;
using WixSharp;

namespace MyConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var project = new Project("MyProduct",
                            new Dir(@"%ProgramFiles%\My Company\My Product",
                                new File(@"Files\Docs\Manual.txt"),
                                new File(@"Files\Bin\MyApp.exe")));

            project.GUID = new Guid("6f330b47-2577-43ad-9095-1861ba25889b");

            Compiler.BuildMsi(project);
        }
    }
}
```
