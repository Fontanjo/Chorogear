
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IsTheWInner : MonoBehaviour
{
    private string P1;
    private string P2;
    [SerializeField] private bool TheWinner;
    [SerializeField] private TextMeshProUGUI valueText;
    void Start()
    {


        valueText.text = "";
        Debug.Log(valueText.text);
        switch (TheWinner)
        {
            case false:
                valueText.text = "Victoire"; break;
            case true:
                valueText.text = "Défaite"; break;
        }
    }
    
}
