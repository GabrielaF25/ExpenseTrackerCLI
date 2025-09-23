using ExpenseTrackerApi.Authentification;
using ExpenseTrackerApi.Profiles;
using ExpenseTrackerApi.Services;

namespace ExpenseTrackerApi.Extensions;

public static  class ServiceCollectionApi
{
    public static IServiceCollection ServceCollectionApi(this IServiceCollection services)
    {
        services.AddScoped<IExpenseServiceApi, ExpenseServiceApi>();  
        
        services.AddAutoMapper(typeof(ExpenseProfile).Assembly);

        return services;
    }
}
