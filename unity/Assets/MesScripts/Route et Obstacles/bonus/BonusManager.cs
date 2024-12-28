using System.Collections.Generic;

public static class BonusManager
{
    public static List<IBonus> BonusEffects { get; private set; }
    public static List<IBonus> MalusEffects { get; private set; }

    static BonusManager()
    {
        BonusEffects = new List<IBonus>
        {
            new CrateSpeedBonus(1f, 5)
        };

        MalusEffects = new List<IBonus>
        {
            new SpeedBonus(20f,5),
            new CrateSpeedBonus(25f, 5)
        };
    }
}
