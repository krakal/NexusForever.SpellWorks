﻿using NexusForever.SpellWorks.GameTable.Model;

namespace NexusForever.SpellWorks.Models
{
    public interface ISpellModel
    {
        Spell4Entry Entry { get; }
        uint Id { get; }
        string Description { get; }
        string ActionBarTooltip { get; }
        string FilledActionBarTooltip { get; }

        ISpellBaseModel SpellBaseModel { get; }
        List<ISpellEffectModel> Effects { get; }
        List<ISpellProcModel> Procs { get; }
        List<uint> ProcReferences { get; }

        void Initialise(Spell4Entry entry);
    }
}