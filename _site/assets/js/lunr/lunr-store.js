var store = [{
        "title": "Natural identifiers in your domain (with Entity Framework Core) - part 1",
        "excerpt":"   Part 1 is about configuring EF Core with server side generated identifiers, but there is also a part 2 where a configuration for database generated identifiers is explained    How often do you see code like this in your applications?   public interface IPaymentService {   /// &lt;summary&gt;   /// Method for creating Payment for specific Order   /// &lt;/summary&gt;   /// &lt;param name=\"orderId\"&gt;Order identifier&lt;/param&gt;   /// &lt;returns&gt;Payment identifier&lt;/returns&gt;   long CreatePayment(long orderId); }   What’s wrong with them?      any long value could be passed as their arguments   long as a return type is not descriptive and understandable (usually, you need to check details of the implementation or check the documentation)   when code changes, the comment needs to be changed or removed (or will become not related to code)   How could we improve this code?   In the Domain Driven Design there is a concept of entities that represent domain objects and are primarily defined by their identity (you could find more information about DDD in other blogs / books).   In our example, we could identify two different entities: Order and Payment. Both of them should have separated identifiers that are immutable and globally unique.   So, we could change our code to something like this:  public interface IPaymentService {   PaymentId CreatePayment(OrderId orderId); }   Why this code is better?      self-documenting code (input and output is clearly described by types)   static type checking at compile time (not able to pass CustomerId as OrderId)   How to configure it in Entity Framework Core?   Described solution is working fine on EF Core 2.2 / 3.0   Main problem with solution described above is to configure EF Core to handle it properly and out of the box for all our domain entities and identifiers.   Configuration of Order class in EF Core:   public class OrderId {   public Guid Value { get; private set; } }  public class Order {   public OrderId Id { get; private set; }   [...] }  public class OrderConfiguration : IEntityTypeConfiguration&lt;Order&gt; {   public void Configure(EntityTypeBuilder&lt;Order&gt; builder)   {     builder.ToTable(nameof(Order));     builder.HasKey(order =&gt; order.Id);   } }    Code execution will end with exception like:      System.InvalidOperationException : The property ‘Order.Id’ is of type ‘OrderId’ which is not supported by current database provider. Either change the property CLR type or ignore the property using the ‘[NotMapped]’ attribute or by using ‘EntityTypeBuilder.Ignore’ in ‘OnModelCreating’.    There are few things that needs to be done before EF Core understands how to handle our identities correctly:      Identifiers have to implement following methods / interfaces:            Equals / GetHashCode       IComparable       == / != operators           Custom implementation for IValueConverterSelector and ValueConverter&lt;TIdentifier, TType&gt;   Replacing internal EF Core implementation of IValueConverterSelector with our custom implementation   After all those changes EF Core will be able to understand LinQ queries with our identities and translates it correctly to SQL queries (will be evaluated on SQL with WHERE clause).   public class OrderService : IOrderService {   public Order GetOrder(OrderId id) {     return dbContext.Orders.SingleOrDefault(c =&gt; c.Id == id);   } }   There is also a second part of this article with proper configuration for database side generated identities - part 2     Code with examples could be found on my GitHub  ","categories": ["domain"],
        "tags": ["server side generated","domain","domain driven design","entity framework","natural identifiers"],
        "url": "http://localhost:4000/domain/natural-identifiers-with-entity-framework-core-part-1/",
        "teaser":null},{
        "title": "Natural identifiers in your domain (with Entity Framework Core) - part 2",
        "excerpt":"   Part 2 is about configuring EF Core with databse side generated identifiers, but there is also a part 1 where a configuration for server side generated identifiers is explained    This is a continuation of this article   How to configure it in Entity Framework Core?   Described solution is working fine on EF Core 3.0   Configuration of Order class in EF Core:   public class OrderId {   public long Value { get; private set; } }  public class Order {   public OrderId Id { get; private set; }   [...] }  public class OrderConfiguration : IEntityTypeConfiguration&lt;Order&gt; {   public void Configure(EntityTypeBuilder&lt;Order&gt; builder)   {     builder.ToTable(nameof(Order));     builder.HasKey(order =&gt; order.Id);              builder.Property(order =&gt; order.Id)           .ValueGeneratedOnAdd();   } }   CREATE TABLE [dbo].[Order] ( [Id] [bigint] IDENTITY(1,1) NOT NULL, [...] )   There are few things that needs to be done before EF Core understands how to handle our identities correctly:      Identifiers have to implement following methods / interfaces:            Equals / GetHashCode       IComparable       == / != operators           Custom implementation for IValueConverterSelector and ValueConverter&lt;TIdentifier, TType&gt; with converter mapping hints that returns temporary identifiers and allows EF Core to track entities   Replacing internal EF Core implementation of IValueConverterSelector with our custom implementation   After all those changes EF Core will be able to understand LinQ queries with our identities and translates it correctly to SQL queries (will be evaluated on SQL with WHERE clause).   public class OrderService : IOrderService {   public Order GetOrder(OrderId id) {     return dbContext.Orders.SingleOrDefault(c =&gt; c.Id == id);   } }     There is one disadvantage of this solution - EF Core migrations are not working correctly, so you need to use solutions like DbUp and SQL scripts.     Code with examples could be found on my GitHub  ","categories": ["domain"],
        "tags": ["database identity","domain","domain driven design","entity framework","natural identifiers"],
        "url": "http://localhost:4000/domain/natural-identifiers-with-entity-framework-core-part-2/",
        "teaser":null}]
