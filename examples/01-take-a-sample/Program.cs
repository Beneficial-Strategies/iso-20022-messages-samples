// Example 01: take a sample.
// Shows:  a named sample of a choice, and that every access builds a new object.
// Needs:  BeneficialStrategies.Iso20022.Messages.Samples (which brings the interfaces).
// Prints: the sample, and whether two accesses give the same object and equal objects.
using BeneficialStrategies.Iso20022.Choices;
using BeneficialStrategies.Iso20022.Choices.AccountIdentification4Choice;
using Samples = BeneficialStrategies.Iso20022.Messages.Samples;

// <snippet take>
// a valid account identification, the IBAN variant of the choice
IAccountIdentification4Choice id = Samples.AccountIdentification4Choice.Valid.IBAN.Minimal;

Console.WriteLine(id);                                                    // IIBAN_1 { Value = DE89370400440532013000 }
if (id is IIBAN iban) Console.WriteLine($"IBAN {iban.Value}");            // IBAN DE89370400440532013000
// </snippet>

// <snippet new-object>
var again = Samples.AccountIdentification4Choice.Valid.IBAN.Minimal;
Console.WriteLine($"{ReferenceEquals(id, again)} {id.Equals(again)}");    // False True: two objects, equal by value
// </snippet>
