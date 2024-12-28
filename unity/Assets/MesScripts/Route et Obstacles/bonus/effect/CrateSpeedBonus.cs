using System.Collections;
using UnityEngine;

public class CrateSpeedBonus : IBonus
{
    private readonly float targetSpeed;
    private float oldMinSpeed;
    private float oldMaxSpeed;
    public Bonus crate { get; set; }
    public float duration { get; set; }
    public CrateSpeedBonus(float targetSpeed, float duration)
    {
        this.targetSpeed = targetSpeed;
        this.duration = duration;
    }

    public IEnumerator applyEffect()
    {
        this.oldMaxSpeed = MovingObstacle.VITESSE_MAX;
        this.oldMinSpeed = MovingObstacle.VITESSE_MIN;

        MovingObstacle.VITESSE_MIN = this.targetSpeed;
        MovingObstacle.VITESSE_MAX = this.targetSpeed;
        MovingObstacle.globalSpeed = this.targetSpeed;

        yield return new WaitForSeconds(duration);

        removeEffet();
    }

    public void removeEffet()
    {
        Debug.Log("Suppression Effet");
        MovingObstacle.VITESSE_MIN = this.oldMinSpeed;
        MovingObstacle.VITESSE_MAX = this.oldMaxSpeed;
        MovingObstacle.globalSpeed = this.oldMinSpeed;
        ((IBonus)this).DisableCrate();
    }
}
