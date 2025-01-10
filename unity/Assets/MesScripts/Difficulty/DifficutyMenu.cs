using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficutyMenu : MonoBehaviour
{
    //l'indice de difficulté
    public int difficultyIndex;
    private void Start()
    {
        setDifficulty(0);
    }
    public void setDifficulty(int index)
    {
        difficultyIndex = index;
        PlayerPrefs.SetInt("Dif", difficultyIndex);
        PlayerPrefs.Save();
    }
}
