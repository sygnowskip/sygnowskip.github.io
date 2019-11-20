---
title: "Natural identifiers in your domain (with Entity Framework Core) - generated by database"
categories:
  - domain
tags:
  - database identity
  - domain
  - domain driven design
  - entity framework
  - natural identifiers
layout: single
---

> *Part 2 is about configuring EF Core with databse side generated identifiers, but there is also a part 1 where a configuration for server side generated identifiers is explained*

This is a continuation of this [article](/domain/natural-identifiers-with-entity-framework-core-part-1/) 

## How to configure it in Entity Framework Core?

*Described solution is working fine on EF Core 3.0*

Configuration of Order class in EF Core:

```csharp
public class OrderId {
  public long Value { get; private set; }
}

public class Order {
  public OrderId Id { get; private set; }
  [...]
}

public class OrderConfiguration : IEntityTypeConfiguration<Order> {
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.ToTable(nameof(Order));
    builder.HasKey(order => order.Id);
        
    builder.Property(order => order.Id)
          .ValueGeneratedOnAdd();
  }
}

```
```sql
CREATE TABLE [dbo].[Order] (
[Id] [bigint] IDENTITY(1,1) NOT NULL,
[...]
)
```

There are few things that needs to be done before EF Core understands how to handle our identities correctly:

1. Identifiers have to implement following methods / interfaces:
    1. `Equals` / `GetHashCode`
    2. `IComparable`
    3. `== / !=` operators
2. Custom implementation for `IValueConverterSelector` and `ValueConverter<TIdentifier, TType>` with converter mapping hints that returns temporary identifiers and allows EF Core to track entities
3. Replacing internal EF Core implementation of `IValueConverterSelector` with our custom implementation

After all those changes EF Core will be able to understand LinQ queries with our identities and translates it correctly to SQL queries (will be evaluated on SQL with WHERE clause).

```csharp
public class OrderService : IOrderService {
  public Order GetOrder(OrderId id) {
    return dbContext.Orders.SingleOrDefault(c => c.Id == id);
  }
}
```
---

*There is one disadvantage of this solution - with current configuration EF Core migrations are not working correctly, so you need to use solutions like [DbUp](https://www.nuget.org/packages/dbup/) and SQL scripts.*

---

*There is one disadvantage of this solution - EF Core migrations are not working correctly, so you need to use solutions like [DbUp](https://www.nuget.org/packages/dbup/) and SQL scripts.*

---

Code with examples could be found on my [GitHub](https://github.com/sygnowskip/sygnowskip.github.io/tree/master/sources/2019-11-03-natural-identifiers-with-entity-framework-core)