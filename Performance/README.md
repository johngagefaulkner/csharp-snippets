# Performance

- These snippets generally apply to the performance of your application (in terms of how long it takes to open, etc.) but some of them are more focused on the speed and responsiveness while a user is *actually interacting* with the app. For example, the first snippet I'm adding is the fastest way to iterate over a collection of objects stored in a `List<T>`. (See below.)

## Examples

### Fastest Iteration of a Collection `List<T>`
  
```csharp
namespace Snippets.Performance;

internal static class DataCollections
{
  public record Customer(int Id, string FirstName, string LastName, string EmailAddress, string PhoneNumber);
  private static List<Customer> Customers = new()
  {
    new(0, John, Doe, john.doe@gmail.com, 6785881234),
    new(1, Mary, Smith, mary.smith@gmail.com, 7706861994)
  };
  public static IEnumerable<Customer> GetCustomers() => Customers;
  
	public void ReadWithForEachUsingSpan()
	{
		foreach (var _customer in CollectionsMarshal.AsSpan(Customers))
		{
			// Do whatever with the item here
			Console.WriteLine($"[{_customer.Id}] {_customer.FirstName} {_customer.LastName}");
			// Prints "[0] John Doe"
		}
	}
}
```
