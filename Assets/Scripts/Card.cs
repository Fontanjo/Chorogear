	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Card : MonoBehaviour
	{

		public int handIndex;
		public int ID;
		public string CardName;
		public string CardEffect;
		public string damage;
		public string type;
    public void SetValues(int handIndex, int ID, string cardName, string cardEffect, string damage, string type)
    {
        this.handIndex = handIndex;
        this.ID = ID;
        this.CardName = cardName;
        this.CardEffect = cardEffect;
        this.damage = damage;
        this.type = type;
    }




}
