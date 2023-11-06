using Discord;
using Moq;
using Xunit;

namespace Chaser.Tests;

/// <summary>
/// Provides unit test cases for <see cref="GuildExtensions"/>.
/// </summary>
public class GuildExtensionsTests : TestsBase
{
    private static readonly GuildPermissions BanMembersPermissions = new(banMembers: true);

    [Theory]
    [MemberData(nameof(GetTryAddBanAsyncData))]
    public async Task TryAddBanAsync_Has_Expected_Result(IBan? ban, GuildPermissions permissions, bool expected) =>
        await TryModifyBanAsync_Has_Expected_Result(ban, permissions, (guild, user) => guild.TryAddBanAsync(user), expected);

    [Theory]
    [MemberData(nameof(GetTryRemoveBanAsyncData))]
    public async Task TryRemoveBanAsync_Has_Expected_Result(IBan? ban, GuildPermissions permissions, bool expected) =>
        await TryModifyBanAsync_Has_Expected_Result(ban, permissions, (guild, user) => guild.TryRemoveBanAsync(user), expected);

    private static async Task TryModifyBanAsync_Has_Expected_Result(IBan? ban, GuildPermissions permissions, Func<IGuild, IUser, Task<bool>> modify, bool expected)
    {
        var guildMock = new Mock<IGuild>();
        var adminMock = new Mock<IGuildUser>();
        var targetMock = new Mock<IGuildUser>();

        targetMock.Setup(x => x.Hierarchy).Returns(int.MinValue);
        adminMock.Setup(x => x.Hierarchy).Returns(int.MaxValue);
        adminMock.Setup(x => x.GuildPermissions).Returns(permissions);

        var cacheMode = It.IsAny<CacheMode>();
        var options = It.IsAny<RequestOptions>();

        guildMock.Setup(x => x.GetBanAsync(targetMock.Object, options)).Returns(Task.FromResult(ban));
        guildMock.Setup(x => x.GetCurrentUserAsync(cacheMode, options)).Returns(Task.FromResult(adminMock.Object));

        bool result = await modify(guildMock.Object, targetMock.Object).ConfigureAwait(false);

        Assert.Equal(result, expected);
    }

    public static IEnumerable<object?[]> GetTryAddBanAsyncData()
    {
        yield return new object?[] { null, BanMembersPermissions, true };
        yield return new object?[] { null, GuildPermissions.None, false };
        yield return new object?[] { new Mock<IBan>().Object, BanMembersPermissions, false };
        yield return new object?[] { new Mock<IBan>().Object, GuildPermissions.None, false };
    }

    public static IEnumerable<object?[]> GetTryRemoveBanAsyncData()
    {
        var ban = new Mock<IBan>().Object;

        yield return new object?[] { ban, BanMembersPermissions, true };
        yield return new object?[] { ban, GuildPermissions.None, false };
        yield return new object?[] { null, BanMembersPermissions, false };
        yield return new object?[] { null, GuildPermissions.None, false };
    }
}
