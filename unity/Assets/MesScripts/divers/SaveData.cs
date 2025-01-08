using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    //Data
    [SerializeField] public Classment _classementData = new Classment();
    [SerializeField] public Shadow _shadow = new Shadow();
    [SerializeField] public GameMode _gameMode = new GameMode();

    //Path
    private string classementPath = Application.persistentDataPath + "/classment.json";
    private string ShadowPath = Application.persistentDataPath + "/shadow.json";
    private string GameModePath = Application.persistentDataPath + "gamemode.json";

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

[System.Serializable]
public class Classment
{
    public List<Player> classement = new List<Player>();
    public Player actualPlayer;

    public void TryAddingToClassement()
    {
        for (int i = 0; i < classement.Count; i++)
        {
            if (int.Parse(classement[i].Score) < int.Parse(actualPlayer.Score))
            {
                classement.Insert(i, actualPlayer);
                if (i == 2)
                {
                    classement.RemoveAt(classement.Count - 1);
                }
            break;
            }
        }
    }
    public void setActualPlayer(Player newPlayer)
    {
        actualPlayer = newPlayer;
    }
}
[System.Serializable]
public class Player
{
    public string Name = "undefine";
    public string Score;

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



[System.Serializable]
public class Shadow
{
    private List<Vector3> ShadowMouvement;
    public void setShadowPath(List<Vector3> shadowPath) { ShadowMouvement = shadowPath; }
}

[System.Serializable]
public class GameMode
{
    private bool modeChrono;

    public void EnableChrono() { modeChrono = true; }
    public void DisableChrono() { modeChrono = false; }
    public bool mode() {  return modeChrono; }
}