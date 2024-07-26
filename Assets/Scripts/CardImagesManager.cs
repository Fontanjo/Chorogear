using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UIElements;

public class CardImagesManager : MonoBehaviour
{

    public static CardImagesManager Instance { get; private set; }

    [Header("Creatures images")]
    [SerializeField] private Sprite id_01;
    [SerializeField] private Sprite id_02;
    [SerializeField] private Sprite id_03;
    [SerializeField] private Sprite id_04;
    [SerializeField] private Sprite id_05;
    [SerializeField] private Sprite id_06;
    [SerializeField] private Sprite id_07;
    [SerializeField] private Sprite id_08;

    // [Header("Passiv images")]

    [Header("Instantaneous images")]
    [SerializeField] private Sprite id_09;
    [SerializeField] private Sprite id_16;
    
    [Header("Default image")]
    [SerializeField] private Sprite defaultImage;
    
    public Sprite GetCardImage(int cardID)
    {
        switch (cardID)
        {
            // ########### CREATURES ###########
            case 1:
                return id_01;
            case 2:
                return id_02;
            case 3:
                return id_03;
            case 4:
                return id_04;
            case 5:
                return id_05;
            case 6:
                return id_06;
            case 7:
                return id_07;
            case 8:
                return id_08;
            // ########### INSTANTANEOUS ###########
            case 9:  // Attack
            case 10:
            case 11:
            case 12:
            case 13:
                return id_09;
            case 16: // Exchange
                return id_16;
            // ########### PASSIV ###########
            case -1:
            default:
                Debug.Log("No image set for this card");
                return defaultImage;
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
