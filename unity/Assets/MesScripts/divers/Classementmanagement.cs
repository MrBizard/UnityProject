using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;


public class Classementmanagement : MonoBehaviour
{
    SaveData Data;
    public Canvas canva;
    private string yourScore;
    // Start is called before the first frame update
    void Start()
    {
        //init data
        Data = new SaveData();
        Data.LoadClassementFromJson();
        yourScore ="Votre score : " + Data._classementData.actualPlayer.Score;
        int i = 0;
        Transform[] elemCanva = canva.GetComponentsInChildren<Transform>();

        //récupère les objet lié au canva
        for(int j = 0;j< elemCanva.Length;j+=2)
        {
            //si se sont des textmeshpro et pas des bouton on les modifie
            if (elemCanva[j].GetComponent<TextMeshProUGUI>() != null && elemCanva[j].transform.parent.GetComponent<UnityEngine.UI.Button>() == null)
            {
                //display le score personnel
                //ok
                if (elemCanva[j].name == "YourScore")
                    elemCanva[j].GetComponent<TextMeshProUGUI>().text = yourScore;
                //display le classement
                else
                {
                    //set le nom
                    elemCanva[j].GetComponent<TextMeshProUGUI>().text = Data._classementData.listScore[i].Name;
                    //set le score
                    elemCanva[j+1].GetComponent<TextMeshProUGUI>().text = Data._classementData.listScore[i].Score;
                
                    //passe a la prochaine position du classement
                    if (Data._classementData.listScore.Count > i + 1)
                    {
                        i++;
                    }
                }
            }
        }

    }


    //fonction lié au boutons
    public void Recommencer()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
