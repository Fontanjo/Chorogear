using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck",
    menuName = "ScritableObjects/Deck")]

public class SO_Deck : ScriptableObject
{
    public List<Card> cards;

    // Start is called before the first frame update
    public void Init()
    {
        // TODO: do not reinitialize each time if not necessary (or yes? TBD)
        cards = new List<Card>();

        // TODO: init cards
        for (int i = 0; i < 10; i++)
        {
            AddCard(new Card(i));
        }
    }

    public void Shuffle()
    {
        // TODO: shuffle list
    }

    public Card PickCard()
    {
        // TODO: check if there are enough cards
        Card c = cards[0];
        cards.RemoveAt(0);

        return c;
    }

    public List<Card> PickCards(int nbOfCards)
    {
        // TODO: check if there are enough cards

        List<Card> pickedCards = new List<Card>();

        for (int i = 0; i < nbOfCards; i++)
        {
            Card c = cards[0];
            cards.RemoveAt(0);

            pickedCards.Add(c);
        }

        return pickedCards;
    }

    public void AddCard(Card newCard)
    {
        // Add card at the end of the list
        cards.Add(newCard);
    }
}

public struct Card
{
    public int value;

    public Card(int val)
    {
        value = val;
    }
}