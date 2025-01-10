using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhantomController : MonoBehaviour
{
    SaveData Data;
    Shadow phantomPath;
    float timeUpdateInterval;
    float updateInterval;
    int nbMove = 0;
    // Start is called before the first frame update
    void Start()
    {
        Data = new SaveData();
        Data.LoadClassementFromJson();
        phantomPath = Data._classementData.bestRace;
        timeUpdateInterval = Data._classementData.bestRace.getInterval();
        updateInterval = 0.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (phantomPath != null)
        {
            if (!(phantomPath.isShadowEmpty())) {
                updateInterval -= Time.deltaTime;

                if (updateInterval <= 0.0f)
                {
                    //new pos
                    this.gameObject.transform.position = phantomPath.ShadowMouvement.shadowPath[nbMove];
                    //new rotation
                    this.transform.rotation = phantomPath.ShadowMouvement.shadowQuat[nbMove];
                    if (nbMove < phantomPath.ShadowMouvement.shadowPath.Count)
                    {
                        nbMove++;
                    }
                    updateInterval = timeUpdateInterval;
                }
            }
        }
    }
}
