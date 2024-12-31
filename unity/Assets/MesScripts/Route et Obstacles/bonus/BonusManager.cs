using System.Collections.Generic;

public static class BonusManager
{
    public static List<IBonus> BonusEffects { get; private set; }
    public static List<IBonus> MalusEffects { get; private set; }

    static BonusManager()
    {
        BonusEffects = new List<IBonus>
        {
            new CrateSpeedBonus(1f, 10, "bonus : vitesse crate diminuer")
        };

        MalusEffects = new List<IBonus>
        {
            new SpeedBonus(20f,20, "malus : vitesse joueur accélérer"),
            new CrateSpeedBonus(25f, 20, "malus : vitesse crate augmenter")
        };
    }
}
