using System;
using HECSFramework.Core;
using Components;
using System.Globalization;

namespace Systems
{
	[Serializable][Documentation(Doc.UI, Doc.GameLogic, "this system convert big nombers to short format")]
    public sealed class ConvertNumbersSystem : BaseSystem 
    {
        [Required]
        private ConvertNumbersSystemParametersComponent parameters;

        public override void InitSystem()
        {
        }

        public string ConvertNumber(int value, int maxLenth)
        {
            float number = value;
            var currentSuffixIndex = 0;

            while (number >= parameters.Divider && currentSuffixIndex < parameters.Suffixes.Length - 1)
            {
                number /= parameters.Divider;
                currentSuffixIndex++;
            }

            var roundedString = GetRoundedString(number, maxLenth);
            
            return $"{roundedString}{parameters.Suffixes[currentSuffixIndex]}";
        }

        private string GetRoundedString(float number, int maxLenth)
        {
            var numberStr = Math.Round(number, 5).ToString("G"+(maxLenth - 1).ToString(), CultureInfo.InvariantCulture); // -1 for suffixe

            return numberStr;
        }
    }
}