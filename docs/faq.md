# Frequently Asked Questions

---

### Creating a result of type `Nullable<T>` from a value

**Q:**  I have a value of type `int?`, and I need to construct a `Maybe<int>` from it, where a `null` value creates a `None` result, and a non-null value creates a `Success` result. I would like to be able to call the `Maybe<int>.FromValue` method, but it won't take a `null` value, only a regular `int`. And I could call `Maybe<int?>.FromValue`, but it returns a `Maybe<int?>`, which isn't exactly what I need. How do I get exactly what I need without having to write too much code?

```c#
int? value;

// Does not compile:
Maybe<int> result = Maybe<int>.FromValue(value);

// Not quite what we need:
Maybe<int?> result = Maybe<int?>.FromValue(value);

// Too complicated:
Maybe<int> result = value is null
	? Maybe<int>.None
	: Maybe<int>.Success(value.Value);
```

**A:** The solution is to first create a result with a nullable value, then convert it to a result with a non-nullable value with the `AsNonNullable` extension method.

```c#
int? value;

// Exactly what we need in one line:
Maybe<int> result = Maybe<int?>.FromValue(value).AsNonNullable();
```

---
