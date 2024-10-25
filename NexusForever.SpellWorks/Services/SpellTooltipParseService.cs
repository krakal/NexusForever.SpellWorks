using NexusForever.SpellWorks.GameTable.Model;
using NexusForever.SpellWorks.GameTable.Static;
using NexusForever.SpellWorks.Models;
using NexusForever.SpellWorks.Services;
using SharpCompress.Common;
using System.Configuration;
using System.Diagnostics;


namespace NexusForever.SpellWorks.Services
{
    public class SpellTooltipParseService : ISpellTooltipParseService
    {
        private readonly ITextTableService _textTableService;
        private readonly IGameTableService _gameTableService;
        private readonly ISpellModelService _spellModelService;

        private IUnitService _casterUnit;

        public SpellTooltipParseService(
            ITextTableService textTableService, 
            IGameTableService gameTableService,
            ISpellModelService spellModelService,
            IUnitService casterUnitService)
        {
            _textTableService = textTableService;
            _gameTableService = gameTableService;
            _spellModelService = spellModelService;
            _casterUnit = casterUnitService;

            _casterUnit.Level = 50;
            _casterUnit.AssaultPower = 230;
            _casterUnit.SupportPower = 230;
        }

        public string GetRawTooltip(ISpellModel spell)
        {
            string text = _textTableService.GetText(spell.Entry.LocalizedTextIdActionBarTooltip);
            
            return text;
        }

        public string Parse(ISpellModel spell)
        {
            string text = _textTableService.GetText(spell.Entry.LocalizedTextIdActionBarTooltip);

            var tokens = GetReplaceableTextTokens(text, spell.Id);

            foreach (var token in tokens.Reverse<ReplaceableTextToken>())
            {
                if(token.replacementText != null)
                {
                    text = text.Replace(token.text, token.replacementText);
                }
                
                //text = text + ($"\nstart:{token.start}, end:{token.end} type:{token.type}, id:{token.id}, modifierId:{token.modifierIndex}, modifier:{token.modifier}");
                //Debug.WriteLine($"start:{token.start}, end:{token.end} type:{token.type}, id:{token.id}, modifierId:{token.modifierIndex}, modifier:{token.modifier}");
            }

            return text;
        }

        struct ReplaceableTextToken
        {
            public int start;
            public int end;
            public string type;
            public int id;
            public string modifier;
            public int modifierIndex;
            public string text;
            public string replacementText;
        }

        List<ReplaceableTextToken> GetReplaceableTextTokens(string text, uint spellId)
        {
            List<ReplaceableTextToken> tokens = new List<ReplaceableTextToken>();

            tokens = FindNextToken(tokens, text, spellId);

            return tokens;
        }

        List<ReplaceableTextToken> FindNextToken(List<ReplaceableTextToken> tokens, string text, uint spellId)
        {
            if (text == "" || text == null)
                return tokens;

            int tokenStartIndex = text.IndexOf('$');
            if (text[tokenStartIndex + 1] == '(')
            {
                int tokenEndIndex = text.IndexOf(')');
                ReplaceableTextToken token = new ReplaceableTextToken();
                token.start = tokenStartIndex;
                token.end = tokenEndIndex;

                token.text = text.Substring(token.start, tokenEndIndex - tokenStartIndex + 1);

                token = ProcessToken(token, text.Substring(tokenStartIndex,tokenEndIndex - tokenStartIndex + 1), spellId);
                tokens.Add(token);
                FindNextToken(tokens, text.Substring(tokenEndIndex + 1), spellId);   
            }
            return tokens;
        }

        ReplaceableTextToken ProcessToken(ReplaceableTextToken token, string text, uint spellId)
        {
            Debug.WriteLine(text);
            int dotPosition = text.IndexOf('.');
            int equalSignPosition = text.IndexOf('=');
            int closingParenPosition = text.IndexOf(')');

            if (dotPosition != -1 && equalSignPosition != -1 && closingParenPosition != -1)
            {
                token.type = text.Substring(2, equalSignPosition - 2);
                Int32.TryParse( text.Substring(equalSignPosition + 1, dotPosition - equalSignPosition - 1), out token.id);

                string secondPart = text.Substring(dotPosition + 1, closingParenPosition - dotPosition - 1);

                if (token.type == "eff")
                {
                    token = ProcessEffect(token, secondPart);
                }
                else if (token.type == "spell")
                {
                    if (dotPosition >= closingParenPosition)
                    {
                         return token;
                    }
                    token = ProcessSpell(token, secondPart);

                } else if (token.type == "cc")
                {
                    token = ProcessCC(token, secondPart, spellId);
                }
            }

            return token;
        }

        public uint[] StatLocalisedStringId = { 134658, 66, 166022, 166023, 184460, 166024, 184461, 184462, 209246,
            209247, 209248, 221023, 313798, 313799, 318506, 318573, 681730, 681732, 694706, 746765, 318573,
            746766, 746767, 746769, 318506, 746770, 746771 };

        float CumulativeScaledEffectParameter(Spell4EffectsEntry Spell4Effect, int ParameterIndex)
        {
            //TODO: Iterate over buffs to get total scaled value
            return ScaledEffectParameter(Spell4Effect, ParameterIndex);
        }

        float ScaledEffectParameter(Spell4EffectsEntry SpellEffect, int ParameterIndex)
        {
            float scaledUnitVital = 0;
            float scaledSecondaryStat = 0;

            scaledUnitVital = ScaledUnitVitalParameters(SpellEffect);
            scaledSecondaryStat = ScaledUnitSecondaryParameters(SpellEffect);

            uint parameterType = 0;
            float totalParameterValue = 0;
            uint relevantParameterCount = 0;

            for (uint i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        parameterType = SpellEffect.ParameterType00;
                        totalParameterValue = totalParameterValue + SpellEffect.ParameterValue00;
                        break;
                    case 1:
                        parameterType = SpellEffect.ParameterType01;
                        totalParameterValue = totalParameterValue + SpellEffect.ParameterValue01;
                        break;

                    case 2:
                        parameterType = SpellEffect.ParameterType02;
                        totalParameterValue = totalParameterValue + SpellEffect.ParameterValue02;
                        break;

                    case 3:
                        parameterType = SpellEffect.ParameterType03;
                        totalParameterValue = totalParameterValue + SpellEffect.ParameterValue03;
                        break;
                }

                if (parameterType > 0 && (SpellEffectParameterType)parameterType <= SpellEffectParameterType.SupportPower_SpellEffectParameterType)
                {
                    relevantParameterCount++;
                }
            }

            uint dataBitsValue1 = 0, dataBitsValue2 = 0;

            switch (SpellEffect.EffectType)
            {
                case SpellEffectType.Transference:
                    dataBitsValue1 = SpellEffect.DataBits02;
                    dataBitsValue2 = SpellEffect.DataBits03;
                    break;
                case SpellEffectType.Damage:
                case SpellEffectType.Heal:
                case SpellEffectType.DistanceDependentDamage:
                case SpellEffectType.DistributedDamage:
                case SpellEffectType.HealShields:
                case SpellEffectType.DamageShields:
                    dataBitsValue1 = SpellEffect.DataBits00;
                    dataBitsValue2 = SpellEffect.DataBits01;
                    break;
                default:
                    break;
            }

            float scaledValue = (BitConverter.ToSingle(BitConverter.GetBytes(dataBitsValue2), 0) + scaledUnitVital) * BitConverter.ToSingle(BitConverter.GetBytes(dataBitsValue1), 0);
            
            if (_casterUnit != null)
            {
                //AND casterUnit.Type != Player && casterUnit.Type != Ghost
                //Implement extra scaling here which is presumably for creatures
            }

            scaledValue = scaledSecondaryStat + scaledValue;

            float effectTypeScaling = 1.0f;
            float unitPropertyScaling = 0f;

            if(SpellEffect.EffectType == SpellEffectType.Damage 
                || SpellEffect.EffectType == SpellEffectType.Transference 
                || SpellEffect.EffectType == SpellEffectType.DamageShields) 
            {
                effectTypeScaling = 0.0f;
            }

            if(_casterUnit !=  null)
            {
                uint unitPropertyIndex = SpellEffect.DamageType + 0x8C;
                if( unitPropertyIndex <= 196) 
                {
                    //need to implement entire unit stat array like in game structure
                    //DamageDealtMultiplierSpell, DamageType = 1
                    //DamageDealtMultiplierPhysical, DamageType = 2
                    //DamageDealtMultiplierTech, DamageType = 3
                    //DamageDealtMultiplierMagic, DamageType = 4 
                    unitPropertyScaling = 1.0f; 
                }
            } else
            {
                effectTypeScaling = 1.0f;
            }

            return Math.Max(effectTypeScaling, unitPropertyScaling) * scaledValue;
        }

        float ScaledUnitVitalParameters(Spell4EffectsEntry SpellEffect)
        {
            float replacementValue = 0;
            float immediateValue = 0;
            uint parameterType = 0;
            float parameterValue = 0;

            for (uint i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        parameterType = SpellEffect.ParameterType00;
                        parameterValue = SpellEffect.ParameterValue00;
                        break;
                    case 1:
                        parameterType = SpellEffect.ParameterType01;
                        parameterValue = SpellEffect.ParameterValue01;
                        break;

                    case 2:
                        parameterType = SpellEffect.ParameterType02;
                        parameterValue = SpellEffect.ParameterValue02;
                        break;

                    case 3:
                        parameterType = SpellEffect.ParameterType03;
                        parameterValue = SpellEffect.ParameterValue03;
                        break;

                    default:
                        parameterType = SpellEffect.ParameterType00;
                        parameterValue = SpellEffect.ParameterValue00;
                        break;
                }

                switch ((SpellEffectParameterType)parameterType)
                {
                    case SpellEffectParameterType.PerLevel_SpellEffectParameterType:
                        {
                            immediateValue = PerLevel(_casterUnit);
                            break;
                        }

                    case SpellEffectParameterType.ItemBudget_SpellEffectParameterType:
                        {
                            immediateValue = parameterValue;
                            break;
                        }

                    case SpellEffectParameterType.CasterCurrentHealth_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.Health;
                            break;
                        }

                    case SpellEffectParameterType.CasterMaxHealth_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.MaxHealth;
                            break;
                        }

                    case SpellEffectParameterType.CasterMissingHealth_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.MaxHealth - _casterUnit.Health;
                            break;
                        }

                    case SpellEffectParameterType.CasterShieldCapacity_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.ShieldCapacity;
                            break;
                        }

                    case SpellEffectParameterType.CasterMaxShieldCapacity_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.MaxShieldCapacity;
                            break;
                        }

                    case SpellEffectParameterType.CasterMissingShields_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.MaxShieldCapacity - _casterUnit.ShieldCapacity;
                            break;
                        }

                    case SpellEffectParameterType.TargetCurrentHealth_SpellEffectParameterType:
                        {
                            immediateValue = 0;
                            //immediateValue = _targetUnit.Health;
                            break;
                        }

                    case SpellEffectParameterType.TargetMaxHealth_SpellEffectParameterType:
                        {
                            immediateValue = 0;
                            //immediateValue = _targetUnit.MaxHealth;
                            break;
                        }

                    case SpellEffectParameterType.TargetMissingHealth_SpellEffectParameterType:
                        {
                            immediateValue = 0;
                            //immediateValue = _targetUnit.MaxHealth - _targetUnit.Health;
                            break;
                        }

                    case SpellEffectParameterType.TargetShieldCapacity_SpellEffectParameterType:
                        {
                            immediateValue = 0;
                            //immediateValue = _targetUnit.ShieldCapacity;
                            break;
                        }

                    case SpellEffectParameterType.TargetMaxShieldCapacity_SpellEffectParameterType:
                        {
                            immediateValue = 0;
                            //value = _targetUnit.MaxShieldCapacity;
                            break;
                        }

                    case SpellEffectParameterType.TargetMissingShields_SpellEffectParameterType:
                        {
                            immediateValue = 0;
                            //value = _targetUnit.MaxShieldCapacity - _targetUnit.ShieldCapacity;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }

                replacementValue = replacementValue + (immediateValue * parameterValue);
            }

            return replacementValue;
        }

        uint PerLevel(IUnitService Unit)
        {
            uint perLevelValue = 0;

            perLevelValue = Unit.EffectiveLevel;
            if(perLevelValue == 0)
            {
                perLevelValue = Unit.Level;
            }
            return perLevelValue;
        }

        float ScaledUnitSecondaryParameters(Spell4EffectsEntry SpellEffect)
        {
            float replacementValue = 0;
            float immediateValue = 0;
            uint parameterType = 0;
            float parameterValue = 0;

            for(uint i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        parameterType = SpellEffect.ParameterType00;
                        parameterValue = SpellEffect.ParameterValue00;
                        break;
                    case 1:
                        parameterType = SpellEffect.ParameterType01;
                        parameterValue = SpellEffect.ParameterValue01;
                        break;

                    case 2:
                        parameterType = SpellEffect.ParameterType02;
                        parameterValue = SpellEffect.ParameterValue02;
                        break;

                    case 3:
                        parameterType = SpellEffect.ParameterType03;
                        parameterValue = SpellEffect.ParameterValue03;
                        break;

                    default:
                        parameterType = SpellEffect.ParameterType00;
                        parameterValue = SpellEffect.ParameterValue00;
                        break;
                }

                switch ((SpellEffectParameterType)parameterType)
                {
                    case SpellEffectParameterType.AssaultPower_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.AssaultPower;
                            break;
                        }

                    case SpellEffectParameterType.Brutality_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.Brutality;
                            break;
                        }

                    case SpellEffectParameterType.Finesse_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.Finesse;
                            break;
                        }

                    case SpellEffectParameterType.Grit_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.Grit;
                            break;
                        }

                    case SpellEffectParameterType.Insight_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.Insight;
                            break;
                        }

                    case SpellEffectParameterType.Moxie_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.Moxie;
                            break;
                        }

                    case SpellEffectParameterType.SupportPower_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.SupportPower;
                            break;
                        }

                    case SpellEffectParameterType.Tech_SpellEffectParameterType:
                        {
                            immediateValue = _casterUnit.Tech;
                            break;
                        }

                    default:
                        {
                            immediateValue = 0;
                            break;
                        }
                }

                replacementValue = replacementValue + (immediateValue * parameterValue);
            }

            return (float)Math.Round(replacementValue);
        }

        ReplaceableTextToken ProcessEffect(ReplaceableTextToken token, string text)
        {
            Spell4EffectsEntry? spell4Effects = _gameTableService.Spell4Effects.GetEntry((ulong)token.id);

            if (spell4Effects == null) { return token; }

            int number = GetNumberFromString(text);
            if (number >= 0)
            {
                token.modifierIndex = number;
            }

            bool containsPct = text.Contains("pct");
            bool containsPctAdd = text.Contains("pctAdd");
            bool containsPctSub = text.Contains("pctSub");
            bool containsAbs = text.Contains("abs");

            if (text.Contains("input"))
            {
                token.modifier = "input";
                int parameterIndex = GetNumberAfterString("input", text);

                float replacementValue = 0;

                replacementValue = CumulativeScaledEffectParameter(spell4Effects, parameterIndex);

                if (containsAbs)
                {
                    replacementValue = Math.Abs(replacementValue);
                }

                string replacementText = $"{replacementValue}";

                token.replacementText = replacementText;
            }
            else if (text.Contains("data"))
            {
                int parameterIndex = GetNumberAfterString("data", text);
                uint dataBits = 0;

                if (parameterIndex >= 10) 
                {
                    return token;
                }

                switch (parameterIndex)
                {
                    case 0:
                        dataBits = spell4Effects.DataBits00;
                        break;
                    case 1:
                        dataBits = spell4Effects.DataBits01;
                        break;
                    case 2:
                        dataBits = spell4Effects.DataBits02;
                        break;
                    case 3:
                        dataBits = spell4Effects.DataBits03;
                        break;
                    case 4:
                        dataBits = spell4Effects.DataBits04;
                        break;
                    case 5:
                        dataBits = spell4Effects.DataBits05;
                        break;
                    case 6:
                        dataBits = spell4Effects.DataBits06;
                        break;
                    case 7:
                        dataBits = spell4Effects.DataBits07;
                        break;
                    case 8:
                        dataBits = spell4Effects.DataBits08;
                        break;
                    case 9:
                        dataBits = spell4Effects.DataBits09;
                        break;

                }

                if (text.Contains("msf", StringComparison.OrdinalIgnoreCase) || text.Contains("ms", StringComparison.OrdinalIgnoreCase))
                {
                    float value = BitConverter.ToSingle(BitConverter.GetBytes(dataBits), 0);
                    if(containsAbs)
                    {
                        value = Math.Abs(value);
                    } 
                    token.replacementText = (value * 0.001f).ToString() ;
                }

                if(!containsPctAdd)
                {
                    if(!containsPctSub)
                    {
                        if (!containsPct)
                        {
                            if (text.Contains("rawInt", StringComparison.CurrentCultureIgnoreCase))
                            {
                                token.replacementText = dataBits.ToString();
                            }
                            else
                            {

                                float value = BitConverter.ToSingle(BitConverter.GetBytes(dataBits), 0);
                                if (containsAbs)
                                {
                                    value = Math.Abs(value);
                                }
                                token.replacementText = value.ToString("0.000");

                            }
                        }
                        else
                        {
                            float value = BitConverter.ToSingle(BitConverter.GetBytes(dataBits), 0);
                            value = value * 10.0f;
                            token.replacementText = value.ToString("0.00") + "%";
                        }
                    } else
                    {
                        float value = BitConverter.ToSingle(BitConverter.GetBytes(dataBits), 0);
                        value = 1000.0f - (value * 1000.0f);
                        value = (float)Math.Round(value)/10.0f;
                        if (containsAbs)
                        {
                            value = Math.Abs(value);
                        }
                        token.replacementText = value.ToString("0.00") + "%";
                    }
                } else
                {
                    float value = BitConverter.ToSingle(BitConverter.GetBytes(dataBits), 0);
                    value = (value * 1000.0f) - 1000.0f;
                    value = (float)Math.Round(value)/10.0f;
                    if (containsAbs)
                    {
                        value = Math.Abs(value);
                    }
                    token.replacementText = value.ToString("0.00") + "%";
                }

            }

            if (text.Contains("tickTimeMS", StringComparison.OrdinalIgnoreCase))
            {
                token.replacementText = (spell4Effects.TickTime * 0.001f).ToString(".0");
            }
            else if (text.Contains("durationMS",StringComparison.OrdinalIgnoreCase))
            {
                token.replacementText = (spell4Effects.DurationTime * 0.001f).ToString(".0");
            } 

            return token;
        }

        int GetNumberAfterString(string name, string text)
        {
            int startOfName = text.IndexOf(name);
            int number = GetNumberFromString( text.Substring(startOfName + name.Length) );

            return number;
        }

        ReplaceableTextToken ProcessSpell(ReplaceableTextToken token, string text)
        {
            Spell4Entry spell = _gameTableService.Spell4.GetEntry((ulong)token.id);

            if (spell == null) { return token; }

            int number = GetNumberFromString(text);
            if (number >= 0)
            {
                token.modifierIndex = number;
            }

            bool containsPct = text.Contains("pct");
            bool containsPctAdd = text.Contains("pctAdd");
            bool containsPctSub = text.Contains("pctSub");
            bool containsAbs = text.Contains("abs");

            if (text.Contains("castTime", StringComparison.InvariantCultureIgnoreCase))
            {
                if (spell.TooltipCastTime > 0)
                {
                    token.replacementText = (spell.TooltipCastTime * 0.001f).ToString();
                } else
                {
                    token.replacementText = (spell.CastTime * 0.001f).ToString();
                }
            }

            if (text.Contains("cd", StringComparison.InvariantCultureIgnoreCase))
            {
                    token.replacementText = "TO BE WORKED ON";
            }

            if (text.Contains("sc", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = _gameTableService.Spell4StackGroup.GetEntry((ulong)spell.Spell4StackGroupId).StackCap.ToString();
            }

            if (text.Contains("auraDuration", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = spell.SpellDuration.ToString();
            }

            if (text.Contains("chargeTotal", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = "TO BE WORKED ON";
            }

            if (text.Contains("rechargeTime", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = "TO BE WORKED ON";
            }

            if (text.Contains("rechargeAmount", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = "TO BE WORKED ON";
            }

            if (text.Contains("totalDuration", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = spell.ThresholdTime.ToString();
            }

            if (text.Contains("cost0", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = spell.InnateCost0.ToString();
            }

            if (text.Contains("cost1", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = spell.InnateCost1.ToString();
            }

            if (text.Contains("minRange", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = spell.TargetMinRange.ToString();
            }

            if (text.Contains("maxRange", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = spell.TargetMaxRange.ToString();
            }

            if (text.Contains("channelMax", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = (spell.ChannelMaxTime * 0.001f).ToString();
            }

            if (text.Contains("channelPulse", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = (spell.ChannelPulseTime * 0.001f).ToString();
            }
            else if (text.Contains("channelPulse", StringComparison.InvariantCultureIgnoreCase))
            {
                token.replacementText = (spell.ChannelPulseTime * 0.001f).ToString();
            }
            else
            {
                if (text.Contains("channelDelay", StringComparison.InvariantCultureIgnoreCase))
                {
                    token.replacementText = (spell.ChannelInitialDelay * 0.001f).ToString();
                }
                else
                {
                    if (text.Contains("lasData", StringComparison.InvariantCultureIgnoreCase))
                    {
                        token.replacementText = "TO BE WORKED ON";
                    }
                    if (text.Contains("threat", StringComparison.InvariantCultureIgnoreCase))
                    {
                        token.replacementText = "TO BE WORKED ON";
                    }
                    if (text.Contains("aoe", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Spell4AoeTargetConstraintsEntry aoeEntry = _gameTableService.Spell4AoeTargetConstraints.GetEntry(spell.Spell4AoeTargetConstraintsId);

                        if (aoeEntry == null) { return token; }

                        if (text.Contains("aoeMax", StringComparison.InvariantCultureIgnoreCase))
                        {
                            token.replacementText = aoeEntry.MaxRange.ToString();
                        }
                        if (text.Contains("aoeMin", StringComparison.InvariantCultureIgnoreCase))
                        {
                            token.replacementText = aoeEntry.MinRange.ToString();
                        }
                        if (text.Contains("aoeTc", StringComparison.InvariantCultureIgnoreCase))
                        {
                            token.replacementText = aoeEntry.TargetCount.ToString();
                        }
                    }
                    if (text.Contains("Tooltip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        token.replacementText = "TO BE WORKED ON";
                    }
                    if (text.Contains("mp", StringComparison.InvariantCultureIgnoreCase))
                    {
                        token.replacementText = "TO BE WORKED ON";

                        if (text.Contains("mpms", StringComparison.InvariantCultureIgnoreCase))
                        {
                            token.replacementText = "TO BE WORKED ON";
                        }
                    }
                }
            }
            return token;
        }

        ReplaceableTextToken ProcessCC(ReplaceableTextToken token, string text, uint spellId)
        {
            Spell4EffectsEntry? spell4Effects = _gameTableService.Spell4Effects.GetEntry((ulong)token.id);

            uint ccAdditionalDataId = spell4Effects.DataBits07;
            CCStateAdditionalDataEntry ccAdditionalData = _gameTableService.CCStateAdditionalData.GetEntry(ccAdditionalDataId);

            int parameterIndex = GetNumberFromString(text);
            if (parameterIndex >= 0)
            {
                token.modifierIndex = parameterIndex;
            }

            bool containsPct = text.Contains("pct");
            bool containsPctAdd = text.Contains("pctAdd");
            bool containsPctSub = text.Contains("pctSub");
            bool containsAbs = text.Contains("abs");

            if (text.Contains("dataInt", StringComparison.InvariantCultureIgnoreCase))
            {
                if (parameterIndex >= 5)
                {
                    return token;
                }

                uint dataBits = 0;

                switch (parameterIndex)
                {
                    case 0:
                        dataBits = ccAdditionalData.DataInt00;
                        break;
                    case 1:
                        dataBits = ccAdditionalData.DataInt01;
                        break;
                    case 2:
                        dataBits = ccAdditionalData.DataInt02;
                        break;
                    case 3:
                        dataBits = ccAdditionalData.DataInt03;
                        break;
                    case 4:
                        dataBits = ccAdditionalData.DataInt04;
                        break;
                }

                float value = 0;
                if (text.Contains("ms", StringComparison.InvariantCultureIgnoreCase))
                {
                    token.replacementText = (dataBits * 0.001f).ToString();
                    return token;
                }
                if (containsPctAdd)
                {
                    value = (dataBits * 1000.0f) - 1000.0f;
                    token.replacementText = Math.Round(value).ToString() + "%";
                    return token;
                }
                if (containsPctSub)
                {
                    value = 1000.0f - (dataBits * 1000.0f);
                    token.replacementText = Math.Round(value).ToString() + "%";
                    return token;
                }
                if (containsPct)
                {
                    value = (dataBits * 1000.0f);
                    token.replacementText = value.ToString() + "%";
                    return token;
                }
            }

            if (text.Contains("dataFloat", StringComparison.InvariantCultureIgnoreCase))
            {
                if(parameterIndex >= 5 )
                {
                    return token;
                }

                float dataBits = 0;

                switch (parameterIndex)
                {
                    case 0:
                        dataBits = ccAdditionalData.DataFloat00;
                        break;
                    case 1:
                        dataBits = ccAdditionalData.DataFloat01;
                        break;
                    case 2:
                        dataBits = ccAdditionalData.DataFloat02;
                        break;
                    case 3:
                        dataBits = ccAdditionalData.DataFloat03;
                        break;
                    case 4:
                        dataBits = ccAdditionalData.DataFloat04;
                        break;
                }

                float value = 0;
                if (text.Contains("ms", StringComparison.InvariantCultureIgnoreCase))
                {
                    token.replacementText = (dataBits * 0.001f).ToString();
                    return token;
                }
                if (containsPctAdd)
                {
                    value = (dataBits * 1000.0f) - 1000.0f;
                    token.replacementText = (Math.Round(value) / 10).ToString() + "%";
                    return token;
                }
                if (containsPctSub)
                {
                    value = 1000.0f - (dataBits * 1000.0f);
                    token.replacementText = (Math.Round(value) / 10).ToString() + "%";
                    return token;
                }
                if (containsPct)
                {
                    value = (dataBits * 100.0f);
                    token.replacementText = value.ToString() + "%";
                    return token;
                }
            }

            if (parameterIndex >= 5)
            {
                return token;
            }

            float floatBits = 0;

            switch (parameterIndex)
            {
                case 0:
                    floatBits = ccAdditionalData.DataFloat00;
                    break;
                case 1:
                    floatBits = ccAdditionalData.DataFloat01;
                    break;
                case 2:
                    floatBits = ccAdditionalData.DataFloat02;
                    break;
                case 3:
                    floatBits = ccAdditionalData.DataFloat03;
                    break;
                case 4:
                    floatBits = ccAdditionalData.DataFloat04;
                    break;

            }
            if (!text.Contains("rawInt", StringComparison.InvariantCultureIgnoreCase))
            {
                if (containsAbs)
                {
                    floatBits = Math.Abs(floatBits);
                }
                token.replacementText = floatBits.ToString();
            } else
            {
                token.replacementText = floatBits.ToString();
            }

            return token;
        }

        int GetNumberFromString(string text) 
        {
            int value = 0;
            char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int numberStart = text.IndexOfAny(numbers);

            if (numberStart == -1)
                return -1;

            int numberEnd = numberStart;

            if (numberStart + 1 < text.Length)
            {
                for (char c = text[++numberEnd]; Char.IsDigit(c);)
                {
                    if(++numberEnd >= text.Length)
                    {
                        break;
                    }
                    c = text[numberEnd];
                }
            }

            Int32.TryParse( text.Substring(numberStart, numberEnd - numberStart ), out value);
         
            return value;
        }
    }
}
