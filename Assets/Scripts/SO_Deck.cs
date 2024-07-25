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

    public void Init()
    {
        // TODO: do not reinitialize each time if not necessary (or yes? TBD)
        // Generate new deck copying the existing cards (do not take direclty CardLoader list to avoid aliasing)
        cards = new List<Card>();

        Debug.Log("adding cards");
        foreach (Card card in CardCsvLoader.Instance.GetAllCards())
        {
            Debug.Log(card.id);
            cards.Add(new Card(card.id, card.value, card.type, card.effect_id, card.audio_id, card.name, card.description));
        }
        // cards = CardCsvLoader.Instance.Cards.ConvertAll(card => new Card(card.id, card.value, card.type, card.name, card.effect));
    }

    /// Shuffle the cards in the deck
    public void Shuffle()
    {
        if (cards != null)
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

public class Card
{
    public int id;
    public int value;
    public CardManager.CardType type;
    public int effect_id;
    public int audio_id;
    public string name = "";
    public string description = "";

    public Card(int id, int val, CardManager.CardType cardType, int effect_id, int audio_id, string name, string description)
    {
        this.id = id;
        this.value = val;
        this.type = cardType;
        this.effect_id = effect_id;
        this.audio_id = audio_id;
        this.name = name;
        this.description = description;
    }
}