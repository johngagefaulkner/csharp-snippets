# Snippets / Data

Will contain snippets related to processing data.

## Basic User Model Example

```csharp
public class User
{
	public int Id { get; set; }

	private string _name = string.Empty;

	public string Name
	{
		get { return _name; }
		set { _name = value; }
	}

	public User()
	{
	}
}
```
