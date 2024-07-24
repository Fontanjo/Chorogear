using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

	public List<Card> deck;
	public TextMeshProUGUI deckSizeText;

	public Transform[] cardSlots;
	public bool[] availableCardSlots;

	public List<Card> discardPile;
	public TextMeshProUGUI discardPileSizeText;

	private Animator camAnim;

	private void Start()
	{
		camAnim = Camera.main.GetComponent<Animator>();
	}



	public void Shuffle()
	{
		if (discardPile.Count >= 1)
		{
			foreach (Card card in discardPile)
			{
				deck.Add(card);
			}
			discardPile.Clear();
		}
	}

	private void Update()
	{
		deckSizeText.text = deck.Count.ToString();
		discardPileSizeText.text = discardPile.Count.ToString();
	}

}
