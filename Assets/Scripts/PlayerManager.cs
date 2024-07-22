using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Deck deck;

    // There might be modifiers influencing this
    public int cardsEachTurn = 5;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting player");
        deck = new Deck();
    }

    public Deck Deck()
    {
        return deck;
    }

    public int NbOfCards()
    {
        return cardsEachTurn;
    }
}
