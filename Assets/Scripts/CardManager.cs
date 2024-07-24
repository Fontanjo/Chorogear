using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum CardType
    {
        CREATURE, PASSIV, INSTANTANEOUS
    }

    [SerializeField] private Button cardButton;

    [SerializeField] private TextMeshProUGUI valueText;
    // [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI effectText;
    [SerializeField] private GameObject effectButton;

    public int cardId;
    public int cardValue;
    public CardType cardType;
    public string cardName;
    public string cardEffect;
    
    public void Init(int id, int value, CardType type, string name = "", string effect = "")
    {
        Debug.Log("Initializing card");

        cardId = id;

        valueText.text = "" + value;
        cardValue = value;

        nameText.text = "" + name;
        cardName = name;

        effectText.text = "" + effect;
        cardEffect = effect;

        cardType = type;

        effectButton.SetActive(false);
    }

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // TODO: make card bigger
        //Output to console the GameObject's name and the following message
        // Debug.Log("Cursor Entering " + cardName + " GameObject");
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // TODO: reset original size
        //Output the following message with the GameObject's name
        // Debug.Log("Cursor Exiting " + cardName + " GameObject");
    }

    public void AddSelfToGameManager()
    {
        GameManager.Instance.SetSelectedCard(this);
    }

    public void PreventSelection()
    {
        // TODO: check if button is assigned
        cardButton.interactable = false;
    }

    public void Highlight()
    {
        // TODO: handle rt not found
        RectTransform rt = gameObject.GetComponent<RectTransform>();

        rt.localScale = new Vector3(1.2f, 1.2f, 1.0f);
    }

    public void DeHighlight()
    {
        // TODO: handle rt not found
        RectTransform rt = gameObject.GetComponent<RectTransform>();

        rt.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void AllowClickForEffect()
    {
        effectButton.SetActive(true);
    }

    public void PreventClickForEffect()
    {
        effectButton.SetActive(false);
    }


    public void MarkForEffect()
    {
        Debug.Log("Button clicked");
        GameManager.Instance.MarkForEffect(this);
    }
}
