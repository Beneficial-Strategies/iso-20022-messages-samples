// Example 02: samples are built from samples.
// Shows:  a bigger sample made of the samples of its parts, and the Minimal and Maximal shape of a scenario.
// Needs:  BeneficialStrategies.Iso20022.Messages.Samples.
// Prints: the IBAN inside a cash account sample, then a Minimal and a Maximal sample side by side.
using BeneficialStrategies.Iso20022.Choices.AccountIdentification4Choice;
using BeneficialStrategies.Iso20022.Components;
using Samples = BeneficialStrategies.Iso20022.Messages.Samples;

// <snippet part>
ICashAccount40 account = Samples.CashAccount40.Valid.Standard.Minimal;
Console.WriteLine(((IIBAN)account.Identification!).Value);                // DE89370400440532013000: the IBAN sample of example 01
// </snippet>

// <snippet shapes>
var small = Samples.AccountIdentification4Choice.Valid.Other.Minimal;
var large = Samples.AccountIdentification4Choice.Valid.Other.Maximal;
Console.WriteLine($"{small.Identification} scheme={(small.SchemeName is null ? "none" : "set")}");   // ACCT-001 scheme=none
Console.WriteLine($"{large.Identification} scheme={(large.SchemeName is null ? "none" : "set")}");   // ACCT-001 scheme=set
// </snippet>
