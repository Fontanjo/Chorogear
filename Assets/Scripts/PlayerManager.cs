using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public SO_Deck deck;

    // There might be modifiers influencing this
    public int cardsEachTurn = 5;

    public int hp = 10;

    // Start is called before the first frame update
    void Play()
    {
        // Debug.Log("Starting player");
        // // deck = gameObject.AddComponent<Deck>();
        // // TODO: check if deck assigned
        // deck.Init();// Done through GameManager
    }

    public SO_Deck Deck()
    {
        return deck;
    }

    public int NbOfCards()
    {
        return cardsEachTurn;
    }
}
