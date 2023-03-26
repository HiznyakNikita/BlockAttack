using BlockAttack.Publisher.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace BlockAttack.Publisher
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddLambdaPublishers(this IServiceCollection services)
		{
			return services.AddScoped<ILambdaPublisher, SimpleLambdaPublisher>();
		}
	}
}
