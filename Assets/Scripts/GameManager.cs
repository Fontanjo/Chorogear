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

    [Header("Playable positions")]
    public List<GameObject> selectors_p1;
    public List<GameObject> selectors_p2;

    public enum State
    {
        PLAYER1, PLAYER2
    };

    State currentState;

    public static GameManager Instance { get; private set; }

    public CardManager selectedCard;

    // Start is called before the first frame update
    void Start()
    {
        // Some initialization
        // TODO: ensure players are selected


        // Select initial state
        currentState = State.PLAYER1;

        // Draw cards, update UI, ...
        UpdateUI();
        DeselectCard();
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
            // nc.gameObject.name = "Card name";
            CardManager cm = nc.gameObject.GetComponent<CardManager>();
            cm.Init(c.value);
        }

        // TODO: show cards in editor
    }

    // Move the game to the next state
    public void NextState()
    {
        // TODO: put remaining cards back in deck
        // TODO: move to next state
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

    /// When a card is clicked, keep a reference to it
    /// This facilitates placing it on the board
    public void SetSelectedCard(CardManager card)
    {
        selectedCard = card;
        Debug.Log("New card clicked");

        // TODO: show selectors only if selected card is a "play immediately" and not an "attack"/...
        switch(currentState)
        {
            case State.PLAYER1:
                foreach (GameObject go in selectors_p1)
                {
                    go.SetActive(true);
                }
                break;
            case State.PLAYER2:
                foreach (GameObject go in selectors_p2)
                {
                    go.SetActive(true);
                }
                break;
        }
    }

    // Remove selected card
    public void DeselectCard()
    {
        selectedCard = null;
        Debug.Log("All cards deselected");

        switch(currentState)
        {
            case State.PLAYER1:
                foreach (GameObject go in selectors_p1)
                {
                    go.SetActive(false);
                }
                break;
            case State.PLAYER2:
                foreach (GameObject go in selectors_p2)
                {
                    go.SetActive(false);
                }
                break;
        }

        // TODO: hide where it can be played
    }

    public void PlaceSelectedCard(Transform parent, int row, int col)
    {
        Debug.Log("Placing selected card! (" + row + "," + col + ")");
        // TODO: check if a card is selected

        // TODO: place selected card as child of the provided parent
        selectedCard.gameObject.transform.SetParent(parent);
        RectTransform rt = selectedCard.gameObject.GetComponent<RectTransform>();
        
        // Set anchors
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        //
        selectedCard.gameObject.transform.localPosition = new Vector3(0, 0, 0);

        // TODO: deselect card

        // TODO: apply effects
    }

    private void Awake() 
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
}
