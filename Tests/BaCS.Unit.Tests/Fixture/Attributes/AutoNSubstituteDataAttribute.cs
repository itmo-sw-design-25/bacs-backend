namespace BaCS.Unit.Tests.Fixture.Attributes;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Customization;

[AttributeUsage(AttributeTargets.Method)]
public class AutoNSubstituteDataAttribute() : AutoDataAttribute(
    () => new Fixture().Customize(new AutoNSubstituteCustomization()).Customize(new MapsterCustomization())
);
