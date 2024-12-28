using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public bool isBonus = true;
    RealistPlayerController player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<RealistPlayerController>();
            if (player)
            {
                List<IBonus> bonusList = isBonus? BonusManager.BonusEffects: BonusManager.MalusEffects;
                if (bonusList.Count > 0)
                {
                    Debug.Log("Bonus ajouter");
                    int randomBonusIndex = Random.Range(0, bonusList.Count);
                    IBonus bonus = bonusList[randomBonusIndex];
                    bonus.crate = this;
                    player.applyBonus(bonus);
                }
            }
        }
    }
}
