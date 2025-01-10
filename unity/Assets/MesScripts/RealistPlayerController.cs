using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RealistPlayerController : MonoBehaviour
{
    public Transform positionRoute;

    private Rigidbody RB; // le rigidBody du véhicule ... à récupérer dans le Start()
    private Vector3 positionCible; // le prochain déplacement à effectuer
    private Quaternion rotationAdd; // la prochaine rotation à appliquer
    private Vector3 deltaPosition; // utilisée uniquement dans update
    private bool physicUpdateDone;
    [Tooltip("optimisation : contraindre la progression sur le plan route , sans tenu compte de l'orientation haut/bas du véhicule")]
    public bool boolConstraintProgressionOnRoutePlane;


    [Header("AVEC courbe d'animation")]
    [Range(3, 9)]
    public float minSpeed = 6f;
    [Range(10, 250)]
    public float maxSpeed = 100f;
    public float turnspeed = 20f;

    [Range(3, 10)]
    [Tooltip("Temps en seconde (float) pour passer de la vittesse MIN à MAX")]
    public float timeFromMinToMax = 5.0f;

    [Tooltip("L'évolution de la vitesse en fonction d'un paramètre d'accélération ")]
    public AnimationCurve accelerationSpeedCURVE;
    [Tooltip("L'évolution du paramètre d'accélération en absence d'accélération")]
    public AnimationCurve decelerationSpeedCURVE;
    [Tooltip("L'évolution du paramètre d'accélération en cas de freinage ")]
    public AnimationCurve brakingSpeedCURVE;
    // pas encore réaliste : impacter l'acceleration et non la vitesse directement fait utiliser la courbe d'acceleration
    // pour l'évolution de la vitesse qui diminue (= combinaison des 2 courbes)
    // on voit par exemple clairement les 2 pallier d'acélération  (en sens inverse)  : pas réaliste
    // lors des retours à la vitesse min (deceleration ou freinage) il faudrait impacter directement la vitesse (et pas l'acceleration)
    // pour ne plus utiliser la courbe d'acceleration dans cette phase
    // 
    [Tooltip("Coefficient multiplicateur : impact de la decelaration sur la vitesse / durée de retour à la vitesse min ")]
    [Range(1, 10)] public float RELATION_DECELERATION_SPEED = 1;
    [Tooltip("Coefficient multiplicateur : impact du freinage sur la vitesse  / durée de retour à la vitesse min")]
    [Range(1, 10)] public float RELATION_BRAKING_SPEED = 1;

    private float speed;

    private float horizontalInput;
    private float forwardInput;

    //Game mode
    public bool isChrono;
    public float chronoTime = 10.0f;
    SaveData Data;

    //Shadow
    public GameObject phantomPrefab;
    //stocke la durée
    private float timeSavingPlayerPos = 0.1f;
    //compte à rebour pour la sauvegarde
    private float SavingPlayerPos;
    private Shadow phantom;


    //Text data
    public TextMeshProUGUI TextTime;
    public TextMeshProUGUI TextScore;
    public string playerName;
    private string baseTimeText = "temps restant : ";
    private string baseScoreText = "Score : ";
    private string ScoreData;
    private string TimeData;


    // Start is called before the first frame update
    void Start()
    {
        //StatsGame.instance.InitStatsGame(player.transform);
        RB = GetComponent<Rigidbody>();
        GameManager.InstanceGame.SetPlayer(this);
        GameManager.InstanceGame.UnlockSpeed();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.start);

        //récupère toutes les données
        Data = new SaveData();
        Data.LoadAll();
        isChrono = Data._gameMode.modeChrono;
        phantom = Data._classementData.bestRace;
        phantomPrefab.GetComponent<Renderer>().material.color = Color.blue;

        if (!isChrono)
        {
            TextTime.enabled = false;
            phantomPrefab.SetActive(false);
            Data._classementData.actualPlayer.Race.emptyShadow();
        }
        else
        {
            //met le phantom au départ
            Data._classementData.actualPlayer.Race.emptyShadow();
            phantomPrefab.transform.position = RB.position;
            SavingPlayerPos = timeSavingPlayerPos;
        }
    }

    // Version Accelerer Freiner (tourner)     	Décélération automatique 
    //	Ne pas pouvoir dépasser une VitesseMax ni descendre en dessous d’une VitesseMin 
    // >>> Utiliser courbe d'animation 
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        GameManager.InstanceGame.UpdateSpeed(forwardInput, accelerationSpeedCURVE, timeFromMinToMax);

        deltaPosition = RB.transform.forward * GameManager.InstanceGame.GetCurrentSpeed() * Time.fixedDeltaTime;

        if (boolConstraintProgressionOnRoutePlane)
        {
            deltaPosition.y = 0;
        }

        if (physicUpdateDone)
        {
            positionCible = RB.transform.position;
            rotationAdd = Quaternion.identity;
            physicUpdateDone = false;
        }

        positionCible += deltaPosition;
        rotationAdd *= Quaternion.AngleAxis(Time.fixedDeltaTime * turnspeed * horizontalInput, Vector3.up);

        if ((positionRoute.position.y - transform.position.y) > GetComponent<Renderer>().bounds.size.y)
        {
            SceneManager.LoadScene("ENDscene");
        }

        ScoreData = Mathf.Round(this.transform.position.z).ToString();
        if (isChrono)
        {
            //Phantom désactiver si il n'existe pas
            if (phantom == null)
            {
                phantomPrefab.SetActive(false);
            }

            //sauvegarde la position et rotation du joueur
            SavingPlayerPos -= Time.deltaTime;
            if (SavingPlayerPos < 0.0f)
            {
                Data._classementData.actualPlayer.Race.addShadowPos(RB.transform.position, RB.transform.rotation);
                SavingPlayerPos = timeSavingPlayerPos;
            }

            //actualise le temps + l'affichage
            chronoTime -= Time.deltaTime;
            TimeData = Mathf.Round(chronoTime).ToString();
            TextDisplay();
            //Test de fin
            if ((positionRoute.position.y - transform.position.y) > this.GetComponent<Renderer>().bounds.size.y)
                EndChrono();
            if (chronoTime < 0.0f)
                EndChrono();
        }
        else
        {
            TextDisplay();
            // Test sortie de route // appliquer le(s) rotation(s)
            if ((positionRoute.position.y - transform.position.y) > this.GetComponent<Renderer>().bounds.size.y)
                SceneManager.LoadScene("ENDscene");
        }

    }

    void FixedUpdate()
    {
        RB.MovePosition(positionCible);
        RB.MoveRotation((RB.transform.rotation * rotationAdd).normalized);
        physicUpdateDone = true;

    }
    void EndChrono()
    {
        Data._classementData.setActualPlayerScore(ScoreData);
        Data._classementData.actualPlayer.Race.setInterval(timeSavingPlayerPos);
        Data.SaveClassementIntoJSON();
        SceneManager.LoadScene("ENDChronoScene");
    }
    void TextDisplay()
    {
        TextTime.text = baseTimeText + TimeData;
        TextScore.text = baseScoreText + ScoreData;
    }
}