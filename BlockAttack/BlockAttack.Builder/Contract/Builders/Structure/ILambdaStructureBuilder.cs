using BlockAttack.Builder.Contract.Models;

namespace BlockAttack.Builder.Contract.Builders.Structure
{
	internal interface ILambdaStructureBuilder
	{
		void Build(LambdaMetadata lambdaMetadata, string lambdaCode);
	}
}
