using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
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

    private float accel_x = 0;
    private float brake_x = 0;
    private float decel_x = 0;
    private float decel_val;
    private float brake_val;

    private float speed_y;
    private float speed;
    private int signAccel;
    private float amplitudeSpeed;
    // définir un coefficient entre la décélaration ou le freingage et la vitesse actuelle 
    private const float RAPPORT_DECELERATION_FREINAGE = 3.0f;

    private float horizontalInput;
    private float forwardInput;


    // Start is called before the first frame update
    void Start()
    {
        //StatsGame.instance.InitStatsGame(player.transform);
        speed = minSpeed;
        amplitudeSpeed = maxSpeed - minSpeed;
        RB = gameObject.GetComponent<Rigidbody>();
    }





    // Version Accelerer Freiner (tourner)     	Décélération automatique 
    //	Ne pas pouvoir dépasser une VitesseMax ni descendre en dessous d’une VitesseMin 
    // >>> Utiliser courbe d'animation 
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        // DECELERATION   => la courbe indique comment diminuer l'acceleration
        if (Mathf.Abs(forwardInput) <= 0.01f)
        {
            decel_x += Time.deltaTime;
            if (decel_x > 1) decel_x = 1;

            decel_val = decelerationSpeedCURVE.Evaluate(decel_x) * Time.deltaTime / timeFromMinToMax * RELATION_DECELERATION_SPEED;
            accel_x -= decel_val;

        }
        else
        {
            decel_x -= Time.deltaTime;
            if (decel_x < 0) decel_x = 0;
        }

        // FREINAGE   => la courbe indique comment diminuer l'acceleration
        if (forwardInput < -0.01f)
        {
            brake_x += Time.deltaTime;
            if (brake_x > 1) brake_x = 1; ; // / RELATION_BRAKING_SPEED * Time.deltaTime;
            brake_val = brakingSpeedCURVE.Evaluate(brake_x) * Time.deltaTime / timeFromMinToMax * RELATION_BRAKING_SPEED;
            accel_x -= brake_val;

        }
        else
        {
            brake_x -= Time.deltaTime;
            if (brake_x < 0) brake_x = 0;
        }

        // ACCELERATION
        if (forwardInput > 0.01f)
        {
            accel_x += Time.deltaTime / timeFromMinToMax;
        }

        if (accel_x < 0.0f) accel_x = 0.0f;
        if (accel_x > 1.0f) accel_x = 1.0f;

        speed_y = accelerationSpeedCURVE.Evaluate(accel_x);     // retounre une vitesse en fonction du cumul d'accélaration
        speed = minSpeed + (amplitudeSpeed) * speed_y;          // calcule la vitesse finale en fonction du max et min 



        deltaPosition = RB.transform.forward * speed * Time.fixedDeltaTime;
        if (boolConstraintProgressionOnRoutePlane)
            deltaPosition.y = 0;
        if (physicUpdateDone)
        { // la maj a été effectuée par le moteur physique (traitée dans FixedUpdate)
          // la prochaine modification s'applique à la position et rotation actuelle de l'objet
            positionCible = RB.transform.position;
            rotationAdd = Quaternion.identity;
            physicUpdateDone = false;
        }
        // else .... update est appelée 2 fois (ou plus) avant que fixed update n'ait été appeléé
        // dans ce cas on cumul les déplacements à effectuer avant que la prochaine maj physique n'ait lieu
        // idem , on cumul les rotations
        positionCible = positionCible + deltaPosition; // appliquer le(s) déplacement(s)
                                                       //==> tourner // >> rotation
        rotationAdd = rotationAdd * Quaternion.AngleAxis(Time.fixedDeltaTime * turnspeed *
        horizontalInput, Vector3.up);
        // Test sortie de route // appliquer le(s) rotation(s)
        if ((positionRoute.position.y - transform.position.y) > this.GetComponent<Renderer>().bounds.size.y)
            SceneManager.LoadScene("ENDscene");

    }

    void FixedUpdate()
    {
        RB.MovePosition(positionCible);
        RB.MoveRotation((RB.transform.rotation * rotationAdd).normalized);
        physicUpdateDone = true;

    }
}