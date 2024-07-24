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

    // Currently selected card
    public CardManager selectedCard;

    // Keep track of cards on the table
    private CardManager[,] boardP1 = new CardManager[3, 2];
    private CardManager[,] boardP2 = new CardManager[3, 2];

    // Start is called before the first frame update
    void Start()
    {
        // Select initial state
        currentState = State.PLAYER1;

        // Draw cards, update UI, ...
        InitDecks();
        UpdateUI();
        HideSelectors();
        DeselectCard();
    }


    // // Update is called once per frame
    // void Update()
    // {
    //     switch(currentState)
    //     {
    //         case State.PLAYER1:
    //             // Debug.Log("p1 playing");
    //             break;
    //         case State.PLAYER2:
    //             // Debug.Log("p2 playing");
    //             break;
    //     }
    // }
    public void InitDecks()
    {
        p1.deck.Init();
        p2.deck.Init();
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
            // Instantiate and initialize a card prefab
            Transform nc = Instantiate(card_GO.GetComponent<Transform>(), new Vector3(0, 0, 0), Quaternion.identity, newCardsContainer_GO.GetComponent<Transform>());
            nc.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 100);
            
            // Set values to prefab
            CardManager cm = nc.gameObject.GetComponent<CardManager>();
            cm.Init(c.id, c.value, c.type, c.name, c.effect);
        }
    }

    // Move the game to the next state
    public void NextState()
    {
        List<GameObject> toDestroy = new List<GameObject>();
        SO_Deck current_player_deck = p1.deck; // initialize with p1, possibly change in switch

        // Move to next state
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
            // TODO: ensure child is a card prefab / contains a card manager
            CardManager cm = child.gameObject.GetComponent<CardManager>();

            // Add card to deck
            current_player_deck.AddCard(new Card(cm.cardId, cm.cardValue, cm.cardType, cm.cardName, cm.cardEffect));

            // Mark as object to destroy. Can not destroy directly while iterating
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
        // Deselect previous card
        DeselectCard();
        
        // Select new card
        selectedCard = card;

        // Highlight/show selected card
        selectedCard.Highlight();
        
        // Show correct selectors
        HideSelectors();
        HideClickableForEffect();
        switch (selectedCard.cardType)
        {
            case CardManager.CardType.CREATURE:
                ShowCreatureSelectors(currentState);
                break;
            case CardManager.CardType.PASSIV:
                ShowPassivSelectors(currentState);
                break;
            case CardManager.CardType.INSTANTANEOUS:
                ShowActivableCards(currentState);
                // TODO: implement
                break;
        }
    }

    // Remove selected card
    public void DeselectCard()
    {
        if (selectedCard != null)
            selectedCard.DeHighlight();
    
        selectedCard = null;
    }

    private void HideSelectors()
    {
        foreach (GameObject go in selectors_p1)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in selectors_p2)
        {
            go.SetActive(false);
        }
    }

    public void HideClickableForEffect()
    {
        for (int i = 0; i < boardP1.GetLength(0); i++)
        {
            for (int j = 0; j < boardP1.GetLength(1); j++)
            {
                if (boardP1[i, j] != null)
                {
                    if (boardP1[i, j] != null)
                        boardP1[i, j].PreventClickForEffect();
                }
            }
        }
        for (int i = 0; i < boardP2.GetLength(0); i++)
        {
            for (int j = 0; j < boardP2.GetLength(1); j++)
            {
                if (boardP2[i, j] != null)
                {
                    if (boardP2[i, j] != null)
                        boardP2[i, j].PreventClickForEffect();
                }
            }
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

    private void ShowCreatureSelectors(State player)
    {
        switch(player)
        {
            case State.PLAYER1:
                foreach (GameObject go in selectors_p1)
                {
                    // Show only front line
                    if (go.GetComponent<PlaceSelector>().Col() == 0)
                        go.SetActive(true);
                }
                break;
            case State.PLAYER2:
                foreach (GameObject go in selectors_p2)
                {
                    // Show only front line
                    if (go.GetComponent<PlaceSelector>().Col() == 0)
                        go.SetActive(true);
                }
                break;
        }
    }

    private void ShowPassivSelectors(State player)
    {
        switch(player)
        {
            case State.PLAYER1:
                foreach (GameObject go in selectors_p1)
                {
                    // Show only front line
                    if (go.GetComponent<PlaceSelector>().Col() == 1)
                        go.SetActive(true);
                }
                break;
            case State.PLAYER2:
                foreach (GameObject go in selectors_p2)
                {
                    // Show only front line
                    if (go.GetComponent<PlaceSelector>().Col() == 1)
                        go.SetActive(true);
                }
                break;
        }
    }

    private void ShowActivableCards(State player)
    {
        switch(player)
        {
            case State.PLAYER1:
                for (int i = 0; i < boardP1.GetLength(0); i++)
                {
                    for (int j = 0; j < boardP1.GetLength(1); j++)
                    {
                        if (boardP1[i, j] != null)
                        {
                            boardP1[i, j].AllowClickForEffect();
                        }
                    }
                }
                break;
            case State.PLAYER2:
                for (int i = 0; i < boardP2.GetLength(0); i++)
                {
                    for (int j = 0; j < boardP2.GetLength(1); j++)
                    {
                        if (boardP2[i, j] != null)
                        {
                            boardP2[i, j].AllowClickForEffect();
                        }
                    }
                }
                break;
        }
    }

    public void PlaceSelectedCard(Transform parent, int row, int col)
    {
        // Don't do anything if no cards selected
        if (selectedCard == null)
            return;

        // Add card to board
        switch (currentState)
        {
            case State.PLAYER1:
                boardP1[row, col] = selectedCard;
                break;
            case State.PLAYER2:
                boardP2[row, col] = selectedCard;
                break;
        }

        // Place selected card as child of the provided parent
        selectedCard.gameObject.transform.SetParent(parent);
        RectTransform rt = selectedCard.gameObject.GetComponent<RectTransform>();
        
        // Set anchors
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        rt.sizeDelta = parent.GetComponent<RectTransform>().sizeDelta;

        // Reset position
        selectedCard.gameObject.transform.localPosition = new Vector3(0, 0, 0);

        // Do not allow to select it again
        selectedCard.PreventSelection();

        // Deselect card
        DeselectCard();
        HideSelectors();

        

        // TODO: apply effects if any (or here never?)
    }

    public void MarkForEffect(CardManager cm)
    {
        // TODO: Apply effect

        // TODO: Remove/delete card
        Destroy(selectedCard.gameObject);

        // TODO: Mark card as not clickable for effect anymore
        Debug.Log("" + cm.cardName + " marked for effect");
        HideClickableForEffect();
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
