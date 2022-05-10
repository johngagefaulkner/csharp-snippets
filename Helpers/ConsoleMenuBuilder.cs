using System;

namespace Snippets.Helpers
{
    public class ConsoleMenuBuilder
    {

// It should be very simple to convert this into a method that accepts an array of strings as menu items/options, a Boolean whether to automatically generate an "exit" option, then returns a string or an int depending on the user's choice.  
   if (Bootstrap.TryInitialize(majorMinorVersion, out result))
            {
                int choice = 0;
                while (choice != 3)
                {
                    Console.WriteLine("\nMENU");
                    Console.WriteLine("1 - Register for state notifications");
                    Console.WriteLine("2 - Unregister for state notifications");
                    Console.WriteLine("3 - Quit");
                    Console.WriteLine("Select an option: ");
                    if (int.TryParse(Console.ReadLine(), out int tmp))
                    {
                        choice = tmp;
                        switch (choice)
                        {
                            case 1:
                                RegisterForStateNotifications();
                                break;
                            case 2:
                                UnregisterForStateNotifications();
                                break;
                            case 3:
                                break;
                            default:
                                Console.WriteLine($"*** Error: {choice} is not a valid choice ***");
                                break;
                        }
                    }
                }
    }
}
