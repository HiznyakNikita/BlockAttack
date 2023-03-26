using BlockAttack.Builder.Contract.Models;
using BlockAttack.Builder.Contract.Models.Code;

namespace BlockAttack.Builder.Contract.Builders
{
	public interface ILambdaBuilder
	{
		void Build(LambdaMetadata lambdaMetadata, LambdaCodeMetadata lambdaCodeMetadata);
	}
}
