using BlockAttack.Builder.Contract.Builders;
using BlockAttack.Builder.Contract.Builders.Code;
using BlockAttack.Builder.Contract.Builders.Structure;
using BlockAttack.Builder.Contract.Models;
using BlockAttack.Builder.Contract.Models.Code;

namespace BlockAttack.Builder.Builders
{
	internal abstract class BaseLambdaBuilder : ILambdaBuilder
	{
		private readonly ILambdaCodeBuilder _codeBuilder;
		private readonly ILambdaStructureBuilder _structureBuilder;

		public BaseLambdaBuilder(ILambdaCodeBuilder codeBuilder,
			ILambdaStructureBuilder structureBuilder)
		{
			_codeBuilder = codeBuilder;
			_structureBuilder = structureBuilder;
		}

		public virtual void Build(LambdaMetadata metadata, LambdaCodeMetadata codeMetadata)
		{
			var code = _codeBuilder.Build(metadata, codeMetadata);
			_structureBuilder.Build(metadata, code);
		}
	}
}
