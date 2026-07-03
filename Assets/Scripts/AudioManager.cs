using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip dartShoot;
    [SerializeField] private AudioClip buttonPress;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Ambient")]
    [SerializeField] private AudioClip lavaAmbient;

    private AudioSource _sfxSource;
    private AudioSource _ambientSource;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.playOnAwake = false;

        _ambientSource = gameObject.AddComponent<AudioSource>();
        _ambientSource.clip = lavaAmbient;
        _ambientSource.loop = true;
        _ambientSource.volume = 0.3f;
        _ambientSource.playOnAwake = false;
        _ambientSource.Play();
    }

    public void PlayDartShoot() => _sfxSource.PlayOneShot(dartShoot);
    public void PlayButtonPress() => _sfxSource.PlayOneShot(buttonPress);
    public void PlayDoorOpen() => _sfxSource.PlayOneShot(doorOpen);
    public void PlayJump() => _sfxSource.PlayOneShot(jumpSound);
    public void PlayDeath() => _sfxSource.PlayOneShot(deathSound);
}