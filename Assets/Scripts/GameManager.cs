using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerManager player1, player2;

    public enum State
    {
        PLAYER1, PLAYER2, OTHER
    };

    State currentState;

    // Start is called before the first frame update
    void Start()
    {
        // Some initialization
        // TODO: ensure players are selected

        // Select initial state
        currentState = State.PLAYER1;

        // TODO: Draw cards, update UI, ...
    }


    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.PLAYER1:
                Debug.Log("p1 playing");
                break;
            case State.PLAYER2:
                Debug.Log("p2 playing");
                break;
        }
    }

    // Move the game to the next state
    public void nextState()
    {
        switch(currentState)
        {
            case State.PLAYER1:
                currentState = State.PLAYER2;
                // TODO: Draw new cards, update UI, ...
                break;
            case State.PLAYER2:
                currentState = State.PLAYER1;
                // TODO: Draw new cards, update UI, ...
                break;
        }
    }
}
