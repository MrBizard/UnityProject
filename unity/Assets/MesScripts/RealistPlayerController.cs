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
    public float speed { get; set; }

    private float horizontalInput;
    private float forwardInput;

    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        GameManager.InstanceGame.SetPlayer(this);
        GameManager.InstanceGame.UnlockSpeed();
    }


    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        // Mise à jour de la vitesse via le GameManager
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
    }


    private void FixedUpdate()
    {
        RB.MovePosition(positionCible);
        RB.MoveRotation((RB.transform.rotation * rotationAdd).normalized);
        physicUpdateDone = true;
    }
}
