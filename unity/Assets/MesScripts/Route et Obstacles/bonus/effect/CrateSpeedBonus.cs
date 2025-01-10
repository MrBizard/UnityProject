using System.Collections;
using UnityEngine;

public class CrateSpeedBonus : IBonus
{
    private readonly float targetSpeed;
    private float oldMinSpeed;
    private float oldMaxSpeed;
    public Bonus crate { get; set; }
    public float duration { get; set; }
    public string name { get; }

    public CrateSpeedBonus(float targetSpeed, float duration, string name)
    {
        this.targetSpeed = targetSpeed;
        this.duration = duration;
        this.name = name;
    }

    public IEnumerator applyEffect()
    {
        ((IBonus)this).DisableCrate();

        this.oldMaxSpeed = MovingObstacle.VITESSE_MAX;
        this.oldMinSpeed = MovingObstacle.VITESSE_MIN;

        MovingObstacle.VITESSE_MIN = targetSpeed;
        MovingObstacle.VITESSE_MAX = targetSpeed;
        MovingObstacle.globalSpeed = targetSpeed;
        Debug.Log($"Applying CrateBonus: {name}, Target Speed: {targetSpeed}");
        yield return new WaitForSeconds(duration);
            
        removeEffet();
    }

    public void removeEffet()
    {
        Debug.Log($"Removing CrateBonus: {name}");
        MovingObstacle.VITESSE_MIN = this.oldMinSpeed;
        MovingObstacle.VITESSE_MAX = this.oldMaxSpeed;
        MovingObstacle.globalSpeed = this.oldMinSpeed;
    }


}
