# Entity Framwork Guidelines

Entity Framework (EF) is the recommended ORM to use these days. There
are other options, but most documentation (read: stackoverflow posts)
use EF.

In EF, you make C# objects to represent your table, use special
attributes or naming conventions to tell the EF database generator
what tables to make. This is called the "code-first" method. EF has
migrations built in, and detects changes to your C# objects usign
reflection to generate up/down migrations.

## Connection management

The EF main object is the `DbContext`, which has a `DbSet<T>` for each
of your model classes. This is the main handle on the database, and
everything comes from here. It's doing some result caching, but as
with any ORM it's stupid easy to fetch more than you want. Your code
changes objects in this context, then at some point you must call
`DbContext.SaveChanges` to commit the work to the database. It's kinda
like a transaction. Some common errors when you `SaveChanges`:

 * EF model validation errors
 * DB constraint violations

The `DbContext` is not thread-safe, but should be re-used to take
advantage of its result caching. Use Ninject web extensions to create
one of these per HTTP request.

Ninject can thread these around for you, but sometimes for one-off
things or long-lived objects it might make more sense to just
instantiate the dbcontext directly.

## Connection strings

EF will look in the config file for a connection string named after
your context. Use this along with the web publish options in visual
studio to re-write config files at publish time to use different
databases. This was you can always say `new MyContext()` and have it
work, and not need to thread the single dbcontext instance across the
universe.

## Model classes

 * put code-first database models in their own assembly. To
   generate/run migrations, you just need a compiled assembly and a
   connection string. If you change the model, you're gonna break some
   code. If your models and code are in the same assembly, you won't
   be able to generate a migration until you've fixed or commented out
   everything.
 * EF can handle inheritence, so make a `TableBase` with `Id` and
   `DateEntered` and inherit other classes from there
 * When making relationships in the model classes, make a `public int
   FooId {get;set;}` and a `public virtual Foo {get;}`. `FooId` will
   be the column with the foriegn key, and `Foo` will lazy-load the
   related object. You can do `public virtual Foo {get;set;}` and
   achieve a similar database, but some parts of the relation are much
   easier to deal with if you have direct access to the id - notably
   dropdowns in editors.
 * on model classes, adding a `public virtual ICollection<T> Models
   {get;set;}` will create lazy-loaded accessor for the other side of
   DB relationships. Initialize these in constructors to avoid errors
   when working with newly created objects, or implement a lazy
   initialzed collection, which is some annoying boilerplate
 * if there's going to be a lot of objects in a virtual collection,
   establish the relationship on the child by setting the foreign key
   property and add it to the dbcontext directly. For example:
   
	    // SELECTs all the votes for the election into the collection, 
		// then queues the INSERT
        election.Votes.Add(new Vote(){For="Bob"});
		
		// queues the INSERT
		dbcontext.Votes.Add(new Vote(){For="Bob", ElectionId=election.Id});

## Migrations

 * use the `Seed` method to setup things like type tables, admin
   users, and roles

## Querying

 * don't use `Contains` in EF5 LINQ queires, that prevents LINQ from
   caching query plans. See http://msdn.microsoft.com/en-us/data/hh949853.aspx
 * EF LINQ has a lot of weird constraints, but if you follow them then
   you'll only select the data you need from DB. Expect to get runtime
   errors like "we don't know how to do X from a LINQ query" and need
   to re-write the query. Some common ones:
    * use parameter-less constructors with the object initializer syntax
	* don't try to call static methods
 * can compose a query from multiple lambda expressions, but you
   probably don't want to try to pass those between methods. The type
   signature is unholy for the lambda:
   
        query.Where(f => f.Name == "bar")
	
 * don't pay too much attention to the crazy SQL it generates; it's
   usually fast enough


##### scratch

allow `new DbContext()`, or require it be passed in via a
method/constructor argument?

`new` is ok when:

 * no model objects coming in or out of the function - things get
   hairy if you mix objects from different contexts
 * we might be a long-lived object that survives multiple HTTP
   requests, and therefore can't pull the one off the request context
 * the data we're dealing with probably isn't going to be cached in
   the per-request context
 * you're ok with hitting a real db to test this function
 * we're _always_ setting the connection string via the default name
   convention with app.config
 * we're _never_ going to do anything funky with the context that
   would require ninject
 * we're _never_ doing things with transactions

