using NexusForever.SpellWorks.GameTable.Model;
using NexusForever.SpellWorks.Models;
using NexusForever.SpellWorks.Services;
using System.Diagnostics;


namespace NexusForever.SpellWorks.Services
{
    public class SpellTooltipParseService : ISpellTooltipParseService
    {
        private readonly ITextTableService _textTableService;
        private readonly IGameTableService _gameTableService;

        public SpellTooltipParseService(
            ITextTableService textTableService, 
            IGameTableService gameTableService)
        {
            _textTableService = textTableService;
            _gameTableService = gameTableService;
        }

        public string GetRawTooltip(ISpellModel spell)
        {
            string text = _textTableService.GetText(spell.Entry.LocalizedTextIdActionBarTooltip);

            return text;
        }

        public string Parse(ISpellModel spell)
        {
            string text = _textTableService.GetText(spell.Entry.LocalizedTextIdActionBarTooltip);

            var tokens = GetReplaceableTextTokens(spell, text);

            foreach (var token in tokens)
            {
                text = text + ($"\nstart:{token.start}, end:{token.end} type:{token.type}, id:{token.id}, modifierId:{token.modifierIndex}, modifier:{token.modifier}");
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
        }

        List<ReplaceableTextToken> GetReplaceableTextTokens(ISpellModel spell, string text)
        {
            List<ReplaceableTextToken> tokens = new List<ReplaceableTextToken>();

            tokens = FindNextToken(spell, tokens, text);

            return tokens;
        }

        List<ReplaceableTextToken> FindNextToken(ISpellModel spell, List<ReplaceableTextToken> tokens, string text)
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

                token = ProcessToken(spell, token, text.Substring(tokenStartIndex,tokenEndIndex - tokenStartIndex + 1));
                tokens.Add(token);
                FindNextToken(spell, tokens, text.Substring(tokenEndIndex + 1));   
            }
            return tokens;
        }

        ReplaceableTextToken ProcessToken(ISpellModel spell, ReplaceableTextToken token, string text)
        {
            Debug.WriteLine(text);
            int dotPosition = text.IndexOf('.');
            int equalSignPosition = text.IndexOf('=');
            int closingParenPosition = text.IndexOf(')');

            if (dotPosition != -1 && equalSignPosition != -1)
            {
                token.type = text.Substring(2, equalSignPosition - 2);
                Int32.TryParse( text.Substring(equalSignPosition + 1, dotPosition - equalSignPosition - 1), out token.id);

                string secondPart = text.Substring(dotPosition + 1, closingParenPosition - dotPosition - 1);

                if (token.type == "eff")
                {
                    token = ProcessEffect(spell, token, secondPart);
                }
            }

            return token;
        }

        public uint[] StatLocalisedStringId = { 134658, 66, 166022, 166023, 184460, 166024, 184461, 184462, 209246,
            209247, 209248, 221023, 313798, 313799, 318506, 318573, 681730, 681732, 694706, 746765, 318573,
            746766, 746767, 746769, 318506, 746770, 746771 };

        ReplaceableTextToken ProcessEffect(ISpellModel spell, ReplaceableTextToken token, string text)
        {
            Spell4EffectsEntry? spell4Effects = null;

            foreach (var entry in _gameTableService.Spell4Effects.Entries)
            {
                if (entry.Id == token.id)
                {
                    spell4Effects = entry;
                    break;
                }
            }

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
                float parameterValue = 0;
                uint parameterType = 0;

                if (containsPct)
                {
                    switch( token.modifierIndex )
                    {
                        case 0:
                            parameterValue = (spell4Effects.ParameterValue00 * 1000.0f) + 0.5f;
                            parameterType = spell4Effects.ParameterType00;
                            break;
                        case 1:
                            parameterValue = (spell4Effects.ParameterValue01 * 1000.0f) + 0.5f;
                            parameterType = spell4Effects.ParameterType01;
                            break;
                        case 2:
                            parameterValue = (spell4Effects.ParameterValue02 * 1000.0f) + 0.5f;
                            parameterType = spell4Effects.ParameterType02;
                            break;
                        case 3:
                            parameterValue = (spell4Effects.ParameterValue03 * 1000.0f) + 0.5f;
                            parameterType = spell4Effects.ParameterType03;
                            break;
                        default:
                            break;
                    }

                    string statText = _textTableService.GetText(StatLocalisedStringId[parameterType]);

                    if (containsPctAdd)
                    {
                        parameterValue = parameterValue - 1000;
                    } 
                    else if (containsPctSub)
                    {
                        parameterValue = 1000 - parameterValue;
                    }
                    if (containsAbs)
                    {
                        parameterValue = Math.Abs(parameterValue);
                    }

                    bool floatingPointPrecision = parameterValue != 10 * ((int)parameterValue / 10);

                    string replacementText = "";

                    if (floatingPointPrecision) 
                    {
                        replacementText = $"[{parameterValue * 0.1:F1}% {statText}]";
                    } 
                    else
                    {
                        replacementText = $"[{parameterValue * 0.1:F0}% {statText}]";
                    }

                    Debug.WriteLine(replacementText);
                }


                
            }
            else if (text.Contains("data"))
            {

            }


            return token;
        }

        int GetNumberAfterString(string name, string text)
        {
            int startOfName = text.IndexOf(name);
            int number = GetNumberFromString( text.Substring(startOfName + name.Length) );

            return 0;
        }

        ReplaceableTextToken ProcessSpell(ReplaceableTextToken token, string text)
        {
            char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int modifierIdPosition = text.IndexOfAny(numbers);
            int closingParenPosition = text.IndexOf(')');

            if (modifierIdPosition != -1 && closingParenPosition != -1)
            {
                token.modifier = text.Substring(0, modifierIdPosition + 1);
                //token.modifier = text.Substring(modifierIdPosition + 1, closingParenPosition - modifierIdPosition - 1);
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

            for(char c = text[++numberEnd]; Char.IsDigit(c);)
            {
                c = text[++numberEnd];
            }

            Int32.TryParse( text.Substring(numberStart, numberEnd - numberStart ), out value);
         
            return value;
        }
    }
}
