using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static CreateRealTimeRoad;

[CreateAssetMenu(fileName = "Data", menuName = "DifficultyLevel", order = 1)]
public class DifficultyLevel : ScriptableObject
{
    [Space(20)]
    [Header("Car")]
    [Tooltip("la vitesse minimum de base du vehicule")]
    [Range(0, 50)]
    public float minSpeed;
    [Tooltip("la vitesse minimum de base du vehicule")]
    [Range(5, 50)]
    public float maxSpeed;

    [Space(20)]
    [Header("Map")]
    [Tooltip("LA probabilité d'avoir un obsctacle lorsqu'un pattern de route est créé")]
    [Range(0.0f, 1.0f)]
    public float probaAllObst;
    [Tooltip("les obstacles possibles sur le jeu")]
    public probaObst[] probaObsts;

    [Space(30)]
    [Header("paramettre d'évolution")]
    [Tooltip("le pourcentage d'augmentation de vitesse de la voiture")]
    public float pourcentAugmentSpeed;
    [Tooltip("le pourcentage d'augmentation d'obstacles")]
    public float pourcentAugmentProbaObst;
    [Tooltip("le temps avant la première application des facteurs précédents")]
    public float firsttimeBeforApplyCoef;
    [Tooltip("le temps entre chaque application des facteurs précédents")]
    [Range(0, 60)]
    public float timeBeforApplyCoef;
    public void initDifficulty(RealistPlayerController player, CreateRealTimeRoad map)
    {
        player.minSpeed = minSpeed;
        player.maxSpeed = maxSpeed;

        map.probaAllObst = probaAllObst;
        map.probaObsts = probaObsts;
    }

    public void updateDifficulty(RealistPlayerController player, CreateRealTimeRoad map)
    {
        player.minSpeed *= 1+pourcentAugmentSpeed/100;
        player.maxSpeed *= 1+pourcentAugmentSpeed/100;

        map.probaAllObst *= 1+pourcentAugmentProbaObst/100;
    }
}

