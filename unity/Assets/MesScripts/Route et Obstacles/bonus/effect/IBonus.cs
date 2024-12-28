using System.Collections;

public interface IBonus
{    
    Bonus crate {  get; set; }
    float duration { get; set; }
    IEnumerator applyEffect();
    void removeEffet();
    public void DisableCrate()
    {
        if (crate != null)
        {
            crate.gameObject.SetActive(false);
        }
    }
}
