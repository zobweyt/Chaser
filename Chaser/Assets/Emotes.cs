using Discord;

namespace Chaser;

/// <summary>
/// Represents a constant set of predefined <see cref="Emote"/> symbols.
/// </summary>
public static class Emotes
{
    /// <summary>
    /// The emote used to indicate a positive state.
    /// </summary>
    public static readonly Emote Check = "<:check:1124759309033689229>";

    /// <summary>
    /// The emote used to indicate a negative state.
    /// </summary>
    public static readonly Emote Cross = "<:сross:1124759305879552030>";

    /// <summary>
    /// The emote used in conjunction with edit context.
    /// </summary>
    public static readonly Emote Wrench = "<:wrench:1137146063871017102>";
    
    /// <summary>
    /// The emote used in conjunction with info context.
    /// </summary>
    public static readonly Emote File = "<:file:1139313722477785212>";
    
    /// <summary>
    /// The emote used in conjunction with date context.
    /// </summary>
    public static readonly Emote Clock = "<:clock:1139272579648073838>";

    /// <summary>
    /// The emote used in conjunction with character context.
    /// </summary>
    public static readonly Emote Character = "<:character:1137348557167407175>";

    /// <summary>
    /// Represents a collection of predefined navigation symbols.
    /// </summary>
    public static class Navigation
    {
        /// <summary>
        /// The emote used for navigating anywhere.
        /// </summary>
        public static readonly Emote Jump = "<:jump:1109408955484078141>";

        /// <summary>
        /// The emote used for navigating forward.
        /// </summary>
        public static readonly Emote Forward = "<:forward:1109210001223974972>";

        /// <summary>
        /// The emote used for navigating backward.
        /// </summary>
        public static readonly Emote Backward = "<:backward:1109210084942295141>";
    }

    /// <summary>
    /// Represents a collection of brand logos.
    /// </summary>
    public static class Logos
    {
        /// <summary>
        /// The official TopGG logo (https://blog.top.gg/logo-redesign).
        /// </summary>
        public static readonly Emote TopGG = "<:topgg:1131243068352368821>";

        /// <summary>
        /// The official GitHub invertocat logo (https://github.com/logos).
        /// </summary>
        public static readonly Emote GitHub = "<:github:1131243046395183176>";

        /// <summary>
        /// The official Discord mark logo (https://discord.com/branding).
        /// </summary>
        public static readonly Emote Discord = "<:discord:1131243248409641000>";
    }
}
