using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown resolutionsDropdown;

    Resolution[] resolutions;

    public void Start()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width,  height = resolution.height }).Distinct().ToArray ();
        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "*" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;

            }
        }

        resolutionsDropdown.AddOptions(options);

        resolutionsDropdown.value = currentResolutionIndex;

        resolutionsDropdown.RefreshShownValue();

        Screen.fullScreen = true;

    }

    public void SetMusicVolume(float volume)
    {
        
        audioMixer.SetFloat("Volume",volume);


    }

    public void SetSoundVolume(float volume)
    {

        // audioMixer.SetFloat("SoundVolume", volume);

    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}