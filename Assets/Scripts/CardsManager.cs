using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private CardCsvLoader cardCsvLoader;
    [SerializeField] private CardBehaviour[] cardSlots;

    private List<Card> deck;
    private bool[] availableCardSlots = new bool[5];

    private void Start()
    {
        if (!cardCsvLoader)
        {
            cardCsvLoader = GetComponent<CardCsvLoader>();
        }

        deck = cardCsvLoader.Cards;
        for (int i = 0; i < availableCardSlots.Length; i++)
        {
            availableCardSlots[i] = true;
        }

        DrawCard();
    }

    public void DrawCard()
    {
        if (deck.Count > 0)
        {
            int randomCardIndex = Random.Range(0, deck.Count);

            for (int i = 0; i < cardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    cardSlots[i].gameObject.SetActive(true);
                    cardSlots[i].FillInCard(deck[randomCardIndex]);
                    //randCard.transform.position = CardSlots[i].position;
                    availableCardSlots[i] = false;
                    deck.RemoveAt(randomCardIndex);
                    return;
                }
            }

        }
    }
}
