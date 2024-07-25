using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Load()
    {
        Debug.Log("Retour à la case départ ");
        SceneManager.LoadScene(1);
    }
    public void test()
    {
        Console.WriteLine("AHHHHHHHHHHHHHHHHHHHHHHHH");
    }
}
