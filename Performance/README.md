# Performance

- These snippets generally apply to the performance of your application (in terms of how long it takes to open, etc.) but some of them are more focused on the speed and responsiveness while a user is *actually interacting* with the app. For example, the first snippet I'm adding is the fastest way to iterate over a collection of objects stored in a `List<T>`. (See below.)

## Examples

### Fastest Iteration of a Collection `List<T>`
  
```csharp
private static List<int> _list = new();

public void ReadWithForEachUsingSpan()
{
    foreach (var item in CollectionsMarshal.AsSpan(_list))
    {
       // Do whatever with the item here
    }
}
```
