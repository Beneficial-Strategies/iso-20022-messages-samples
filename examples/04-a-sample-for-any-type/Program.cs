// Example 04: a sample for any type.
// Shows:  IsoSample.Minimal<T>(), a valid object made from the interface itself, and that a named sample wins where there is one.
// Needs:  BeneficialStrategies.Iso20022.Messages.Samples.
// Prints: a generated component, and a choice for which a named sample exists.
using BeneficialStrategies.Iso20022.Choices;
using BeneficialStrategies.Iso20022.Components;
using Samples = BeneficialStrategies.Iso20022.Messages.Samples;

// <snippet any>
IGenericIdentification3 id = Samples.IsoSample.Minimal<IGenericIdentification3>();
Console.WriteLine(id);                                          // IGenericIdentification3_1 { Identification = A, Issuer = null }

var account = Samples.IsoSample.Minimal<IAccountIdentification4Choice>();
Console.WriteLine(account);                                     // IIBAN_2 { Value = DE89370400440532013000 }: a named sample wins, wherever the type appears
// </snippet>
