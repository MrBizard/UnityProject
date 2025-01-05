using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    public static AudioManager Instance;
    
    public AudioClip bonus;
    public AudioClip brake;
    public AudioClip crateBrake;
    public AudioClip loose;
    public AudioClip start;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
