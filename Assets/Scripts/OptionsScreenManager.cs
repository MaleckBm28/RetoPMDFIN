using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsScreenManager : MonoBehaviour
{
    public Slider volumeSlider;
    public string mainMenu = "MainMenu";

    private const string VOLUME_KEY = "MasterVolume";

    void Start()
    {
        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    public void OnSliderValueChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(value);
        }
        else
        {
            Debug.LogWarning("OptionsScreenManager no pudo encontrar el AudioManager.Instance!");
        }
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);
    }
}