// Copyright 2026 Jeff Ward, Beneficial Strategies. Usage subject to license of enclosing library.

using System.Reflection;

namespace BeneficialStrategies.Iso20022.Samples.Tests;

/// <summary>What every named sample promises, found by reflection so a new sample is checked without a new test.</summary>
public class NamedSamplesTests
{
    private static readonly Assembly SamplesAssembly = typeof(IsoSample).Assembly;
    private static readonly Assembly ModelAssembly = typeof(IGroupHeader104).Assembly;

    private static IEnumerable<(Type Class, PropertyInfo Property)> NamedSamples() =>
        SamplesAssembly.GetTypes()
            .Where(t => t.IsClass && t.IsAbstract && t.IsSealed && t.IsPublic && t.Namespace is not null
                && (t.Namespace.EndsWith(".Valid", StringComparison.Ordinal) || t.Namespace.EndsWith(".Invalid", StringComparison.Ordinal)))
            .SelectMany(t => t.GetProperties(BindingFlags.Public | BindingFlags.Static).Select(p => (Class: t, Property: p)));

    [Fact]
    public void ThereAreNamedSamples()
    {
        Assert.NotEmpty(NamedSamples());
    }

    [Fact]
    public void EverySampleIsAnObjectOfAnInterfaceOfTheModel()
    {
        var wrong = NamedSamples()
            .Where(s => !s.Property.PropertyType.IsInterface || Iso.KindOf(s.Property.PropertyType) is not (IsoKind.Message or IsoKind.Component or IsoKind.Choice or IsoKind.ChoiceVariant))
            .Select(s => $"{s.Class.FullName}.{s.Property.Name}: {s.Property.PropertyType.Name}")
            .ToList();

        Assert.True(wrong.Count == 0, "Not an interface of the model:\n" + string.Join("\n", wrong));
    }

    [Fact]
    public void EverySampleIsFoundUnderTheNameOfItsType()
    {
        // Samples.<Type>.Valid.<Scenario>: <Type> is the interface the sample is of, or the choice the sample's variant belongs to.
        var wrong = NamedSamples()
            .Where(s =>
            {
                var type = s.Class.Namespace!.Split('.')[^2];
                var model = ModelAssembly.GetTypes().Where(t => t.IsInterface && t.Name.TrimStart('I') == type).ToList();
                return model.Count == 0;
            })
            .Select(s => s.Class.FullName)
            .Distinct()
            .ToList();

        Assert.True(wrong.Count == 0, "Named after no type of the model:\n" + string.Join("\n", wrong));
    }

    [Fact]
    public void EveryAccessMakesANewObject_EqualByValueToTheOthers()
    {
        var wrong = new List<string>();
        foreach (var (cls, property) in NamedSamples())
        {
            var first = property.GetValue(null);
            var second = property.GetValue(null);
            if (first is null || second is null)
                wrong.Add($"{cls.FullName}.{property.Name} is null");
            else if (ReferenceEquals(first, second))
                wrong.Add($"{cls.FullName}.{property.Name} is the same object on every access");
            else if (!first.Equals(second))
                wrong.Add($"{cls.FullName}.{property.Name} is not equal to itself");
        }

        Assert.True(wrong.Count == 0, string.Join("\n", wrong));
    }
}
