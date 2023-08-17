using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardHolder : MonoBehaviour
{
    public List<GameObject> cards;
    int cardLimit = 3;
    Vector3 multipleCardOffset;
    Vector3 cardExtents;
    Vector3 selfExtents;
    void Start()
    {
        cards = new List<GameObject>();
    }

    public void setValues (float horiOffset, float vertOffset, Vector3 cardExtents)
    {
        this.cardExtents = cardExtents;
        multipleCardOffset = new Vector3(cardExtents.x  * horiOffset, this.cardExtents.y*2, -cardExtents.z  * vertOffset);
        selfExtents = transform.GetComponent<MeshRenderer>().bounds.extents;
    }

    public bool pointOnCard(GameObject card, Vector3 point)
    {
        return Mathf.Abs(point.x - card.transform.position.x) <= cardExtents.x &&
                               Mathf.Abs(point.z - card.transform.position.z) <= cardExtents.z;
    }

    public int getCardCount()
    {
        return cards.Count;
    }

    public int getCardLimit()
    {
        return cardLimit;
    }
    
    public bool canAddCard()
    {
        return cards.Count < cardLimit;
    }

    public void addCard(GameObject card)
    {
        cards.Add(card);
        
    }
    public Vector3 getNextPosition(GameObject card)
    {
        return new Vector3(transform.position.x, transform.position.y + cardExtents.y + selfExtents.y, transform.position.z) + multipleCardOffset * cards.Count;
    }
}
