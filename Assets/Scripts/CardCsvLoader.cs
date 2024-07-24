using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class CardCsvLoader : MonoBehaviour
{
    [SerializeField] private TextAsset csvAsset;
    [SerializeField] private List<Card> cards = new List<Card>();

    public List<Card> Cards { get => cards; private set => cards = value; }
    
    public static CardCsvLoader Instance { get; private set; }

    void Awake()
    {

        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 


        string[] Data = csvAsset.text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < Data.Length; i++)
        {
            string[] splitData = Data[i].Split(';');
            if (splitData.Length < 3)
            {
                Debug.LogError($"Pourquoi {splitData.Length}");
            }

            try
            {
                // Card type
                CardManager.CardType type = CardManager.CardType.CREATURE;
                switch(splitData[0])
                {
                    case "Creature":
                        type = CardManager.CardType.CREATURE;
                        break;
                    case "Passif":
                        type = CardManager.CardType.PASSIV;
                        break;
                    case "Instantane":
                        type = CardManager.CardType.INSTANTANEOUS;
                        break;         
                };

                string name = splitData[1];
                string effect = splitData[2];
                int damage = Convert.ToInt32(splitData[3]);
                int id = Convert.ToInt32(splitData[4]);
                
                Cards.Add(new Card(id, damage, type, name, effect));
            }
            catch (Exception e)
            {
                Debug.LogError($"Could not parse card data: {e}");
            }
        }
    }
}