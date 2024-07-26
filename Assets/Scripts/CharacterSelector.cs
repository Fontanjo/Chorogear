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

    public Character characterP1;
    public Character characterP2;
    [SerializeField] private GameObject p1ChoiceObject;
    [SerializeField] private GameObject P1Turn;
    [SerializeField] private GameObject P2Turn;

    [SerializeField] private GameObject p2ChoiceObject;
    [SerializeField] private string sceneToLoadName = "MainGame";
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject Panel2;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        StartCoroutine(CountdownP1());
    }

    private IEnumerator CountdownP1()
    {
       
            yield return new WaitForSeconds((float)1.5);
           Panel.SetActive(false);
            P1Turn.SetActive(true);
        }

    private IEnumerator CountdownP2()
    {

        yield return new WaitForSeconds((float)1.5);
        Panel2.SetActive(false);
    }

    public void SelectCharP1Int(int character)
    {
        SelectCharP1((Character)character);
    }

    public void SelectCharP1(Character character)
    {
        characterP1 = character;
        p1ChoiceObject.SetActive(false);
        Panel2.SetActive(true);
        P2Turn.SetActive(true);
        StartCoroutine(CountdownP2());

    }
    public void SelectRandomP1()
    {
       int IndexAlea =  UnityEngine.Random.Range(0, Enum.GetNames(typeof(Character)).Length);
        Character characterAlea = (Character) IndexAlea;

Debug.Log("[1] Perso al�atoire : " + characterAlea );

        p1ChoiceObject.SetActive(false);
        Panel2.SetActive(true);
        P2Turn.SetActive(true);
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
        SelectCharP2((Character)character);
    }

    public void SelectCharP2(Character character)
    {
        StartCoroutine(CountdownP2());
        characterP2 = character;
        SceneManager.LoadScene(sceneToLoadName);
    }
}
