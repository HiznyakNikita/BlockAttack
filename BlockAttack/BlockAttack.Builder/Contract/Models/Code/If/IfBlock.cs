namespace BlockAttack.Builder.Contract.Models.Code
{
    public class IfBlock : IBlock
    {
        public IfBlockCondition Condition { get; init; }
        public IfBlockHandler Handler { get; init; }
        public IfBlock(IfBlockCondition condition, IfBlockHandler handler)
        {
            Condition = condition;
            Handler = handler;
        }
    }
}
