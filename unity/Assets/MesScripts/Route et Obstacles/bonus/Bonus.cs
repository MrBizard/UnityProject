using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public bool isBonus = true;
    RealistPlayerController player;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<IBonus> bonusList = isBonus ? BonusManager.BonusEffects : BonusManager.MalusEffects;
            if (bonusList.Count > 0)
            {
                Debug.Log("Bonus ajouter");
                int randomBonusIndex = Random.Range(0, bonusList.Count);
                IBonus bonus = bonusList[randomBonusIndex];
                bonus.crate = this;
                GameManager.InstanceGame.AddBonus(bonus);
            }
        }            
        
    }


}
