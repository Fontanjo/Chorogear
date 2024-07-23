using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck",
    menuName = "ScritableObjects/Deck")]

public class SO_Deck : ScriptableObject
{
    public List<Card> cards;

    private static System.Random rng = new System.Random();

    // Start is called before the first frame update
    public void Init()
    {
        // TODO: do not reinitialize each time if not necessary (or yes? TBD)
        cards = new List<Card>();

        // TODO: init cards
        for (int i = 0; i < 30; i++)
        {
            AddCard(new Card(i, CardManager.CardType.CREATURE));
        }
    }

    /// Shuffle the cards in the deck
    public void Shuffle()
    {
        cards = cards.OrderBy(_ => rng.Next()).ToList();
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
    public CardManager.CardType type;

    public Card(int val, CardManager.CardType cardType)
    {
        value = val;
        type = cardType;
    }
}