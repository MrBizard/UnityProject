using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    //Data
    [SerializeField] public Classment _classementData = new Classment();
    [SerializeField] public Shadow _shadow = new Shadow();
    [SerializeField] public GameMode _gameMode = new GameMode();

    //Path
    private string classementPath = Application.persistentDataPath + "/classment.json";
    private string ShadowPath = Application.persistentDataPath + "/shadow.json";
    private string GameModePath = Application.persistentDataPath + "/gamemode.json";

    //Init
    public void InitJson()
    {
        if (!File.Exists(classementPath))
        {
            _classementData.init();
            SaveClassementIntoJSON();
            Debug.Log("Création du fichier de classement");
        }
        if (!File.Exists(ShadowPath))
        {
            SaveShadowIntoJSON();
            Debug.Log("Création du fichier du Phantom");
        }
        if (!File.Exists(GameModePath))
        {
            SaveGameModeIntoJson();
            Debug.Log("Création du fichier du Mode de jeu");
        }
    }
    //Save
    public void SaveAll()
    {
        SaveClassementIntoJSON();
        SaveShadowIntoJSON();
        SaveGameModeIntoJson();
    }
    public void SaveClassementIntoJSON()
    {
        string classement = JsonUtility.ToJson(_classementData);
        System.IO.File.WriteAllText(classementPath, classement);
    }
    public void SaveShadowIntoJSON() 
    {
        string shadow = JsonUtility.ToJson(_shadow);
        System.IO.File.WriteAllText(ShadowPath, shadow);
    }
    public void SaveGameModeIntoJson()
    {
        string mode = JsonUtility.ToJson(_gameMode);
        System.IO.File.WriteAllText(GameModePath, mode);
    }

    //Load
    public void LoadAll()
    {
        LoadClassementFromJson();
        LoadShadowFromJson();
        LoadGameModeFromJson();
    }
    public void LoadClassementFromJson()
    {
        string classementData = System.IO.File.ReadAllText(classementPath);
        _classementData = JsonUtility.FromJson<Classment>(classementData);
    }
    public void LoadShadowFromJson()
    {
        string ShadowData = System.IO.File.ReadAllText(ShadowPath);
        _shadow = JsonUtility.FromJson<Shadow>(ShadowData);
    }
    public void LoadGameModeFromJson()
    {
        string mode = System.IO.File.ReadAllText(GameModePath);
        _gameMode = JsonUtility.FromJson<GameMode>(mode);
    }
}

//DATA
//classement
[System.Serializable]
public class Classment
{
    public List<Player> listScore = new List<Player>();
    public Player actualPlayer;
    public void init()
    {
        listScore.Add(new Player());
        listScore.Add(new Player());
        listScore.Add(new Player());
    }
    public void TryAddingToClassement()
    {

        for (int i = 0; i < listScore.Count; i++)
        {
            if (int.Parse(listScore[i].Score) < int.Parse(actualPlayer.Score))
            {
                Debug.Log(int.Parse(listScore[i].Score) + int.Parse(actualPlayer.Score) + "here score");
                listScore.Insert(i, actualPlayer);
                //limite a 3 le nombre d'élément
                while (listScore.Count > 3)
                {
                    listScore.RemoveAt(listScore.Count - 1);
                }
            break;
            }
        }
    }
    public void setActualPlayerName(string name)
    {
        actualPlayer.setName(name);
    }
    public void setActualPlayerScore(string score)
    {
        actualPlayer.setScore(score);
        TryAddingToClassement();
    }
}
//DataJoueur
[System.Serializable]
public class Player
{
    public string Name = "undefine";
    public string Score = "0";

    public void setScore(string score)
    {
        Score = score;
    }
    public void setName(string name) { 
        //si le nom est renseigné et non vide il est modifié sinon il reste "undefined"
        if(!string.IsNullOrEmpty(name))
            Name = name;
    }
}

//Phantom
[System.Serializable]
public class Shadow
{
    private List<Vector3> ShadowMouvement;
    public void setShadowPath(List<Vector3> shadowPath) { ShadowMouvement = shadowPath; }
}
//Mode de jeu
[System.Serializable]
public class GameMode
{
    public bool modeChrono;

    public void EnableChrono() { modeChrono = true; }
    public void DisableChrono() { modeChrono = false; }
}