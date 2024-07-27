using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public SO_Deck deck;

    // There might be modifiers influencing this
    public int cardsEachTurn = 5;

    public int hp = 10;

    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private GameObject targetButton;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTexts();
        targetButton.SetActive(false);
    }

    public SO_Deck Deck()
    {
        return deck;
    }

    public int NbOfCards()
    {
        return cardsEachTurn;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        UpdateTexts();
        GameManager.Instance.CheckVictory();
    }

    public void IncreaseHealth(int points)
    {
        hp += points;
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        if (hpText != null)
            hpText.text = "" + hp;
    }

    public void AllowClickForTarget()
    {
        targetButton.SetActive(true);
    }

    public void PreventClickForTarget()
    {
        targetButton.SetActive(false);
    }

    public void MarkPlayerForTarget()
    {
        GameManager.Instance.MarkPlayerForTarget(this);
    }
}
