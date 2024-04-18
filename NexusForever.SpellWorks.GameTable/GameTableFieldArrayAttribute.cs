namespace NexusForever.SpellWorks.GameTable
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GameTableFieldArrayAttribute : Attribute
    {
        public uint Length { get; }

        public GameTableFieldArrayAttribute(uint length)
        {
            Length = length;
        }
    }
}
