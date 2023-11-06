using Bogus;
using Discord;
using Moq;
using Xunit;

namespace Chaser.Tests;

/// <summary>
/// Provides unit test cases for <see cref="EmbedBuilderExtensions"/>.
/// </summary>
public class EmbedBuilderExtensionsTests : TestsBase
{
    [Fact]
    public void WithStyle_Should_Apply_Style_To_EmbedBuilder()
    {
        var embedBuilder = new EmbedBuilder();
        var embedStyleMock = new Mock<EmbedStyle>();

        embedStyleMock.Setup(x => x.Name).Returns(Faker.Lorem.Word());
        embedStyleMock.Setup(x => x.IconUrl).Returns(Faker.Internet.Avatar());
        embedStyleMock.Setup(x => x.Footer).Returns(Faker.Lorem.Sentence());
        embedStyleMock.Setup(x => x.Color).Returns(Faker.Random.RawColor());

        embedBuilder.WithStyle(embedStyleMock.Object);

        Assert.Equal(embedStyleMock.Object.Name, embedBuilder.Author.Name);
        Assert.Equal(embedStyleMock.Object.IconUrl, embedBuilder.Author.IconUrl);
        Assert.Equal(embedStyleMock.Object.Footer, embedBuilder.Footer.Text);
        Assert.Equal(embedStyleMock.Object.Color, embedBuilder.Color);
    }
}
