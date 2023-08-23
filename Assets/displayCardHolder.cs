using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class displayCardHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cardPrefab;
    public GameObject cardHolder;
    public Scrollbar scrollbar; 
    public List<Sprite> temporarySprites;
    List<GameObject> cards;
    Rect boxArea;
    RectTransform cardSize;
    public int cardCount = 10;



    float horizontalCardPadding = 1;
    float verticalCardPadding = 1;
    float intracardXPadding = 1;
    float intracardYPadding =1 ;
    void Start()
    {

        cards = new List<GameObject>();
        for(int i = 0; i < cardCount; ++i)
        {
            GameObject go = GameObject.Instantiate(cardPrefab);
            go.name = $"{i}";
            go.transform.GetComponent<Image>().sprite = temporarySprites[i % temporarySprites.Count];
            cards.Add(go);
            go.transform.SetParent(cardHolder.transform);

        }   
        boxArea = RectTransformUtility.PixelAdjustRect(transform.GetComponent<RectTransform>(),GetComponent<Canvas>());
        cardSize = cards[0].transform.GetComponent<RectTransform>();
        setScrollValues();
        updateSearchBox();
    }


    void setScrollValues()
    {
        // float startPos = boxArea.width/-2 + horizontalCardPadding + cardSize.sizeDelta.x/2;
        // float horiPosition = boxArea.height/2 - verticalCardPadding - cardSize.sizeDelta.y/2;
        // int cardsPerRow = (int) ((boxArea.width - horizontalCardPadding * 2)/(cardSize.sizeDelta.x + intracardXPadding));
        // Debug.Log($"Cards Per Row : {cardsPerRow}");
        // int totalSteps = (int) (((boxArea.height - verticalCardPadding * 2) - (horiPosition)) / ((cardSize.sizeDelta.y + intracardYPadding) * (cards.Count / cardsPerRow)));
        // Debug.Log($"Total Steps : {totalSteps}");

        float boxHeight = boxArea.height - verticalCardPadding * 2;
        float cardHeight = cardSize.sizeDelta.y + intracardYPadding;

        int cardsPerRow = (int)((boxArea.width - horizontalCardPadding * 2) / (cardSize.sizeDelta.x + intracardXPadding));
        int cardsPerColumn = Mathf.FloorToInt(boxHeight / cardHeight);

        Debug.Log($"Cards Per Row: {cardsPerRow}");
        Debug.Log($"Cards Per Column: {cardsPerColumn}");

        int totalCards = cardsPerRow * cardsPerColumn;
        Debug.Log($"Total Cards: {totalCards}");

    }
    

    public void updateSearchBox()
    {
        float scaleVal = 1;
        float startPos = boxArea.width/-2 + horizontalCardPadding + cardSize.sizeDelta.x/2;
        float addAmount = cardSize.sizeDelta.x;
        float horiPosition = boxArea.height/2 - verticalCardPadding - cardSize.sizeDelta.y/2;
        float iterPos = startPos;
        for(int i =0 ; i < cards.Count; ++i)
        {
            GameObject currentCard = cards[i].gameObject;            
            RectTransform rt = currentCard.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(iterPos,horiPosition);
            Debug.Log($"{startPos}, {boxArea.width/2}, {cardSize.sizeDelta.x}");
            
            currentCard.transform.localScale = new Vector3(scaleVal,scaleVal,scaleVal);
            if (iterPos + addAmount  + horizontalCardPadding> boxArea.width/2)
            {
                horiPosition -= cardSize.sizeDelta.y + intracardYPadding;
                iterPos = startPos;
                continue;
            }
            iterPos += addAmount + intracardXPadding;

        }
    }
}
