using System.Collections;
using UnityEngine;

public class SpeedBonus : IBonus
{
    private readonly float targetSpeed;
    private readonly GameManager gameManager;

    public Bonus crate { get; set; }
    public float duration { get; set; }
    public string name { get; }

    public SpeedBonus(float targetSpeed, float duration, string name)
    {
        this.name = name;
        this.targetSpeed = targetSpeed;
        this.duration = duration;

        gameManager = GameManager.InstanceGame;

        if (gameManager == null)
        {
            Debug.LogError("GameManager instance is null");
        }
    }

    public IEnumerator applyEffect()
    {
        if (gameManager == null) yield break;

        ((IBonus)this).DisableCrate();

        Debug.Log($"Applying SpeedBonus: {name}, Target Speed: {targetSpeed}");

        gameManager.SetSpeedManually(targetSpeed);

        yield return new WaitForSeconds(duration);

        removeEffet();
    }

    public void removeEffet()
    {
        if (gameManager != null)
        {
            Debug.Log($"Removing SpeedBonus: {name}");
            gameManager.ResetSpeedToDefault();
        }
    }
}
