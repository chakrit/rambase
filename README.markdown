RAM-based barely-transactional C# database which lets you forget about
persistence for a (very) big while.

What you need to do to use RAMbase is to create a class that contains all
of your models and mark it as `[Serializable]`.

    [Serializable]
    public class Root {
      public IList<User> Users { get; private set; }
      public IList<BlogPost> BlogPosts { get; private set; }
    }

Then surround your model-accessing code with

    using (var ctx = RAM.CreateContext<Root>())
    using (var scope = ctx.CreateReadScope()) {

      // do non-modifying stuff with your model

    }

or

    using (var ctx = RAM.CreateContext<Root>())
    using (var scope = ctx.CreateWriteScope()) {

      // modify your model

    }

The above `using` blocks ensure that you're not incorrectly writing to the
model from multiple threads resulting in inconsistent model state. Internally,
a `ReaderWriterLockSlim` is used.

RAMBase is actually a product of my frustration over having to install and
setup large relational databases (or any NoSQL for that matter) just to store
a small set of data.

Some projects just calls for a simple file-based persistence with all data
loaded into the RAM at runtime. Sure, you could throw in some JSON
serialization love or `BinaryFormatter` for its simplicity but why do that
time and time again? Just throw in `RAMBase` and forget about persistence
until your project gets really big.

# DOCS

By default RAMbase will try to persist your model to disk every 2 seconds after
a write or a read at the system's temp folder using the `BinaryFormatter`
(may change to custom serializer in the future -- I already have a working
implementation in several projects but it may not be generic enough).

To customize how RAMbase works, instead of creating a new context directly,
use the `Configure<T>()` method instead:

    var settings = RAM
      .Configure<Root>()
      .PersistAt(@"C:\MyAppFolder")
      .PersistMinSeconds(5);

The above code will create a RAMbase `Settings<T>` which saves data to
`C:\MyAppFolder` every 5 seconds (still needs a call into the context to
trigger the save) and then to obtain a new context, simply doL

    var context = settings.CreateContext();

More features will be added in the future if it proves to be of interest.

If you have any questions or suggestions, feel free to ping me on twitter
[@chakrit](http://twitter.com/chakrit) or just shoot me an email.

# TODO

If you find it interesting, a few points where I think might be good addition is:

* A better default persistence (I have one implementation using Reflection
  which I might add to RAMbase in the future).
* More persistence choices. JSON, XML, protobuf, blah blah.
* Some transaction support. Atomic writes, never corrupts save file etc.
* Compact runtime support. Silverlight, WP7 etc.

This code is PUBLIC DOMAIN. Use it however you want. Also, there is no
guarantee whatsoever of any use. Use it at your own risk.
