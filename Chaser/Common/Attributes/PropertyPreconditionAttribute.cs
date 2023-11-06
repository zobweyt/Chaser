using Discord;
using Discord.Interactions;
using System.Reflection;

namespace Chaser;

/// <summary>
/// Requires the modal properties to pass the specified precondition before execution can begin.
/// </summary>
/// <seealso cref="PreconditionAttribute"/>
/// <seealso cref="ParameterPreconditionAttribute"/>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
public abstract class PropertyPreconditionAttribute : Attribute
{
    /// <summary>
    ///  Checks if the <paramref name="value"/> to be passed meets the precondition requirements.
    /// </summary>
    /// <param name="context">The context of the command.</param>
    /// <param name="propertyInfo">The property being checked.</param>
    /// <param name="obj">The object instance that contains the property.</param>
    /// <param name="value">The value of the property being checked.</param>
    /// <param name="services">The service collection used for dependency injection.</param>
    public abstract Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, PropertyInfo propertyInfo, object obj, string value, IServiceProvider services);
}
