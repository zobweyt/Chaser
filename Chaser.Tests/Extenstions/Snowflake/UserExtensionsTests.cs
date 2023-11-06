using Bogus;
using Discord;
using Moq;
using Xunit;

namespace Chaser.Tests;

/// <summary>
/// Provides unit test cases for <see cref="UserExtensions"/>.
/// </summary>
public class UserExtensionsTests : TestsBase
{
    [Fact]
    public void GetDisplayAvatarUrl_Should_Return_GetDefaultAvatarUrl_When_GetAvatarUrl_Is_Null()
    {
        var userMock = new Mock<IUser>();
        string defaultAvatarUrl = Faker.Internet.Avatar();

        userMock.Setup(x => x.GetAvatarUrl(It.IsAny<ImageFormat>(), It.IsAny<ushort>())).Returns((string)null);
        userMock.Setup(x => x.GetDefaultAvatarUrl()).Returns(defaultAvatarUrl);

        string actualAvatarUrl = userMock.Object.GetDisplayAvatarUrl();

        Assert.Equal(actualAvatarUrl, defaultAvatarUrl);
    }

    [Theory]
    [MemberData(nameof(HasHigherHierarchyData))]
    public void HasHigherHierarchy_Has_Expected_Values(int adminHierarchy, int targetHierarchy, bool expected)
    {
        var adminMock = new Mock<IGuildUser>();
        var targetMock = new Mock<IGuildUser>();

        adminMock.Setup(x => x.Hierarchy).Returns(adminHierarchy);
        targetMock.Setup(x => x.Hierarchy).Returns(targetHierarchy);

        bool result = adminMock.Object.HasHigherHierarchy(targetMock.Object);

        Assert.Equal(result, expected);
    }

    public static IEnumerable<object?[]> HasHigherHierarchyData()
    {
        yield return new object?[] { 20, 15, true };
        yield return new object?[] { 10, 15, false };
        yield return new object?[] { 15, 15, false };
    }
}
