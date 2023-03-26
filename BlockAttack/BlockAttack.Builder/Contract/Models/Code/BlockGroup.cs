namespace BlockAttack.Builder.Contract.Models.Code
{
    public class BlockGroup
    {
        public IReadOnlyDictionary<int, IfBlock> Blocks { get; init; }
        public bool OrderedById { get; init; }
    }
}
