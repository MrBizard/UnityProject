using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    //Data
    [SerializeField] public Classment _classementData = new Classment();
    [SerializeField] public GameMode _gameMode = new GameMode();

    //Path
    private string classementPath = Application.persistentDataPath + "/classment.json";
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
        SaveGameModeIntoJson();
    }
    public void SaveClassementIntoJSON()
    {
        string classement = JsonUtility.ToJson(_classementData);
        System.IO.File.WriteAllText(classementPath, classement);
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
        LoadGameModeFromJson();
    }
    public void LoadClassementFromJson()
    {
        string classementData = System.IO.File.ReadAllText(classementPath);
        _classementData = JsonUtility.FromJson<Classment>(classementData);
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
    public Shadow bestRace = null;
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
                //si c'est le meilleur/nouveaux meilleur joueur on récupère son phantom
                if (i == 0)
                {
                    bestRace = actualPlayer.Race;
                }
                //sinon on lui enlève pour ne pas la sauvegarder
                else { 
                    actualPlayer.Race.emptyShadow(); 
                }
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
    public Shadow Race;

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
    public shadowPosition ShadowMouvement;
    public float interval;

    public bool isShadowEmpty()
    {
        return ShadowMouvement.shadowQuat.Count <=0 || ShadowMouvement.shadowPath.Count <= 0;
    }
    public void emptyShadow()
    {
        ShadowMouvement.shadowQuat.Clear();
        ShadowMouvement.shadowPath.Clear();
    }
    public void addShadowPos(Vector3 pos, Quaternion rot)
    {
        ShadowMouvement.shadowPath.Add(pos);
        ShadowMouvement.shadowQuat.Add(rot);
    }
    public void setInterval(float time)
    {
        interval = time;
    }
    public float getInterval() {  return interval; }
}

//Stoque les mouvement du fantome
[System.Serializable]
public class shadowPosition
{
    public List<Vector3> shadowPath;
    public List<Quaternion> shadowQuat;
}

//Mode de jeu
[System.Serializable]
public class GameMode
{
    public bool modeChrono;

    public void EnableChrono() { modeChrono = true; }
    public void DisableChrono() { modeChrono = false; }
}