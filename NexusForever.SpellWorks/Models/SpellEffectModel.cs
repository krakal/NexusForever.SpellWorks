using Microsoft.Extensions.DependencyInjection;
using NexusForever.SpellWorks.GameTable.Model;
using NexusForever.SpellWorks.GameTable.Static;
using NexusForever.SpellWorks.Models.Effect;

namespace NexusForever.SpellWorks.Models
{
    public class SpellEffectModel : ISpellEffectModel
    {
        public SpellEffectType Type => _entry.EffectType;
        public uint DamageType => _entry.DamageType;
        public uint DelayTime => _entry.DelayTime;
        public uint TickTime => _entry.TickTime;
        public uint DurationTime => _entry.DurationTime;
        public uint Flags => _entry.Flags;

        public ISpellEffectColumnData ColumnData { get; private set; }
        public List<ISpellEffectRowData> RowData { get; } = [];

        private Spell4EffectsEntry _entry;

        #region Dependency Injection

        private readonly IServiceProvider _serviceProvider;

        public SpellEffectModel(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        public void Initialise(Spell4EffectsEntry entry)
        {
            _entry = entry;

            ColumnData = _serviceProvider.GetKeyedService<ISpellEffectColumnData>(Type);
            ColumnData ??= new DefaultSpellEffectColumnData();

            var rowData = _serviceProvider.GetKeyedService<ISpellEffectRowData>(Type);
            rowData ??= new DefaultSpellEffectRowData();
            rowData.Entry = _entry;
            RowData.Add(rowData);
        }
    }
}
