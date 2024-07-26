using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float HIGHLIGHT_SCALE = 1.3f;
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
    [SerializeField] private Image backgroundImage;

    public int cardId;
    public int cardValue;
    public int cardEffectId;
    public int cardAudioId;
    public CardType cardType;
    public string cardName;
    public string cardDescription;

    public void Init(int id, int val, CardType type, int effect_id, int audio_id, string name, string description)
    {
        cardId = id;
        backgroundImage.overrideSprite = CardImagesManager.Instance.GetCardImage(id);

        cardValue = val;
        if (cardValue > 0)
            valueText.text = "" + cardValue;
        else
            valueText.text = "";

        cardType = type;

        cardEffectId = effect_id;

        cardAudioId = audio_id;

        cardName = name;
        nameText.text = "" + cardName;

        cardDescription = description;
        descriptionText.text = "" + cardDescription;

        effectButton.SetActive(false);
        targetButton.SetActive(false);
    }

    public void IncrementValue(int val = 1)
    {
        cardValue += val;
        valueText.text = "" + cardValue;
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

        rt.localScale = new Vector3(HIGHLIGHT_SCALE, HIGHLIGHT_SCALE, 1.0f);
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
