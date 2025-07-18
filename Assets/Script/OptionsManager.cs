using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class OptionsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        // 1. Volume
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 1f);
        AudioListener.volume = volumeSlider.value;
        volumeSlider.onValueChanged.AddListener(v =>
        {
            AudioListener.volume = v;
            PlayerPrefs.SetFloat("masterVolume", v);
        });

        // 2. Fullscreen
        fullscreenToggle.isOn = PlayerPrefs.GetInt("isFullscreen", Screen.fullScreen ? 1 : 0) == 1;
        fullscreenToggle.onValueChanged.AddListener(isFs =>
        {
            Screen.fullScreen = isFs;
            PlayerPrefs.SetInt("isFullscreen", isFs ? 1 : 0);
        });

        // 3. Resolution Dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        var options = new System.Collections.Generic.List<string>();
        int currentIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string opt = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(opt);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                currentIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("resolutionIndex", currentIndex);
        ApplyResolution(resolutionDropdown.value);
        resolutionDropdown.onValueChanged.AddListener(idx =>
        {
            ApplyResolution(idx);
            PlayerPrefs.SetInt("resolutionIndex", idx);
        });
    }

    private void ApplyResolution(int index)
    {
        var res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}