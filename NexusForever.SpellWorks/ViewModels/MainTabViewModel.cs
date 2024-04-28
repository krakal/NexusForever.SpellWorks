namespace NexusForever.SpellWorks.ViewModels
{
    public partial class MainTabViewModel : BaseTabItem
    {
        public override string Header => "Main";

        public SpellInfoSearchViewModel SpellInfoSearchControl { get; }
        public SpellInfoViewModel SpellInfoControlLeft { get; }
        public SpellInfoViewModel SpellInfoControlRight { get; }

        #region Dependency Injection

        public MainTabViewModel(
            SpellInfoSearchViewModel spellInfoSearchControl,
            SpellInfoViewModel spellInfoControlLeft,
            SpellInfoViewModel spellInfoControlRight)
        {
            SpellInfoSearchControl = spellInfoSearchControl;
            SpellInfoControlLeft   = spellInfoControlLeft;
            SpellInfoControlRight  = spellInfoControlRight;
        }

        #endregion

        public MainTabViewModel()
        {
        }
    }
}