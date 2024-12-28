using System.Collections;
using UnityEngine;

public class SpeedBonus : IBonus
{
    private float targetSpeed;
    private RealistPlayerController player;
    public Bonus crate { get; set; }
    public float duration { get; set; }
    public SpeedBonus(float targetSpeed, float duration)
    {
        this.targetSpeed = targetSpeed;
        this.duration = duration;
        player = GameManager.InstanceGame.Player;

        if (player == null)
        {
            Debug.LogError("Player instance null GameManager");
        }
    }

    public IEnumerator applyEffect()
    {
        if (player == null) yield break;

        Debug.LogError("Speed : " + targetSpeed);
        player.lockSpeed(targetSpeed);

        yield return new WaitForSeconds(duration);
        Debug.Log("après 5 seconde");
        removeEffet();
    }

    public void removeEffet()
    {
        Debug.Log("remove");
        if (player != null)
        {
            Debug.Log("Suppression Effet");
            Debug.LogError("Speed : remove" );
            player.unlockSpeed();
            ((IBonus)this).DisableCrate();
        }
    }
}
