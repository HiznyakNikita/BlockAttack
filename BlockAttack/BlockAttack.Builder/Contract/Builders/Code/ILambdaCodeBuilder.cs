using BlockAttack.Builder.Contract.Models.Code;
using BlockAttack.Builder.Contract.Models;

namespace BlockAttack.Builder.Contract.Builders.Code
{
	internal interface ILambdaCodeBuilder
	{
		string Build(LambdaMetadata lambdaMetadata, LambdaCodeMetadata lambdaCodeMetadata);
	}
}
