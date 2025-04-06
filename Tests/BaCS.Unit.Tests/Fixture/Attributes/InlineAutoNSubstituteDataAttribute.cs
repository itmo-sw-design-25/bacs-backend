namespace BaCS.Unit.Tests.Fixture.Attributes;

using AutoFixture.Xunit2;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class InlineAutoNSubstituteDataAttribute : CompositeDataAttribute
{
    public InlineAutoNSubstituteDataAttribute(params object[] values)
        : base(new InlineDataAttribute(values), new AutoNSubstituteDataAttribute()) { }
}
