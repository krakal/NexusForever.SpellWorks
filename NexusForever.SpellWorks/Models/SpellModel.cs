using NexusForever.SpellWorks.GameTable.Model;
using NexusForever.SpellWorks.Services;

namespace NexusForever.SpellWorks.Models
{
    public class SpellModel : ISpellModel
    {
        public Spell4Entry Entry { get; private set; }
        public uint Id => Entry.Id;
        public string Description => Entry.Description;

        public ISpellBaseModel SpellBaseModel { get; private set; }
        public List<ISpellEffectModel> Effects { get; private set; }

        private readonly ISpellModelService _spellModelService;
        private readonly IGameTableService _gameTableService;

        public SpellModel(
            ISpellModelService spellModelService,
            IGameTableService gameTableService)
        {
            _spellModelService = spellModelService;
            _gameTableService  = gameTableService;
        }

        public void Initialise(Spell4Entry entry)
        {
            Entry = entry;
            SpellBaseModel = _spellModelService.SpellBaseModels[entry.Spell4BaseIdBaseSpell];

            _spellModelService.SpellEffectModels.TryGetValue(entry.Id, out List<ISpellEffectModel> effects);
            Effects = effects;
        }
    }
}
