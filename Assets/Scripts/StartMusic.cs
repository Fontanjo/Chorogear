using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    public enum Clip
    {
        MAIN_AUDIO, INTRO_AUDIO
    };

    public Clip audio_track;

    // Start is called before the first frame update
    void Start()
    {
        if (SoundManager.Instance != null)
        {
            switch (audio_track)
            {
                case Clip.MAIN_AUDIO:
                    SoundManager.Instance.PlayMainClip();   
                    break;
                case Clip.INTRO_AUDIO:
                    SoundManager.Instance.PlayIntroClip();   
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Sound manager not initialized, make sure to start the game from menu scene");
        }
    }
}
