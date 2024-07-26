using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CentralTextHandler : MonoBehaviour
{
    [SerializeField] private GameObject bg_p1;
    [SerializeField] private GameObject bg_p2;

    [SerializeField] private GameObject cards_bg_p1;
    [SerializeField] private GameObject cards_bg_p2;


    [SerializeField] private TextMeshProUGUI text;

    public void SetPlayerTurn(int turn)
    {
        switch (turn)
        {
            case 1:
                bg_p1.SetActive(true);
                cards_bg_p1.SetActive(true);

                bg_p2.SetActive(false);
                cards_bg_p2.SetActive(false);

                text.text = "Player 1";
                break;
            case 2:
                bg_p1.SetActive(false);
                cards_bg_p1.SetActive(false);

                bg_p2.SetActive(true);
                cards_bg_p2.SetActive(true);
                text.text = "Player 2";
                break;
            default:
                Debug.Log("Invalid value passed for player, must be 1 or 2 but got " + turn);
                break;
        }
    }
}
