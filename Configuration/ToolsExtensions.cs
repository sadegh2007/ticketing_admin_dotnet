using System.Globalization;
using ERP.Shared.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

namespace ERP.Ticketing.HttpApi.Configuration;

public static class ToolsExtensions
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddValidatorsFromAssemblyContaining<OtpRequestValidator>()
            .AddFluentValidationRulesToSwagger();

        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("fa");
        
        return services;
    }
}