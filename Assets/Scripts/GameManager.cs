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
        HideSelectors(State.PLAYER1);
        HideSelectors(State.PLAYER2);
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

        // Show cards in editor
        foreach (Card c in newCards)
        {
            // TODO: instantiate and initialize a card prefab
            Debug.Log("Picked card with value" + c.value);

            Transform nc = Instantiate(card_GO.GetComponent<Transform>(), new Vector3(0, 0, 0), Quaternion.identity, newCardsContainer_GO.GetComponent<Transform>());
            CardManager cm = nc.gameObject.GetComponent<CardManager>();
            cm.Init(c.value, c.type);
        }
    }

    // Move the game to the next state
    public void NextState()
    {
        // TODO: put remaining cards back in deck
        // TODO: move to next state

        List<GameObject> toDestroy = new List<GameObject>();
        SO_Deck current_player_deck = p1.deck; // initialize with p1, possibly change in switch

        switch(currentState)
        {
            case State.PLAYER1:
                // Set destination deck
                current_player_deck = p1.deck;
                // Update state
                currentState = State.PLAYER2;
                break;
            case State.PLAYER2:
                // Set destination deck
                current_player_deck = p2.deck;
                // Update state
                currentState = State.PLAYER1;
                break;
        }

        // Add remaining card back to deck
        foreach (Transform child in newCardsContainer_GO.transform)
        {
            CardManager cm = child.gameObject.GetComponent<CardManager>();
            // TODO: check if card manager found

            // Add card to deck
            current_player_deck.AddCard(new Card(cm.cardValue, cm.cardType));

            toDestroy.Add(child.gameObject);
        }

        // Destroy all children
        toDestroy.ForEach(c => Object.Destroy(c.gameObject));



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
        // TODO: 

        switch (selectedCard.cardType)
        {
            case CardManager.CardType.CREATURE:
            case CardManager.CardType.PASSIV:
                ShowSelectors(currentState);
                break;
            case CardManager.CardType.INSTANTANEOUS:
                Debug.Log("Instantaneous card");
                // TODO: implement
                break;
        }
    }

    // Remove selected card
    public void DeselectCard()
    {
        selectedCard = null;
    }

    private void HideSelectors(State player)
    {
        switch(player)
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
    }

    private void ShowSelectors(State player)
    {
        switch(player)
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

        // Reset position
        selectedCard.gameObject.transform.localPosition = new Vector3(0, 0, 0);

        // Deselect card
        DeselectCard();
        HideSelectors(currentState);

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
