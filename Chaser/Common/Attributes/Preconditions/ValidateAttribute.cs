using Discord;
using Discord.Interactions;
using System.Reflection;

namespace Chaser;

/// <summary>
/// Represents an attribute that is used to validate properties of a modal.
/// </summary>
public sealed class ValidateAttribute : ParameterPreconditionAttribute
{
    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, IParameterInfo parameterInfo, object value, IServiceProvider services)
    {
        if (value is not IModal modal || context.Interaction is not IModalInteraction interaction)
            throw new ArgumentException("Attribute cannot be added to this parameter.", nameof(value));

        var tasks = modal.GetType().GetRuntimeProperties()
            .Select(propertyInfo => CheckPropertyRequirementsAsync(context, modal, interaction, propertyInfo, services));

        var results = await Task.WhenAll(tasks);

        return results.FirstOrDefault(result => !result.IsSuccess) ?? PreconditionResult.FromSuccess();
    }

    private static async Task<PreconditionResult> CheckPropertyRequirementsAsync(IInteractionContext context, IModal modal, IModalInteraction interaction, PropertyInfo propertyInfo, IServiceProvider services)
    {
        var customId = propertyInfo.GetCustomAttribute<ModalInputAttribute>(true)?.CustomId;
        var isRequired = propertyInfo.GetCustomAttribute<RequiredInputAttribute>(true)?.IsRequired;
        var component = interaction.Data.Components.FirstOrDefault(c => c.CustomId == customId);

        if (component == null || isRequired == false && string.IsNullOrEmpty(component.Value))
            return PreconditionResult.FromSuccess();

        var tasks = propertyInfo.GetCustomAttributes<PropertyPreconditionAttribute>(true)
            .Select(p => p.CheckRequirementsAsync(context, propertyInfo, modal, component.Value, services));

        var results = await Task.WhenAll(tasks);

        return results.FirstOrDefault(result => !result.IsSuccess) ?? PreconditionResult.FromSuccess();
    }
}
