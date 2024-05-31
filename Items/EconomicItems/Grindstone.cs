namespace GamePrototype.Items.EconomicItems
{
    public sealed class Grindstone : EconomicItem
    {
        public uint DurabilityRestore => 5; //add
        public override bool Stackable => false;

        public Grindstone(string name) : base(name)
        {
        }    
    }
}
