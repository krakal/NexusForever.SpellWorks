namespace NexusForever.SpellWorks.GameTable.Model
{
    public class Creature2Entry
    {
        public uint Id;
        public uint CreationTypeEnum;
        public string Description;
        public uint LocalizedTextIdName;
        public uint Creature2AoiSizeEnum;
        public uint UnitRaceId;
        public uint Creature2DifficultyId;
        public uint Creature2ArcheTypeId;
        public uint Creature2TierId;
        public uint Creature2ModelInfoId;
        public uint Creature2DisplayGroupId;
        public uint Creature2OutfitGroupId;
        public uint PrerequisiteIdVisibility;
        public float ModelScale;
        [GameTableFieldArray(4u)]
        public uint[] Spell4IdActivate;
        [GameTableFieldArray(4u)]
        public uint[] PrerequisiteIdActivateSpells;
        public uint ActivateSpellCastTime;
        public float ActivateSpellMinRange;
        public float ActivateSpellMaxRange;
        public uint LocalizedTextIdActivateSpellText;
        public uint Spell4VisualGroupIdActivateSpell;
        public uint TrainerClassIdMask;
        public uint TradeSkillIdTrainer;
        public uint TradeSkillIdStation;
        [GameTableFieldArray(25u)]
        public uint[] QuestIdGiven;
        [GameTableFieldArray(25u)]
        public uint[] QuestIdReceive;
        public uint QuestAnimStateId;
        public uint PrerequisiteIdAnimState;
        public uint QuestAnimObjectiveIndex;
        public uint Flags;
        public uint UiFlags;
        public uint ActivationFlags;
        public float AimYawConstraint;
        public float AimPitchUpConstraint;
        public float AimPitchDownConstraint;
        public uint Item2IdMTXKey00;
        public uint Item2IdMTXKey01;
        public uint Creature2FamilyId;
        public uint Creature2TractId;
        public uint BindPointId;
        public uint ResourceConversionGroupId;
        public uint TaxiNodeId;
        public uint PathScientistExperimentationId;
        public uint DatacubeId;
        public uint DatacubeVolumeId;
        public uint FactionId;
        public uint MinLevel;
        public uint MaxLevel;
        public uint RescanCooldownTypeEnum;
        public uint RescanCooldown;
        public uint Creature2AffiliationId;
        public uint ItemIdDisplayItemRight;
        public uint SoundEventIdAggro;
        public uint SoundEventIdAware;
        public uint SoundImpactDescriptionIdOrigin;
        public uint SoundImpactDescriptionIdTarget;
        public uint SoundSwitchIdModel;
        public uint SoundCombatLoopId;
        public uint RandomTextLineIdGoodbye00;
        public uint RandomTextLineIdGoodbye01;
        public uint RandomTextLineIdGoodbye02;
        public uint RandomTextLineIdGoodbye03;
        public uint RandomTextLineIdGoodbye04;
        public uint RandomTextLineIdGoodbye05;
        public uint RandomTextLineIdGoodbye06;
        public uint RandomTextLineIdGoodbye07;
        public uint RandomTextLineIdGoodbye08;
        public uint RandomTextLineIdGoodbye09;
        public uint RandomTextLineIdHello00;
        public uint RandomTextLineIdHello01;
        public uint RandomTextLineIdHello02;
        public uint RandomTextLineIdHello03;
        public uint RandomTextLineIdHello04;
        public uint RandomTextLineIdHello05;
        public uint RandomTextLineIdHello06;
        public uint RandomTextLineIdHello07;
        public uint RandomTextLineIdHello08;
        public uint RandomTextLineIdHello09;
        public uint RandomTextLineIdIntro;
        public uint LocalizedTextIdDefaultGreeting;
        public uint RandomTextLineIdReturn00;
        public uint RandomTextLineIdReturn01;
        public uint RandomTextLineIdReturn02;
        public uint RandomTextLineIdReturn03;
        public uint RandomTextLineIdReturn04;
        public uint RandomTextLineIdReturn05;
        public uint RandomTextLineIdReturn06;
        public uint RandomTextLineIdReturn07;
        public uint RandomTextLineIdReturn08;
        public uint RandomTextLineIdReturn09;
        public uint LocalizedTextIdReturnGreeting;
        public uint RandomTextLineIdCompleted;
        public uint LocalizedTextIdCompletedGreeting;
        public uint UnitVoiceTypeId;
        public uint GossipSetId;
        public uint UnitVisualTypeId;
        public uint Spell4VisualGroupIdAttached;
        public uint GenericStringGroupsIdInteractContext;
        public uint Creature2ActionSetId;
        public uint Creature2ActionTextId;
        public uint PathMissionIdSoldier;
        public uint InstancePortalId;
        public uint ModelSequenceIdAnimationPriority00;
        public uint ModelSequenceIdAnimationPriority01;
        public uint ModelSequenceIdAnimationPriority02;
        public uint ModelSequenceIdAnimationPriority03;
        public uint ModelSequenceIdAnimationPriority04;
        public uint PrerequisiteIdPriority00;
        public uint PrerequisiteIdPriority01;
        public uint PrerequisiteIdPriority02;
        public uint PrerequisiteIdPriority03;
        public uint PrerequisiteIdPriority04;
        public float DonutDrawDistance;
        public uint ArchiveArticleIdInteractUnlock;
        public uint TradeskillHarvestingInfoId;
        public uint CcStateImmunitiesFlags;
        public uint Creature2ResistId;
        public uint UnitVehicleId;
        public uint Creature2DisplayInfoIdPortraitOverride;
    }
}