using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{
    public  TMP_InputField playerName;
    private SaveData data;
    void Start()
    {
        //création des fichier si inexistant
        data = new SaveData();
        data.InitJson();
        //initialisation des Données de data
        data.LoadClassementFromJson();
        data.LoadGameModeFromJson();
    }

    public void Chrono()
    {
        data._classementData.actualPlayer.setName(playerName.text);
        data._gameMode.EnableChrono();
        data.SaveClassementIntoJSON();
        data.SaveGameModeIntoJson();
        SceneManager.LoadScene("GameScene");
    }
    public void Infini()
    {
        data._gameMode.DisableChrono();
        data.SaveGameModeIntoJson();
        SceneManager.LoadScene("GameScene");
    }
}
