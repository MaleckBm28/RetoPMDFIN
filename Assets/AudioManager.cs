using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private const string VOLUME_KEY = "MasterVolume";

    public AudioClip menuMusicClip;
    public AudioClip gameMusicClip;
    public AudioClip defeatMusicClip;
    public AudioClip victoryMusicClip;

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
        SetMasterVolume(savedVolume, false);

        if (menuMusicClip != null)
        {
            audioSource.clip = menuMusicClip;
            audioSource.Play();
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SetMasterVolume(float volume)
    {
        SetMasterVolume(volume, true);
    }

    public void SetMasterVolume(float volume, bool save)
    {
        AudioListener.volume = volume;

        if (save)
        {
            PlayerPrefs.SetFloat(VOLUME_KEY, volume);
            PlayerPrefs.Save();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName == "juego")
        {
            PlayNewTrack(gameMusicClip);
        }
        else if (sceneName == "Derrota")
        {
            PlayNewTrack(defeatMusicClip);
        }
        else if (sceneName == "Victoria")
        {
            PlayNewTrack(victoryMusicClip);
        }
        else if (sceneName == "MainMenu" || sceneName == "MenuOpciones")
        {
            PlayNewTrack(menuMusicClip);
        }
    }

    public void PlayNewTrack(AudioClip newClip)
    {
        if (newClip == null)
        {
            Debug.LogWarning("PlayNewTrack: Se ha pasado un clip nulo.");
            return;
        }

        if (audioSource.clip == newClip && audioSource.isPlaying)
        {
            return;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeTrack(newClip));
    }

    IEnumerator FadeTrack(AudioClip newClip)
    {
        float fadeTime = 0.5f;
        float startVolume = AudioListener.volume;
        float currentClipVolume = audioSource.volume;

        while (currentClipVolume > 0)
        {
            currentClipVolume -= startVolume * Time.unscaledDeltaTime / fadeTime;
            audioSource.volume = currentClipVolume;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        while (currentClipVolume < startVolume)
        {
            currentClipVolume += startVolume * Time.unscaledDeltaTime / fadeTime;
            audioSource.volume = currentClipVolume;
            yield return null;
        }

        audioSource.volume = startVolume;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}