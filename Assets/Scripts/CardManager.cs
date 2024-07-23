using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI valueText;
    
    public void Init(int value)
    {
        Debug.Log("Initializing card");
        valueText.text= "" + value;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
