![Beneficial Strategies](https://raw.githubusercontent.com/Beneficial-Strategies/iso-20022-messages-samples/main/.beneficial/logo.png)

# BeneficialStrategies.Iso20022.Messages.Samples

Named, valid ISO 20022 objects for tests, and a generated minimal valid object for any interface of the model, built through the Factory against the interfaces, so a test needs no message records.

**Named ISO 20022 objects for your tests.** When a test needs "a valid account identification", "a valid cash account" or, in time, "a valid payment message", it takes one from here instead of building it by hand: no ten lines of setup, no message records to install, and the same object every time.

The samples are built against the **interfaces** of the model ([BeneficialStrategies.Iso20022.Messages](https://www.nuget.org/packages/BeneficialStrategies.Iso20022.Messages)), the contract the rest of the family is written to, with the objects made by the [Factory](https://www.nuget.org/packages/BeneficialStrategies.Iso20022.Factory). So a sample works with anything that takes the interface: the validators, a consumer of a message bus, a method of your own.

## Install

```
dotnet add package BeneficialStrategies.Iso20022.Messages.Samples --prerelease
```

The package is in alpha, so the command asks for a prerelease version. It depends on `Primitives`, `Messages` and `Factory`, not on the message records, and targets netstandard2.0, net8.0 and net10.0. Four examples follow, each a step further.

## How to use

### Take a sample

A sample is a static property. Its path says what it is: the type, `Valid`, the scenario, and how much of it is filled.

<!-- snippet: examples/01-take-a-sample/Program.cs#take -->
```csharp
// a valid account identification, the IBAN variant of the choice
IAccountIdentification4Choice id = Samples.AccountIdentification4Choice.Valid.IBAN.Minimal;

Console.WriteLine(id);                                                    // IIBAN_1 { Value = DE89370400440532013000 }
if (id is IIBAN iban) Console.WriteLine($"IBAN {iban.Value}");            // IBAN DE89370400440532013000
```
<!-- /snippet -->

**Every access builds a new object**, so a test can never change what another test reads: the objects have setters (the Factory's classes need them), and a shared sample would be a shared variable.

<!-- snippet: examples/01-take-a-sample/Program.cs#new-object -->
```csharp
var again = Samples.AccountIdentification4Choice.Valid.IBAN.Minimal;
Console.WriteLine($"{ReferenceEquals(id, again)} {id.Equals(again)}");    // False True: two objects, equal by value
```
<!-- /snippet -->

Try the [project](https://github.com/Beneficial-Strategies/iso-20022-messages-samples/tree/main/examples/01-take-a-sample) yourself.

### Samples are built from samples

A bigger sample is made of the samples of its parts. A valid cash account contains a valid account identification:

<!-- snippet: examples/02-samples-from-samples/Program.cs#part -->
```csharp
ICashAccount40 account = Samples.CashAccount40.Valid.Standard.Minimal;
Console.WriteLine(((IIBAN)account.Identification!).Value);                // DE89370400440532013000: the IBAN sample of example 01
```
<!-- /snippet -->

Most scenarios have two shapes: **`Minimal`** has few members set, **`Maximal`** more. They are both valid, and the second is there for the test that needs the optional parts:

<!-- snippet: examples/02-samples-from-samples/Program.cs#shapes -->
```csharp
var small = Samples.AccountIdentification4Choice.Valid.Other.Minimal;
var large = Samples.AccountIdentification4Choice.Valid.Other.Maximal;
Console.WriteLine($"{small.Identification} scheme={(small.SchemeName is null ? "none" : "set")}");   // ACCT-001 scheme=none
Console.WriteLine($"{large.Identification} scheme={(large.SchemeName is null ? "none" : "set")}");   // ACCT-001 scheme=set
```
<!-- /snippet -->

The scheme name of the `Maximal` is itself a sample (`AccountSchemeName1Choice.Valid.Proprietary.Minimal`), so a message's `Minimal` is made of the `Minimal` of its header and its parts, all the way down.

Try the [project](https://github.com/Beneficial-Strategies/iso-20022-messages-samples/tree/main/examples/02-samples-from-samples) yourself.

### Vary a sample

A sample is an ordinary object of the interface, so it goes wherever the interface does: into a validator, a handler, a serializer. When a test needs a **variation**, copy it and change what the test is about. The sample itself is untouched:

<!-- snippet: examples/03-vary-a-sample/Program.cs#with -->
```csharp
var changed = IsoDataFactory.With(Samples.AccountIdentification4Choice.Valid.Other.Minimal, b => b
    .Set(x => x.Identification).To("OTHER-2"));

Console.WriteLine(changed.Identification);                                                  // OTHER-2
Console.WriteLine(Samples.AccountIdentification4Choice.Valid.Other.Minimal.Identification);  // ACCT-001
```
<!-- /snippet -->

Try the [project](https://github.com/Beneficial-Strategies/iso-20022-messages-samples/tree/main/examples/03-vary-a-sample) yourself.

### A sample for any type

The named samples cover what tests have needed so far. For any other interface of the model, `IsoSample` makes a valid object from the interface itself: its required members, no others.

<!-- snippet: examples/04-a-sample-for-any-type/Program.cs#any -->
```csharp
IGenericIdentification3 id = Samples.IsoSample.Minimal<IGenericIdentification3>();
Console.WriteLine(id);                                          // IGenericIdentification3_1 { Identification = A, Issuer = null }

var account = Samples.IsoSample.Minimal<IAccountIdentification4Choice>();
Console.WriteLine(account);                                     // IIBAN_2 { Value = DE89370400440532013000 }: a named sample wins, wherever the type appears
```
<!-- /snippet -->

A list gets one item, a choice its first variant (by name), a code set its first value, and a primitive the first value of its format that the type accepts. It is the same on every call, and a new object each time. The values are placeholders that fit the format, so write no test that depends on what they are.

Where the rules ask for more than the shape says (one of two optional members must be present, a list of at least two), the object is built to the shape and a validator says no. Those types need a named sample. Writing the sample (`Samples/<Type>/Valid/Standard.cs`) fixes it for every object that contains the type.

Try the [project](https://github.com/Beneficial-Strategies/iso-20022-messages-samples/tree/main/examples/04-a-sample-for-any-type) yourself.

## More

* Documentation, release notes and issues: https://github.com/Beneficial-Strategies/iso-20022-messages-samples
* License: MIT. Status: alpha.

### What is here, and what is coming

Today: the variants of 24 choices (`AccountIdentification4Choice`, `AmountType4Choice`, `Party50Choice`, ...), the components they need (`CashAccount40`, `PostalAddress27`, ...) and one message (`MultilateralSettlementRequestV02`). The values in them are those of the tests they were moved out of, so `Minimal` is "few members" and `Maximal` is "more members", not yet "only the required members" and "every member" in the sense of the model.

Any other interface is available through `IsoSample.Minimal<T>()`. Coming, as the tests that use them are moved onto the samples: the rest of the messages, components and choices; samples for the **primitive values** (simple types, code sets, amounts); the `Invalid` samples, one class for each known issue; and `Maximal` for the whole model (every optional member the rules allow together).

### The standard for samples

Every sample follows one pattern, so a sample is found by its name:

```
Samples/<Type>/Valid/<Scenario>.cs     a static class per scenario, with Minimal and (where it differs) Maximal
Samples/<Type>/Invalid/<Issue>.cs      one static class per known issue
```

* **Namespace and name:** `BeneficialStrategies.Iso20022.Messages.Samples.<Type>.Valid.<Scenario>`. `<Type>` is the name of the message, component or choice as in the model. For a **variant of a choice**, `<Scenario>` is the variant (`AccountIdentification4Choice.Valid.Other`); for **any other type** it is `Standard` (`CashAccount40.Valid.Standard`).
* **`Minimal` and `Maximal`:** two valid shapes; `Maximal` has more members set, and exists only where it differs from `Minimal`.
* **Properties, not fields,** so each access builds a new object.
* **Built from other samples,** with `IsoDataFactory.Make<T>` (and `IsoDataFactory.With` to change a copy of another); a sample makes objects of the interfaces only that way.
* **A name that is also a type of `System` gets a trailing underscore:** `Valid.DateTime_`, `Valid.Type_` (the ones the model has are `DateTime`, `Decimal`, `Index`, `Range`, `Single`, `Type` and `ValueType`).

<!-- end of package readme -->

## Building and contributing

```
dotnet build -c Release
dotnet test -c Release --no-build
tools/run-examples.sh              # packs the package, then builds and runs every example against it
python3 tools/sync-readme.py       # fills the code blocks above from the examples
```

Whether the samples are *valid* is checked by the validators, which live in another repository (the .NET libraries for ISO 20022): a test there builds the `Minimal` sample of every interface that has a validator and asserts it is valid. A bug report or a change starts with an issue. See CONTRIBUTING.md.
