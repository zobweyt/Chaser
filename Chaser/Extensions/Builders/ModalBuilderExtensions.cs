using Discord;

namespace Chaser;

/// <summary>
/// Provides extension methods for <see cref="ModalBuilder"/>.
/// </summary>
public static class ModalBuilderExtensions
{
    /// <summary>
    /// Updates the value of a <see cref="TextInputComponent"/> in the <see href="modalBuilder"/> by the 
    /// specified <paramref name="customId">.
    /// </summary>
    /// <param name="modalBuilder">The current builder.</param>
    /// <param name="customId">The <see cref="TextInputComponent.CustomId"/> of the input to update.</param>
    /// <param name="action">An action that configures the updated text input.</param>
    /// <returns>The current builder instance with the updated text input.</returns>
    public static ModalBuilder UpdateTextInput(this ModalBuilder modalBuilder, string customId, string? value)
    {
        var row = modalBuilder.Components.ActionRows.First(r => r.Components.Any(c => c.CustomId == customId));
        var component = row.Components.OfType<TextInputComponent>().First(c => c.CustomId == customId);

        var builder = new TextInputBuilder()
        {
            CustomId = customId,
            Label = component.Label,
            MaxLength = component.MaxLength,
            MinLength = component.MinLength,
            Placeholder = component.Placeholder,
            Required = component.Required,
            Style = component.Style,
            Value = value
        };

        row.Components.RemoveAll(c => c.CustomId == customId);
        row.AddComponent(builder.Build());

        return modalBuilder;
    }

     /// <summary>
    /// Removes a component from the <see cref="ModalBuilder"/> by the specified <paramref name="customId">.
    /// </summary>
    /// <param name="modalBuilder">The current builder.</param>
    /// <param name="customId">The <see cref="Component.CustomId"/> of the component to remove.</param>
    /// <returns>The current builder instance with the component removed.</returns>
    public static ModalBuilder RemoveComponent(this ModalBuilder modalBuilder, string customId)
    {
        modalBuilder.Components.ActionRows.RemoveAll(r => r.Components.Any(c => c.CustomId == customId));

        return modalBuilder;
    }
}
