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
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject effectButton;
    [SerializeField] private GameObject targetButton;

    public int cardId;
    public int cardValue;
    public int cardEffectId;
    public int cardAudioId;
    public CardType cardType;
    public string cardName;
    public string cardDescription;

    public void Init(int id, int value, CardType type, int effect_id, int audio_id, string name, string description)
    {
        cardId = id;

        valueText.text = "" + value;
        cardValue = value;

        cardType = type;

        cardEffectId = effect_id;

        cardAudioId = audio_id;

        nameText.text = "" + name;
        cardName = name;

        descriptionText.text = "" + description;
        cardDescription = description;


        effectButton.SetActive(false);
        targetButton.SetActive(false);
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
        GameManager.Instance.MarkForEffect(this);
    }

    public void AllowClickForTarget()
    {
        targetButton.SetActive(true);
    }

    public void PreventClickForTarget()
    {
        targetButton.SetActive(false);
    }


    public void MarkForTarget()
    {
        GameManager.Instance.MarkForTarget(this);
    }

    public Card ToCardObject()
    {
        return new Card(this.cardId, this.cardValue, this.cardType, this.cardEffectId, this.cardAudioId, this.cardName, this.cardDescription);
    }
}
