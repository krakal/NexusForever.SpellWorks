﻿using System.IO;
using MahApps.Metro.Controls.Dialogs;
using Nexus.Archive;
using NexusForever.SpellWorks.GameTable;
using NexusForever.SpellWorks.GameTable.Model;

namespace NexusForever.SpellWorks.Services
{
    public class GameTableService : IGameTableService
    {
        public GameTable<CCStatesEntry> CCStates { get; private set; }
        public GameTable<CCStateAdditionalDataEntry> CCStateAdditionalData { get; private set; }
        public GameTable<CCStateDiminishingReturnsEntry> CCStateDiminishingReturns { get; private set; }
        public GameTable<Spell4Entry> Spell4 { get; private set; }
        public GameTable<Spell4AoeTargetConstraintsEntry> Spell4AoeTargetConstraints { get; private set; }
        public GameTable<Spell4BaseEntry> Spell4Base { get; private set; }
        public GameTable<Spell4CCConditionsEntry> Spell4CCConditions { get; private set; }
        public GameTable<Spell4CastResultEntry> Spell4CastResult { get; private set; }
        public GameTable<Spell4ClientMissileEntry> Spell4ClientMissile { get; private set; }
        public GameTable<Spell4ConditionsEntry> Spell4Conditions { get; private set; }
        public GameTable<Spell4EffectGroupListEntry> Spell4EffectGroupList { get; private set; }
        public GameTable<Spell4EffectModificationEntry> Spell4EffectModification { get; private set; }
        public GameTable<Spell4EffectsEntry> Spell4Effects { get; private set; }
        public GameTable<Spell4GroupListEntry> Spell4GroupList { get; private set; }
        public GameTable<Spell4HitResultsEntry> Spell4HitResults { get; private set; }
        public GameTable<Spell4ModificationEntry> Spell4Modification { get; private set; }
        public GameTable<Spell4PrerequisitesEntry> Spell4Prerequisites { get; private set; }
        public GameTable<Spell4ReagentEntry> Spell4Reagent { get; private set; }
        public GameTable<Spell4RunnerEntry> Spell4Runner { get; private set; }
        public GameTable<Spell4ServiceTokenCostEntry> Spell4ServiceTokenCost { get; private set; }
        public GameTable<Spell4SpellTypesEntry> Spell4SpellTypes { get; private set; }
        public GameTable<Spell4StackGroupEntry> Spell4StackGroup { get; private set; }
        public GameTable<Spell4TagEntry> Spell4Tag { get; private set; }
        public GameTable<Spell4TargetAngleEntry> Spell4TargetAngle { get; private set; }
        public GameTable<Spell4TargetMechanicsEntry> Spell4TargetMechanics { get; private set; }
        public GameTable<Spell4TelegraphEntry> Spell4Telegraph { get; private set; }
        public GameTable<Spell4ThresholdsEntry> Spell4Thresholds { get; private set; }
        public GameTable<Spell4TierRequirementsEntry> Spell4TierRequirements { get; private set; }
        public GameTable<Spell4ValidTargetsEntry> Spell4ValidTargets { get; private set; }
        public GameTable<Spell4VisualEntry> Spell4Visual { get; private set; }
        public GameTable<Spell4VisualGroupEntry> Spell4VisualGroup { get; private set; }
        public GameTable<SpellCoolDownEntry> SpellCoolDown { get; private set; }
        public GameTable<SpellEffectTypeEntry> SpellEffectType { get; private set; }
        public GameTable<SpellLevelEntry> SpellLevel { get; private set; }
        public GameTable<SpellPhaseEntry> SpellPhase { get; private set; }


        #region Dependency Injection

        private readonly IArchiveService _archiveService;

        public GameTableService(
            IArchiveService archiveService)
        {
            _archiveService = archiveService;
        }

        #endregion

        private int count;

        public async Task Initialise(ProgressDialogController controller)
        {
            controller.SetMessage("Loading Game Tables...");
            controller.Minimum = 0;
            controller.Maximum = 35;

            CCStates = await LoadGameTable<CCStatesEntry>(controller, "CCStates.tbl");
            CCStateAdditionalData = await LoadGameTable<CCStateAdditionalDataEntry>(controller, "CCStateAdditionalData.tbl");
            CCStateDiminishingReturns = await LoadGameTable<CCStateDiminishingReturnsEntry>(controller, "CCStateDiminishingReturns.tbl");
            Spell4 = await LoadGameTable<Spell4Entry>(controller, "Spell4.tbl");
            Spell4AoeTargetConstraints = await LoadGameTable<Spell4AoeTargetConstraintsEntry>(controller, "Spell4AoeTargetConstraints.tbl");
            Spell4Base = await LoadGameTable<Spell4BaseEntry>(controller, "Spell4Base.tbl");
            Spell4CastResult = await LoadGameTable<Spell4CastResultEntry>(controller, "Spell4CastResult.tbl");
            Spell4CCConditions = await LoadGameTable<Spell4CCConditionsEntry>(controller, "Spell4CCConditions.tbl");
            Spell4ClientMissile = await LoadGameTable<Spell4ClientMissileEntry>(controller, "Spell4ClientMissile.tbl");
            Spell4Conditions = await LoadGameTable<Spell4ConditionsEntry>(controller, "Spell4Conditions.tbl");
            Spell4EffectGroupList     = await LoadGameTable<Spell4EffectGroupListEntry>(controller, "Spell4EffectGroupList.tbl");
            Spell4EffectModification  = await LoadGameTable<Spell4EffectModificationEntry>(controller, "Spell4EffectModification.tbl");
            Spell4Effects             = await LoadGameTable<Spell4EffectsEntry>(controller, "Spell4Effects.tbl");
            Spell4GroupList           = await LoadGameTable<Spell4GroupListEntry>(controller, "Spell4GroupList.tbl");
            Spell4HitResults          = await LoadGameTable<Spell4HitResultsEntry>(controller, "Spell4HitResults.tbl");
            Spell4Modification        = await LoadGameTable<Spell4ModificationEntry>(controller, "Spell4Modification.tbl");
            Spell4Prerequisites       = await LoadGameTable<Spell4PrerequisitesEntry>(controller, "Spell4Prerequisites.tbl");
            Spell4Reagent             = await LoadGameTable<Spell4ReagentEntry>(controller, "Spell4Reagent.tbl");
            Spell4Runner              = await LoadGameTable<Spell4RunnerEntry>(controller, "Spell4Runner.tbl");
            Spell4ServiceTokenCost    = await LoadGameTable<Spell4ServiceTokenCostEntry>(controller, "Spell4ServiceTokenCost.tbl");
            Spell4SpellTypes          = await LoadGameTable<Spell4SpellTypesEntry>(controller, "Spell4SpellTypes.tbl");
            Spell4StackGroup          = await LoadGameTable<Spell4StackGroupEntry>(controller, "Spell4StackGroup.tbl");
            Spell4Tag                 = await LoadGameTable<Spell4TagEntry>(controller, "Spell4Tag.tbl");
            Spell4TargetAngle         = await LoadGameTable<Spell4TargetAngleEntry>(controller, "Spell4TargetAngle.tbl");
            Spell4TargetMechanics     = await LoadGameTable<Spell4TargetMechanicsEntry>(controller, "Spell4TargetMechanics.tbl");
            Spell4Telegraph           = await LoadGameTable<Spell4TelegraphEntry>(controller, "Spell4Telegraph.tbl");
            Spell4Thresholds          = await LoadGameTable<Spell4ThresholdsEntry>(controller, "Spell4Thresholds.tbl");
            Spell4TierRequirements    = await LoadGameTable<Spell4TierRequirementsEntry>(controller, "Spell4TierRequirements.tbl");
            Spell4ValidTargets        = await LoadGameTable<Spell4ValidTargetsEntry>(controller, "Spell4ValidTargets.tbl");
            Spell4Visual              = await LoadGameTable<Spell4VisualEntry>(controller, "Spell4Visual.tbl");
            Spell4VisualGroup         = await LoadGameTable<Spell4VisualGroupEntry>(controller, "Spell4VisualGroup.tbl");
            SpellCoolDown             = await LoadGameTable<SpellCoolDownEntry>(controller, "SpellCoolDown.tbl");
            SpellEffectType           = await LoadGameTable<SpellEffectTypeEntry>(controller, "SpellEffectType.tbl");
            SpellLevel                = await LoadGameTable<SpellLevelEntry>(controller, "SpellLevel.tbl");
            SpellPhase                = await LoadGameTable<SpellPhaseEntry>(controller, "SpellPhase.tbl");

        }

        private Task<GameTable<T>> LoadGameTable<T>(ProgressDialogController controller, string name) where T : class, new()
        {
            return Task.Run(() =>
            {
                string filePath = Path.Combine("DB", name);
                if (_archiveService.MainArchive.IndexFile.FindEntry(filePath) is not IArchiveFileEntry file)
                    throw new FileNotFoundException();

                using Stream archiveStream = _archiveService.MainArchive.OpenFileStream(file);
                using var memoryStream = new MemoryStream();
                archiveStream.CopyTo(memoryStream);
                memoryStream.Position = 0;

                var gameTable = new GameTable<T>(memoryStream);
                Interlocked.Increment(ref count);
                controller.SetProgress(count);

                return gameTable;
            });
        }
    }
}
