using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AddCardsToHand : MonoBehaviour
{
    public List<Card> deckList = new List<Card>();
    public Transform[] CardSlots;
    public bool[] availableCardSlots;

    public void drawCard()
    {
        if (deckList.Count > 0)
        {
            Card randCard = deckList[Random.Range(0, deckList.Count)];


            for (int i = 0; i < CardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    randCard.gameObject.SetActive(true);
                    randCard.transform.position = CardSlots[i].position;
                    availableCardSlots[i] = false;
                    deckList.Remove(randCard);
                    return;
                }
            }

        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
