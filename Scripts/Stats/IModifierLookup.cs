using System.Collections.Generic;

namespace ZAM.Stats
{
    public interface IModifierLookup
    {
        IEnumerable<float> GetAdditiveModifiers(Stat stat);
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}
