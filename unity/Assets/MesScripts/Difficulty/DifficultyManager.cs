using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Space(20)]
    [Header("object")]
    public RealistPlayerController player;
    public CreateRealTimeRoad map;

    [Space(20)]
    [Header("difficulty manage")]
    public DifficultyLevel[] diffLevel;
    public int actualDifficultyLevel;
    private float timeLastAugment = 0;
    private bool firstTime = true;
    // Start is called before the first frame update
    void Awake()
    {
        actualDifficultyLevel = PlayerPrefs.GetInt("Dif");
        diffLevel[actualDifficultyLevel].initDifficulty(player, map);
        map.setObs();
    }
    // Update is called once per frame
    void Update()
    {
        timeLastAugment += Time.deltaTime;
        if (firstTime)
        {
            augmentValue();
            firstTime = false;
        }
        else
            if (timeLastAugment >= diffLevel[actualDifficultyLevel].timeBeforApplyCoef)
                augmentValue();
    }
    void augmentValue()
    {
        DifficultyLevel level = diffLevel[actualDifficultyLevel];
        level.updateDifficulty(player, map);
        timeLastAugment = 0;
    }
}
