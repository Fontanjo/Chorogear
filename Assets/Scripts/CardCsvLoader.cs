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

    void Awake()
    {

        //string CSVPath = "Assets/Ressources/CardData.csv";
        //if (!csvAsset)
        //csvAsset = (TextAsset)Resources.Load(CSVPath, typeof(TextAsset));

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
                Card c = new Card();
                c.type = splitData[0];
                c.CardName = splitData[1];
                c.CardEffect = splitData[2];
                c.damage = Convert.ToInt32(splitData[3]);
                Cards.Add(c);
            }
            catch (Exception e)
            {
                Debug.LogError($"Could not parse card data: {e}");
            }
        }
    }
}
