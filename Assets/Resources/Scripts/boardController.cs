using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boardController : MonoBehaviour
{
    public int cardsPerRow;
    // public List<GameObject> cardHolders;
    // List<Vector2> cardPositions;

    Dictionary<Vector2, GameObject> cardHolderDict;

    void Start()
    {

    }
    public void addCard(GameObject card)
    {
        cardHolderDict[new Vector2(card.transform.position.x,card.transform.position.z)] = card;
        // cardHolders.Add(card);
        // cardPositions.Add(new Vector2(card.transform.position.x,card.transform.position.z));
    }  

    public GameObject getClosestCard(Vector2 mousePosition)
    {
        List<Vector2> cardPositions = new List<Vector2>(cardHolderDict.Keys);
        cardPositions.Sort((a,b) => Vector2.Distance(a, mousePosition).CompareTo(Vector2.Distance(b, mousePosition)));
        return cardHolderDict[cardPositions[0]];
    }
}
