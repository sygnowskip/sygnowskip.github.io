---
title: "Natural identifiers in your domain (with Entity Framework Core) - generated on server side"
categories:
  - domain
tags:
  - server side generated
  - domain
  - domain driven design
  - entity framework
  - natural identifiers
layout: single
---

> *Part 1 is about configuring EF Core with server side generated identifiers, but there is also a part 2 where a configuration for database generated identifiers is explained*

How often do you see code like this in your applications?

```csharp
public interface IPaymentService {
  /// <summary>
  /// Method for creating Payment for specific Order
  /// </summary>
  /// <param name="orderId">Order identifier</param>
  /// <returns>Payment identifier</returns>
  long CreatePayment(long orderId);
}
```

What's wrong with them? 

* any `long` value could be passed as their arguments
* `long` as a return type is not descriptive and understandable (usually, you need to check details of the implementation or check the documentation)
* when code changes, the comment needs to be changed or removed (or will become not related to code)

## How could we improve this code?

In the Domain Driven Design there is a concept of entities that represent domain objects and are primarily defined by their identity (you could find more information about DDD in other blogs / books). 

In our example, we could identify two different entities: Order and Payment. Both of them should have separated identifiers that are immutable and globally unique.

So, we could change our code to something like this:
```csharp
public interface IPaymentService {
  PaymentId CreatePayment(OrderId orderId);
}
```

Why this code is better?

* self-documenting code (input and output is clearly described by types)
* static type checking at compile time (not able to pass `CustomerId` as `OrderId`)

## How to configure it in Entity Framework Core?

*Described solution is working fine on EF Core 2.2 / 3.0*

Main problem with solution described above is to configure EF Core to handle it properly and out of the box for all our domain entities and identifiers.

Configuration of Order class in EF Core:

```csharp
public class OrderId {
  public Guid Value { get; private set; }
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
  }
}

```

Code execution will end with exception like:

> System.InvalidOperationException : The property 'Order.Id' is of type 'OrderId' which is not supported by current database provider. Either change the property CLR type or ignore the property using the '[NotMapped]' attribute or by using 'EntityTypeBuilder.Ignore' in 'OnModelCreating'.

There are few things that needs to be done before EF Core understands how to handle our identities correctly:

1. Identifiers have to implement following methods / interfaces:
    1. `Equals` / `GetHashCode`
    2. `IComparable`
    3. `== / !=` operators
2. Custom implementation for `IValueConverterSelector` and `ValueConverter<TIdentifier, TType>`
3. Replacing internal EF Core implementation of `IValueConverterSelector` with our custom implementation

After all those changes EF Core will be able to understand LinQ queries with our identities and translates it correctly to SQL queries (will be evaluated on SQL with WHERE clause).

```csharp
public class OrderService : IOrderService {
  public Order GetOrder(OrderId id) {
    return dbContext.Orders.SingleOrDefault(c => c.Id == id);
  }
}
```

There is also a second part of this article with proper configuration for database side generated identities - [part 2](/domain/natural-identifiers-with-entity-framework-core-part-2/) 

---

Code with examples could be found on my [GitHub](https://github.com/sygnowskip/sygnowskip.github.io/tree/master/sources/2019-11-03-natural-identifiers-with-entity-framework-core)