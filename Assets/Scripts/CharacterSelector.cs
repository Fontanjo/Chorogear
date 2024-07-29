using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    [Serializable]
    public enum Character : int
    {
        Jean,
        David,
        George,
        Paul,
    }

    [SerializeField] private GameObject p1ChoiceObject;
    [SerializeField] private GameObject p2ChoiceObject;

    [SerializeField] private string sceneToLoadName = "MainGame";
    
    [SerializeField] private GameObject Panel1;
    [SerializeField] private GameObject Panel2;

    void Start()
    {
        Panel1.SetActive(true);
        Panel2.SetActive(false);
        p1ChoiceObject.SetActive(false);
        p2ChoiceObject.SetActive(false);
        StartCoroutine(CountdownP1());
    }

    private IEnumerator CountdownP1()
    {  
            yield return new WaitForSeconds(2.5f);

            Panel1.SetActive(false);
            p1ChoiceObject.SetActive(true);
        }

    private IEnumerator CountdownP2()
    {
        yield return new WaitForSeconds(2.5f);
        Panel2.SetActive(false);
        p2ChoiceObject.SetActive(true);
    }

    private IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(sceneToLoadName);
    }

    public void SelectCharP1Int(int character)
    {
        PlayerPrefs.SetInt("Player1", character);
        Debug.Log("p1 selected " + character);

        // Play sound
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayCharacterAudio(character);
        }
        else
        {
            Debug.LogWarning("Sound manager not initialized, make sure to start the game from menu scene"); 
        }

        p1ChoiceObject.SetActive(false);
        Panel2.SetActive(true);

        StartCoroutine(CountdownP2());
    }

    public void SelectRandomP1()
    {
       int IndexAlea =  UnityEngine.Random.Range(0, Enum.GetNames(typeof(Character)).Length);
        Character characterAlea = (Character) IndexAlea;

        Debug.Log("[1] Perso aleatoire : " + characterAlea );

        p1ChoiceObject.SetActive(false);
        Panel2.SetActive(true);
        StartCoroutine(CountdownP2());
    }
    
    public void SelectRandomP2()
    {

        int IndexAlea = UnityEngine.Random.Range(0, Enum.GetNames(typeof(Character)).Length);
        Character characterAlea = (Character)IndexAlea;

        Debug.Log(" [2] Perso al�atoire : " + characterAlea);
        SceneManager.LoadScene(sceneToLoadName);

    }
    
    public void SelectCharP2Int(int character)
    {
        PlayerPrefs.SetInt("Player2", character);
        Debug.Log("p2 selected " + character);

        // Play sound
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayCharacterAudio(character);
        }
        else
        {
            Debug.LogWarning("Sound manager not initialized, make sure to start the game from menu scene"); 
        }

        StartCoroutine(WaitForLoad());
    }
}
