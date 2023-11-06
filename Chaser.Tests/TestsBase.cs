using Bogus;

namespace Chaser.Tests;

/// <summary>
/// The base abstract class for defining unit test cases.
/// </summary>
public abstract class TestsBase
{
    protected Faker Faker { get; } = new();
}
