using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    [SerializeField] private Card card;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI nameText;

    private void Awake()
    {
        if (!descriptionText && !nameText)
        {
            descriptionText = GetComponentInChildren<TextMeshProUGUI>();
            nameText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void FillInCard(Card newCard)
    {
        card = newCard;

        descriptionText.text = card.CardEffect;
        nameText.text = card.CardName;    


    }
}
