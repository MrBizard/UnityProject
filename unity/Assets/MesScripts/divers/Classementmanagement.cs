using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;


public class Classementmanagement : MonoBehaviour
{
    SaveData Data;
    public Canvas canva;
    private string yourScore;
    // Start is called before the first frame update
    void Start()
    {
        Data.LoadClassementFromJson();
        yourScore ="Votre score : " + Data._classementData.actualPlayer.Score;
        int i = 0;
        Transform[] Classement = canva.GetComponentsInChildren<Transform>();
        foreach (Transform enfant in Classement) {
            if (enfant.GetComponent<TextMeshProUGUI>() != null) {
                if (enfant.name == "YourScore")
                {
                    enfant.GetComponent<TextMeshProUGUI>().text = yourScore;
                }
                else
                {
                    enfant.GetComponent<TextMeshProUGUI>().text = Data._classementData.classement[i].Name;
                    enfant.GetComponentInChildren<TextMeshProUGUI>().text = Data._classementData.classement[i].Score;
                }
                
            }
        }

    }

    public void Recommencer()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
