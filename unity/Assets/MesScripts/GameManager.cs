using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager instance;

    public static GameManager InstanceGame
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    private void init()
    {
        ResetSpeedToDefault();
        activeBonuses = new List<IBonus>();
    }

    public RealistPlayerController Player { get; private set; }

    private float currentSpeed;
    private bool isSpeedLocked;
    
    private float accel_x = 0;
    private float brake_x = 0;
    private float decel_x = 0;
    private float decel_val;
    private float brake_val;

    private float speed_y;

    private List<IBonus> activeBonuses;

    public float GetCurrentSpeed()
    {
        if (!isSpeedLocked)
        {
            currentSpeed = Mathf.Clamp(currentSpeed, Player.minSpeed, Player.maxSpeed);
        }
        return currentSpeed;
    }

    public void SetSpeedManually(float speed)
    {
        currentSpeed = speed;
        LockSpeed();
    }

    public void ResetSpeedToDefault()
    {
        currentSpeed = Player.minSpeed;
        UnlockSpeed();
    }

    public void LockSpeed()
    {
        isSpeedLocked = true;
    }

    public void UnlockSpeed()
    {
        isSpeedLocked = false;
    }

    public void SetPlayer(RealistPlayerController player)
    {
        Player = player;
        init();
    }

    public void UpdateSpeed(float forwardInput, AnimationCurve speedCurve, float timeToMax)
    {
        if (isSpeedLocked) return;



        if (Mathf.Abs(forwardInput) <= 0.01f)
            {
                decel_x += Time.deltaTime;
                if (decel_x > 1) decel_x = 1;

                decel_val = Player.decelerationSpeedCURVE.Evaluate(decel_x) * Time.deltaTime 
                / Player.timeFromMinToMax * Player.RELATION_DECELERATION_SPEED;
                accel_x -= decel_val;

            }
            else
            {

                decel_x -= Time.deltaTime;
                if (decel_x < 0) decel_x = 0;
            }

            // FREINAGE   => la courbe indique comment diminuer l'acceleration
            if (forwardInput < -0.01f)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.brake);
                brake_x += Time.deltaTime;
                if (brake_x > 1) brake_x = 1; ; // / RELATION_BRAKING_SPEED * Time.deltaTime;
                brake_val = Player.brakingSpeedCURVE.Evaluate(brake_x) * Time.deltaTime 
                / Player.timeFromMinToMax * Player.RELATION_BRAKING_SPEED;
                accel_x -= brake_val;

            }
            else
            {
                brake_x -= Time.deltaTime;
                if (brake_x < 0) brake_x = 0;
            }

            // ACCELERATION
            if (forwardInput > 0.01f)
            {
                accel_x += Time.deltaTime / Player.timeFromMinToMax;
            }

            if (accel_x < 0.0f) accel_x = 0.0f;
            if (accel_x > 1.0f) accel_x = 1.0f;
            speed_y = Player.accelerationSpeedCURVE.Evaluate(accel_x);     // retounre une vitesse en fonction du cumul d'accélaration

            currentSpeed = Player.minSpeed + (Player.maxSpeed - Player.minSpeed) * speed_y;          // calcule la vitesse finale en fonction du max et min 
    }
    public void AddBonus(IBonus bonus)
    {
        RemoveEquivalentBonuses(bonus);
        activeBonuses.Add(bonus);

        if (Player != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.bonus);
            Player.StartCoroutine(bonus.applyEffect());
        }
    } 

    public void RemoveBonus(IBonus bonus)
    {
        bonus.removeEffet();
        activeBonuses.Remove(bonus);
    }

    private void RemoveEquivalentBonuses(IBonus newBonus)
    {
        List<IBonus> bonusesToRemove = new List<IBonus>();
        foreach (var bonus in activeBonuses)
        {
            if (AreBonusesEquivalent(bonus, newBonus))
            {
                bonusesToRemove.Add(bonus);
            }
        }

        foreach (var bonus in bonusesToRemove)
        {
            RemoveBonus(bonus);
        }
    }
    public void setMaxSpeed(float maxSpeed)
    {
        Player.maxSpeed = maxSpeed;
    }
    public void setMinSpeed(float minSpeed)
    {
        Player.minSpeed = minSpeed;
    }
    private bool AreBonusesEquivalent(IBonus bonus1, IBonus bonus2)
    {
        return bonus1.GetType() == bonus2.GetType();
    }
}
