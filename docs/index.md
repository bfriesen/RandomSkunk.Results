# RandomSkunk.Results

RandomSkunk.Results is a library that provides an alternative approach to exception-based error handling in C#. Using this approach, also known as <em>railway oriented programming</em>, methods will -- instead of throwing an exception -- return a <em>result object</em> that explicitly represents either the success or the failure of the method. In terms of functional programming, a <em>result object</em> from this library is both a <em>functor</em> and a <em>monad</em>, which in C# is basically just a fancy way of saying that a <em>result object</em> has `Select` and `SelectMany` methods and can be composed using LINQ syntax.

---

- [FAQ](faq.md)
