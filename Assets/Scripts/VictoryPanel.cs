using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class VictoryPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText;

    public void ShowVictory(string winner)
    {
        winnerText.text = winner + " won the game!";
        Debug.Log(winner + " won the game!");
        gameObject.SetActive(true);
    }

    public void ReturnToHome()
    {
        SceneManager.LoadScene("StartingScene");
    }
}
