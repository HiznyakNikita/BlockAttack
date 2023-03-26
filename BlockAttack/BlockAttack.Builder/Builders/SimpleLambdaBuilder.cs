using BlockAttack.Builder.Builders.Code;
using BlockAttack.Builder.Builders.Structure;
using BlockAttack.Builder.Contract.Builders;

namespace BlockAttack.Builder.Builders
{
    internal class SimpleLambdaBuilder : BaseLambdaBuilder, ILambdaBuilder
    {
        public SimpleLambdaBuilder(SimpleLambdaCodeBuilder codeBuilder,
            SimpleLambdaStructureBuilder structureBuilder)
            : base(codeBuilder, structureBuilder)
        {

        }
    }
}