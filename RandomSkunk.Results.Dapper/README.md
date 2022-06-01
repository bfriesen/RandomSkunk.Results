# RandomSkunk.Results.Dapper [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.Dapper.svg)](https://www.nuget.org/packages/RandomSkunk.Results.Dapper)

## IDbConnection extension methods

This library contains extension methods for `IDbConnection` - all starting with `Try` - that mirror the extension methods from the `Dapper` library, but return a `Result<T>` or `Maybe<T>`. Each of these `Try` methods call their corresponding `Dapper` method inside a try/catch block. If no exception is thrown, a `Result<T>` or `Maybe<T>` is returned with its value corresponding with the value returned by Dapper. If an exception is thrown, a `Fail` result is returned with its error created from the exception.

```c#
public async Task<Result<IEnumerable<Order>>> GetOrders(Guid userId)
{
    using IDbConnection connection = GetConnection();
    return await connection.TryQueryAsync<Order>(
        "SELECT * FROM Orders WHERE UserId = @userId",
        new { userId });
}

public async Task<Result<int>> AddOrderLineItem(OrderLineItem item)
{
    using IDbConnection connection = GetConnection();
    return await connection.TryExecuteAsync(
        "INSERT INTO OrderLineItems " +
        "VALUES (@OrderId, @ItemId, @Quantity, @Price);",
        item);
}
```
