using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI topCentralText;
    [SerializeField] private TextMeshProUGUI remainingMovesText;

    public enum State
    {
        PLAYER1, PLAYER2
    };

    State currentState;

    public static GameManager Instance { get; private set; }

    [Header("For info (do not assign)")]
    // Currently selected card
    public CardManager selectedCard;
    public CardManager selectedEffectCard;

    public int remainingMoves = 0;
    private int MAX_MOVES = 3;

    // Keep track of cards on the table
    private CardManager[,] boardP1 = new CardManager[3, 2];
    private CardManager[,] boardP2 = new CardManager[3, 2];

    // Start is called before the first frame update
    void Start()
    {
        // Select initial state
        currentState = State.PLAYER1;

        // Initializations (draw cards, update UI, ...)
        InitDecks();
        InitUI();
        DrawCards();
        HideSelectors();
        DeselectCard();
        ResetRemainingMoves();
    }

    public void InitDecks()
    {
        p1.deck.Init();
        p2.deck.Init();
    }

    public void InitUI()
    {
        if (topCentralText != null)
            topCentralText.text = "Player 1";
    }

    public void DrawCards()
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
            cm.Init(c.id, c.value, c.type, c.effect_id, c.audio_id, c.name, c.description);
        }

        // Deselect all
        DeselectCard();
        DeselectEffectCard();
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
                // Update UI
                if (topCentralText != null)
                    topCentralText.text = "Player 2";
                break;
            case State.PLAYER2:
                // Set destination deck
                current_player_deck = p2.deck;
                // Update state
                currentState = State.PLAYER1;
                if (topCentralText != null)
                    topCentralText.text = "Player 1";
                break;
        }

        // Add remaining card back to deck
        foreach (Transform child in newCardsContainer_GO.transform)
        {
            // TODO: ensure child is a card prefab / contains a card manager
            CardManager cm = child.gameObject.GetComponent<CardManager>();

            // Add card to deck
            current_player_deck.AddCard(new Card(cm.cardId, cm.cardValue, cm.cardType, cm.cardEffectId, cm.cardAudioId, cm.cardName, cm.cardDescription));

            // Mark as object to destroy. Can not destroy directly while iterating
            toDestroy.Add(child.gameObject);
        }

        // Destroy all children
        toDestroy.ForEach(c => Object.Destroy(c.gameObject));

        // Draw new cards, update UI
        DrawCards();

        // Reset max # of moves
        ResetRemainingMoves();

    }

    // return current state (p1 or p2 playing)
    public State CurrentState()
    {
        return currentState;
    }

    public void ResetRemainingMoves()
    {
        // Possible to adapt based on player/effects/...
        remainingMoves = MAX_MOVES;
        UpdateMovesText();
    }

    public void DecreaseRemainingMoves()
    {
        remainingMoves -= 1;
        UpdateMovesText();
    }

    public void UpdateMovesText()
    {
        remainingMovesText.text = "" + remainingMoves;
    }

    /// When a card is clicked, keep a reference to it
    /// This facilitates placing it on the board
    public void SetSelectedCard(CardManager card)
    {
        // TODO: only allow if there are still actions (max 2 cards per turn)
        if (remainingMoves <= 0)
            return;

        // Deselect previous card
        DeselectCard();
        DeselectEffectCard();
        
        // Select new card
        selectedCard = card;

        // Highlight/show selected card
        selectedCard.Highlight();
        
        // Show correct selectors
        HideSelectors();
        HideClickableForEffect();
        HideClickableForTarget();
        switch (selectedCard.cardType)
        {
            case CardManager.CardType.CREATURE:
                ShowCreatureSelectors(currentState);
                break;
            case CardManager.CardType.PASSIV:
                ShowPassivSelectors(currentState);
                break;
            case CardManager.CardType.INSTANTANEOUS:
                ShowActivableCardsEffect(currentState, 0); // Instantaneous effect can only be applied to creatures ()
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

    // Remove selected card
    public void DeselectEffectCard()
    {
        selectedEffectCard = null;
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

    public void HideClickableForTarget()
    {
        for (int i = 0; i < boardP1.GetLength(0); i++)
        {
            for (int j = 0; j < boardP1.GetLength(1); j++)
            {
                if (boardP1[i, j] != null)
                {
                    if (boardP1[i, j] != null)
                        boardP1[i, j].PreventClickForTarget();
                }
            }
        }
        p1.PreventClickForTarget();
        for (int i = 0; i < boardP2.GetLength(0); i++)
        {
            for (int j = 0; j < boardP2.GetLength(1); j++)
            {
                if (boardP2[i, j] != null)
                {
                    if (boardP2[i, j] != null)
                        boardP2[i, j].PreventClickForTarget();
                }
            }
        }
        p2.PreventClickForTarget();
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

    /// Allow to click on cards to apply an effect
    private void ShowActivableCardsEffect(State player, int col = -1)
    {
        switch(player)
        {
            case State.PLAYER1:
                for (int i = 0; i < boardP1.GetLength(0); i++)
                {
                    for (int j = 0; j < boardP1.GetLength(1); j++)
                    {
                        if (col != -1 && col != j) // -1 to select all cols
                            continue;
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
                        if (col != -1 && col != j) // -1 to select all cols
                            continue;
                        if (boardP2[i, j] != null)
                        {
                            boardP2[i, j].AllowClickForEffect();
                        }
                    }
                }
                break;
        }
    }

    /// Allow to click on cards to mark as target for an effect
    private void ShowActivableCardsTarget(State player, int row = -1, int col = -1)
    {
        Debug.Log("Row " + row);
        switch(player)
        {
            case State.PLAYER2:
                for (int i = 0; i < boardP1.GetLength(0); i++)
                {
                    if (row != -1 && row != i) // -1 to select all rows
                        continue;
                    for (int j = 0; j < boardP1.GetLength(1); j++)
                    {
                        if (col != -1 && col != j) // -1 to select all cols
                            continue;
                        if (boardP1[i, j] != null)
                        {
                            boardP1[i, j].AllowClickForTarget();
                        }
                    }
                }
                break;
            case State.PLAYER1:
                for (int i = 0; i < boardP2.GetLength(0); i++)
                {
                    if (row != -1 && row != i) // -1 to select all rows
                        continue;
                    for (int j = 0; j < boardP2.GetLength(1); j++)
                    {
                        if (col != -1 && col != j) // -1 to select all cols
                            continue;
                        if (boardP2[i, j] != null)
                        {
                            boardP2[i, j].AllowClickForTarget();
                        }
                    }
                }
                break;
        }
    }

    private void ShowActivableOppositePlayerTarget(State currentPlayer, int row = -1)
    {
        switch(currentPlayer)
        {
            case State.PLAYER2:
                // Can only attack player if no creature on that line
                if (boardP1[row, 0] == null)
                {
                    p1.AllowClickForTarget();
                }
                break;
            case State.PLAYER1:
                // Can only attack player if no creature on that line
                if (boardP2[row, 0] == null)
                {
                    p2.AllowClickForTarget();
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

        // Mark as played
        DecreaseRemainingMoves();

        // Deselect card
        DeselectCard();
        HideSelectors();

        // TODO: play sound

        // TODO: apply effects if any (or here never?)
    }

    public void MarkForEffect(CardManager cm)
    {
        // Possibly deselect previously selected effect
        DeselectEffectCard();

        // Register effect
        selectedEffectCard = cm;

        Vector2 car_pos = CardPositionOnBoard(cm, currentState);
        int row = (int)car_pos.x;
        int col = (int)car_pos.y;


        // TODO: Only show depending on effect
        switch (selectedCard.cardEffectId) // Effect is on the card used, not on the one just selected on the board
        {
            case 0: // Attack
                ShowActivableCardsTarget(currentState, row, -1);
                ShowActivableOppositePlayerTarget(currentState, row);
                break;
            case 1: // Exchange creature with opponent on the same line
                ShowActivableCardsTarget(currentState, row, 0);
                break;
        }

        // TODO: Block clicking on next card

        // Put it back in the deck
        switch(currentState)
        {
            case State.PLAYER1:
                p1.deck.AddCard(selectedCard.ToCardObject());
                break;
            case State.PLAYER2:
                p2.deck.AddCard(selectedCard.ToCardObject());
                break;
        }
        // Remove card from board
        Destroy(selectedCard.gameObject);

        // Mark as played
        DecreaseRemainingMoves();

        // TODO: Pass to next turn if enough cards played

        // Mark cards as not clickable for effect anymore
        HideClickableForEffect();
    }

    public void MarkForTarget(CardManager cm)
    {
        Debug.Log("Marked for target " + cm.cardName);
        HideClickableForTarget();

        Vector2 att_pos = CardPositionOnBoard(selectedEffectCard, currentState);
        Vector2 def_pos = CardPositionOnBoard(cm, OppositeState(currentState));

        switch (selectedCard.cardEffectId)
        {
            case 0: // Attack
                // Debug.Log("Attacking card on (" + def_pos.x + "," + def_pos.y + ")");
                // TODO: consider modifiers
                // Apply passiv effect
                int attack_value = selectedEffectCard.cardValue;
                attack_value += PassiveEffectOnRowN((int)att_pos[0], currentState);
                // Assume attack card is a creature, so at col 0
                if ((int)att_pos[1] == 1)
                    Debug.LogError("Attacking with a non-creature card");
                int defense_value = cm.cardValue;
                if ((int)def_pos[1] == 0)
                    defense_value += PassiveEffectOnRowN((int)def_pos[0], OppositeState(currentState));
                // When attacking, if one value > other, destroy weaker card. Else destroy both if even force
                if (attack_value >= defense_value)
                {
                    // Destroy defense (target) card
                    switch(currentState)
                    {
                        case State.PLAYER1:
                            p2.deck.AddCard(cm.ToCardObject());
                            break;
                        case State.PLAYER2:
                            p1.deck.AddCard(cm.ToCardObject());
                            break;
                    }
                    Destroy(cm.gameObject);
                }
                if (defense_value >= attack_value)
                {
                    // Destroy attack (own) card
                    switch(currentState)
                    {
                        case State.PLAYER1:
                            p2.deck.AddCard(selectedEffectCard.ToCardObject());
                            break;
                        case State.PLAYER2:
                            p1.deck.AddCard(selectedEffectCard.ToCardObject());
                            break;
                    }
                    Destroy(selectedEffectCard.gameObject);
                }
                break;
                case 1: // Exchange
                    Debug.Log("Selected for exchange");
                    // Exchange on boardP1
                    switch(currentState)
                    {
                        case State.PLAYER1:
                            boardP2[(int)def_pos[0], (int)def_pos[1]] = boardP1[(int)att_pos[0], (int)att_pos[1]];
                            boardP1[(int)att_pos[0], (int)att_pos[1]] = cm;
                            break;
                        case State.PLAYER2:
                            boardP1[(int)def_pos[0], (int)def_pos[1]] = boardP2[(int)att_pos[0], (int)att_pos[1]];
                            boardP2[(int)att_pos[0], (int)att_pos[1]] = cm;
                            break;
                    }
                    // TODO: exchange on UI
                    Transform tempParent = selectedEffectCard.transform.parent;
                    selectedEffectCard.transform.SetParent(cm.transform.parent);
                    selectedEffectCard.transform.localPosition = new Vector3(0, 0, 0);

                    cm.transform.SetParent(tempParent);
                    cm.transform.localPosition = new Vector3(0, 0, 0);

                    break;
        }

        // TODO: Implement other effects
    }

    private int PassiveEffectOnRowN(int row, State player)
    {
        Debug.Log("Row " + row);
        int v = 0;

        CardManager[,] board = boardP1;
        CardManager[,] opponentBoard = boardP2;

        switch (player)
        {
            case State.PLAYER1:
                board = boardP1;
                opponentBoard = boardP2;
                break;
            case State.PLAYER2:
                board = boardP2;
                opponentBoard = boardP1;
                break;
        }

        // Passiv on same line
        if (board[row, 1] != null)
        {
            switch (board[row, 1].cardEffectId)
            {
                case 10: // +1 attack on line
                    v += 1;
                    break;
                case 11: // +2 attack on line, -1 attack on adjecent lines
                    v += 2;
                    break;
            }
        }

        // Passive line above
        if (row >= 1 && board[row - 1, 1] != null)
        {
            switch (board[row - 1, 1].cardEffectId)
            {
                case 11: // +2 attack on line, -1 attack on adjecent lines
                    v -= 1;
                    break;
            }
        }

        // Passive line below
        if (row <= 1 && board[row + 1, 1] != null)
        {
            switch (board[row + 1, 1].cardEffectId)
            {
                case 11: // +2 attack on line, -1 attack on adjecent lines
                    v -= 1;
                    break;
            }
        }

        // Opponent, same line
        if (opponentBoard[row, 1] != null)
        {
            switch (opponentBoard[row, 1].cardEffectId)
            {
                case 12: // -1 attack for opponent on line
                    v -= 1;
                    break;
            }
        }

        Debug.Log("Applying " + v + " effect");
        return v;
    }

    public void MarkPlayerForTarget(PlayerManager pm)
    {
        // Debug.Log("Player marked for target " + pm.hp);
        HideClickableForTarget();

        // The effect is on the card that is being played, not on the one on the table
        switch (selectedCard.cardEffectId)
        {
            case 0: // Attack
                Debug.Log("Attacking player");
                int attack_value = selectedEffectCard.cardValue;
                Vector2 att_pos = CardPositionOnBoard(selectedEffectCard, currentState);
                attack_value += PassiveEffectOnRowN((int)att_pos[0], currentState);
                pm.TakeDamage(attack_value);
                break;
        }
        
        // TODO: Apply effect
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

    private Vector2 CardPositionOnBoard(CardManager cm, State player)
    {
        int x = -1;
        int y = -1;


        switch (player)
        {

            case State.PLAYER1:
                for (int i = 0; i < boardP1.GetLength(0); i++)
                {
                    for (int j = 0; j < boardP1.GetLength(1); j++)
                    {
                        if (boardP1[i, j] == cm)
                        {
                            x = i;
                            y = j;
                            break;
                        }
                    }
                }
                break;
            case State.PLAYER2:
                for (int i = 0; i < boardP2.GetLength(0); i++)
                {
                    for (int j = 0; j < boardP2.GetLength(1); j++)
                    {
                        if (boardP2[i, j] == cm)
                        {
                            x = i;
                            y = j;
                            break;
                        }
                    }
                }
                break;
        }

        return new Vector2(x, y);
    }

    private State OppositeState(State state)
    {
        switch (state)
        {
            case State.PLAYER1:
                return State.PLAYER2;
            case State.PLAYER2:
                return State.PLAYER1;
        }
        // Default return, to make C# happy
        return State.PLAYER1;
    }
}
