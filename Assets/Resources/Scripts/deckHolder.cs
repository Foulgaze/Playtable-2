using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deckHolder : cardHolder
{
    public void addCard(GameObject card)
    {
        cards.Add(card);
        
    }
}
