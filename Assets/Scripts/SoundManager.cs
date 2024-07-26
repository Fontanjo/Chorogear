using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; }

    [Header("Audio source element")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource soundSource;


    [Header("UI audio")]
    [SerializeField] private AudioClip intro_clip;
    [SerializeField] private AudioClip main_clip;
    [SerializeField] private AudioClip button_clicked;

    [Header("Audio clip for creatures")]
    [SerializeField] private AudioClip id_01;
    [SerializeField] private AudioClip id_02;
    [SerializeField] private AudioClip id_03;
    [SerializeField] private AudioClip id_04;
    [SerializeField] private AudioClip id_05;
    [SerializeField] private AudioClip id_06;
    [SerializeField] private AudioClip id_07;
    [SerializeField] private AudioClip id_08;

    [Header("Audio clip for instantaneous cards")]
    [SerializeField] private AudioClip id_09;
    [SerializeField] private AudioClip id_10;

    [Header("Audio clip for passiv cards")]
    [SerializeField] private AudioClip id_11;
    

    public void PlayAudioById(int audioID)
    {
        switch (audioID)
        {
            // ########### CREATURES ###########
            case 1:
                soundSource.PlayOneShot(id_01, 1.0f);
                break;
            case 2:
                soundSource.PlayOneShot(id_02, 1.0f);
                break;
            case 3:
                soundSource.PlayOneShot(id_03, 1.0f);
                break;
            case 4:
                soundSource.PlayOneShot(id_04, 1.0f);
                break;
            case 5:
                soundSource.PlayOneShot(id_05, 1.0f);
                break;
            case 6:
                soundSource.PlayOneShot(id_06, 1.0f);
                break;
            case 7:
                soundSource.PlayOneShot(id_07, 1.0f);
                break;
            case 8:
                soundSource.PlayOneShot(id_08, 1.0f);
                break;
            // ########### INSTANTANEOUS ###########
            case 9: // Attack
                soundSource.PlayOneShot(id_09, 1.0f);
                break;
            case 10: // Others
                soundSource.PlayOneShot(id_10, 1.0f);
                break;
            // ########### PASSIV ###########
            case 11:
                soundSource.PlayOneShot(id_11, 1.0f);
                break;
            // ########### DEFAULT ###########
            case -1:
                Debug.Log("No audio set for this card");
                // source.PlayOneShot(id_01, 1.0f);
                break;
            default:
                Debug.Log("No audio set for this card");
                // source.PlayOneShot(id_01, 1.0f);
                break;               
        }
    }

    public void PlayIntroClip()
    {
        // source.Stop();
        source.clip = intro_clip;
        source.Play(0);
    }

    public void PlayMainClip()
    {
        // TODO: fade out intro / fade in main
        source.Stop();
        source.clip = main_clip;
        source.Play(0);
    }

    public void ButtonClicked()
    {
        source.PlayOneShot(button_clicked, 1.0f);
    }


    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;
                DontDestroyOnLoad(gameObject);
                Debug.Log("Instance initialized");
        } 
    }
}
