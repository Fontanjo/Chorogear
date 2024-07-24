using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CardManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum CardType
    {
        CREATURE, PASSIV, INSTANTANEOUS
    }

    [SerializeField] private TextMeshProUGUI valueText;
    // [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI effectText;

    public int cardValue;
    public CardType cardType;
    public string cardName;
    public string cardEffect;
    
    public void Init(int value, CardType type, string name = "", string effect = "")
    {
        Debug.Log("Initializing card");

        valueText.text = "" + value;
        cardValue = value;

        nameText.text = "" + name;
        cardName = name;

        effectText.text = "" + effect;
        cardEffect = effect;

        cardType = type;
    }

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // TODO: make card bigger
        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Entering " + name + " GameObject");
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // TODO: reset original size
        //Output the following message with the GameObject's name
        Debug.Log("Cursor Exiting " + name + " GameObject");
    }

    public void AddSelfToGameManager()
    {
        GameManager.Instance.SetSelectedCard(this);
    }
}
