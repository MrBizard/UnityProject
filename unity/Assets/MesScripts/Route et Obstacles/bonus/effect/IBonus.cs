using System.Collections;
using UnityEngine;

public interface IBonus
{
    Bonus crate { get; set; }
    float duration { get; set; }
    string name { get; }
    IEnumerator applyEffect();
    void removeEffet();
    public void DisableCrate()
    {
        if (crate != null)
        {
            crate.gameObject.SetActive(false);
            Debug.Log($"Crate for bonus {name} has been disabled.");
        }
    }
}
