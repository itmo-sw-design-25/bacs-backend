namespace BaCS.Unit.Tests;

using Domain.Core.Entities;
using FluentAssertions;

public class UnitTest1
{
    [Fact]
    public void DummyTest_Success()
    {
        // Arange
        var user = new User();

        // Assert
        user.Id.Should().NotBeEmpty();
    }
}
