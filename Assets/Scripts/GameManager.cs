using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Players")]
    public PlayerManager p1; 
    public PlayerManager p2;

    [Header("Prefabs")]
    [SerializeField] private GameObject card_GO;
    [SerializeField] private GameObject newCardsContainer_GO;

    public enum State
    {
        PLAYER1, PLAYER2
    };

    State currentState;

    // Start is called before the first frame update
    void Start()
    {
        // Some initialization
        // TODO: ensure players are selected


        // Select initial state
        currentState = State.PLAYER1;

        // Draw cards, update UI, ...
        UpdateUI();
    }


    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.PLAYER1:
                // Debug.Log("p1 playing");
                break;
            case State.PLAYER2:
                // Debug.Log("p2 playing");
                break;
        }
    }

    public void UpdateUI()
    {
        // Initialize with p1 values
        SO_Deck deck = p1.Deck();
        int nbCards = p1.NbOfCards();
        
        // Select correct values based on current player
        switch(currentState)
        {
            case State.PLAYER1:
                deck = p1.Deck();
                nbCards = p1.NbOfCards();
                break;
            case State.PLAYER2:
                deck = p2.Deck();
                nbCards = p2.NbOfCards();
                break;
        }

        // Shuffle deck
        deck.Shuffle();

        // Pick (5) cards
        List<Card> newCards = deck.PickCards(nbCards);

        foreach (Card c in newCards)
        {
            // TODO: instantiate and initialize a card prefab
            Debug.Log("Picked card with value" + c.value);

            Transform nc = Instantiate(card_GO.GetComponent<Transform>(), new Vector3(0, 0, 0), Quaternion.identity, newCardsContainer_GO.GetComponent<Transform>());
            nc.gameObject.name = "Card name";
        }

        // TODO: show cards in editor
    }

    // Move the game to the next state
    public void NextState()
    {
        switch(currentState)
        {
            case State.PLAYER1:
                currentState = State.PLAYER2;
                break;
            case State.PLAYER2:
                currentState = State.PLAYER1;
                break;
        }

        // Pick and draw new cards, update UI, ...
        UpdateUI();
    }

    // return current state (p1 or p2 playing)
    public State CurrentState()
    {
        return currentState;
    }
}
