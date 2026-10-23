// Example 03: vary a sample.
// Shows:  a copy of a sample with the one member a test is about changed; the sample itself is untouched.
// Needs:  BeneficialStrategies.Iso20022.Messages.Samples (which brings the Factory).
// Prints: the identification of the copy and of the sample.
using BeneficialStrategies.Iso20022.Factory;
using Samples = BeneficialStrategies.Iso20022.Messages.Samples;

// <snippet with>
var changed = IsoDataFactory.With(Samples.AccountIdentification4Choice.Valid.Other.Minimal, b => b
    .Set(x => x.Identification).To("OTHER-2"));

Console.WriteLine(changed.Identification);                                                  // OTHER-2
Console.WriteLine(Samples.AccountIdentification4Choice.Valid.Other.Minimal.Identification);  // ACCT-001
// </snippet>
