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

    public void MarkForPermutation()
    {
        GameManager.Instance.PermutateSelectedCard(transform.parent, row, col);
    }

    public int Row()
    {
        return row;
    }

    public int Col()
    {
        return col;
    }
}
