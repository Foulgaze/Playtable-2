using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class boardController: MonoBehaviour
{
    public int cardsPerRow;
    public float distance = 5;
    public Vector3 cardDimensions;
    Dictionary<Vector3, cardHolder> cardHolderScripts = new Dictionary<Vector3, cardHolder>();
    Dictionary<Vector3, cardHolder> vectorToCardOnField = new Dictionary<Vector3, cardHolder>();

    public void addCard(cardHolder c)
    {   
        cardHolderScripts[c.transform.position] = c;
    }  

    cardHolder getClosestCard(Vector3 mousePosition, Dictionary<Vector3, cardHolder> searchContainer)
    {
        List<Vector3> cardPositions = new List<Vector3>(searchContainer.Keys);
        int closestCard = 0;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < cardPositions.Count; ++i)
        {
            float distance = Vector3.Distance(mousePosition, cardPositions[i]);
            cardHolder currentCardHolder = searchContainer[cardPositions[i]];
            if (distance < closestDistance && currentCardHolder.canAddCard())
            {
                closestCard = i;
                closestDistance = distance;
            }
        }
        return searchContainer[cardPositions[closestCard]];
    }

    Vector3 raycastToField()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        
        if (ray.direction.y != 0)
        {
            float distance = ray.origin.y / -ray.direction.y;
            Vector3 intersectionPoint = ray.origin + ray.direction * distance;
            return intersectionPoint;
        }
        Debug.LogError("Somehow ray is not raycasting :/");
        return Vector3.zero;
    }


    public cardHolder getClosestCardHolder()
    {
        return getClosestCard(raycastToField(),cardHolderScripts);
    }


    public GameObject mouseOverClosestCard()
    {
        cardHolder card = getClosestCardHolder();
        if (card == null || card.cards.Count < 1)
        {
            return null;
        }
        GameObject lastCardInPile = card.cards[card.cards.Count-1];
        if (card.pointOnCard(lastCardInPile, raycastToField()))
        {
            return lastCardInPile;
        }
        return null;


    }

    void Update()
    {
       
    }

    GameObject spawnCube(Vector3 position)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(position.x,0, position.z);
        return cube;
    }
}
