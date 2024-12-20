using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chronoMode : MonoBehaviour
{
    public GameObject player;
    private float time = 10.0f;
    private Vector3 startPos;
    void Start()
    {
        startPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0.0f)
        {
            SceneManager.LoadScene("ENDChronoScene");
        }
    }
}
