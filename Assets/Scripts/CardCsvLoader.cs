using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class CardCsvLoader : MonoBehaviour
{
    [SerializeField] private TextAsset csvAsset;

    private List<Card> Cards = new List<Card>();
    
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
    }

    void LoadCards()
    {
        string[] Data = csvAsset.text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < Data.Length; i++)
        {
            string[] splitData = Data[i].Split(';');
            if (splitData.Length != 7)
            {
                Debug.LogError($"Wrong line, len == {splitData.Length}");
                Debug.Log(Data[i]);
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
                string description = splitData[2];
                int damage = Convert.ToInt32(splitData[3]);
                int id = Convert.ToInt32(splitData[4]);
                int effect_id = Convert.ToInt32(splitData[5]);
                int audio_id = Convert.ToInt32(splitData[6]);
                
                Cards.Add(new Card(id, damage, type, effect_id, audio_id, name, description));
            }
            catch (Exception e)
            {
                Debug.LogError($"Could not parse card data: {e}");
            }
        }
    }

    public List<Card> GetAllCards()
    {
        if (Cards != null && Cards.Count > 0)
        {
            return Cards;
        }

        LoadCards();
        return Cards;
    }
}