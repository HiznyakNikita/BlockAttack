using BlockAttack.Builder.Builders;
using BlockAttack.Builder.Builders.Code;
using BlockAttack.Builder.Builders.Structure;
using BlockAttack.Builder.Contract.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace BlockAttack.Builder
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddLambdaBuilders(this IServiceCollection services)
		{
			return services
				.AddScoped<SimpleLambdaCodeBuilder>()
				.AddScoped<SimpleLambdaStructureBuilder>()
				.AddScoped<ILambdaBuilder, SimpleLambdaBuilder>();
		}
	}
}
