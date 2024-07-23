using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSelector : MonoBehaviour
{
    [SerializeField] int row;
    [SerializeField] int col;

    public void PlaceSelectedCardHere()
    {
        GameManager.Instance.PlaceSelectedCard(transform.parent, row, col);
    }
}
