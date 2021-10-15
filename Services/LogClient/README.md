# LogClient
Simple logging service for your app. 

## Getting Started
Basic setup guide using a C# .NET 5 Console App.

### Prerequisites
 - Create a new class in your project called "LogClient.cs"
 - Copy the code from https://github.com/johngagefaulkner/csharp-snippets/blob/main/Services/LogClient/LogClient.cs into your new class.

### Creating and using the LogClient
- Open the class containing your main entry point for the console application. By default, this is `Program.cs`.
- The boilerplate automatically written when creating a blank Console App is shown below:
```cs
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```

- Add "Snippets.Services" to your usings statements at the top. See example below:
```cs
using System;
using Snippets.Services;
```

- Replace the `Console.WriteLine("Hello World!");` code in your `Main` method with the following code:
```cs
static void Main(string[] args)
{
    Console.Title = "LogClient Tutorial";
    Console.WriteLine("Initializing, please wait...");
    LogClient Logger = LogClient.Create(true); // You can optionally display debug information, define log output types and/or a custom log file path.
    Logger.LogInfo("Initialization complete!");
    Console.WriteLine("");
    Console.Write("Press any key to exit:");
    Console.ReadKey();
}
```

- Save your changes and press F5 to debug the application.
- If successful, you should see the following output:

![ConsoleApp1](https://i.imgur.com/MKkZplH.png)

- You can navigate to `C:\Users\%username%\AppData\Local\ConsoleApp1.log` to view the output in the actual log file itself.
- See the example below:

![LogExample1](https://i.imgur.com/QqorGkD.png)
