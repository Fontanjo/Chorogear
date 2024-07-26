using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; }

    [Header("Audio source element")]
    [SerializeField] private AudioSource source;

    [Header("Audio clip for creatures")]
    [SerializeField] private AudioClip id_01;
    [SerializeField] private AudioClip id_02;
    [SerializeField] private AudioClip id_03;
    [SerializeField] private AudioClip id_04;
    [SerializeField] private AudioClip id_05;
    [SerializeField] private AudioClip id_06;
    [SerializeField] private AudioClip id_07;
    [SerializeField] private AudioClip id_08;
    
    public void PlayAudioById(int audioID)
    {
        switch (audioID)
        {
            case 1:
                source.PlayOneShot(id_01, 1.0f);
                break;
            case 2:
                source.PlayOneShot(id_02, 1.0f);
                break;
            case 3:
                source.PlayOneShot(id_03, 1.0f);
                break;
            case 4:
                source.PlayOneShot(id_04, 1.0f);
                break;
            case 5:
                source.PlayOneShot(id_05, 1.0f);
                break;
            case 6:
                source.PlayOneShot(id_06, 1.0f);
                break;
            case 7:
                source.PlayOneShot(id_07, 1.0f);
                break;
            case 8:
                source.PlayOneShot(id_08, 1.0f);
                break;
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
            Debug.Log("Instance initialized");
        } 
    }
}
