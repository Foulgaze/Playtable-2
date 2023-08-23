using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class displayCardHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cardPrefab;
    public GameObject cardHolder;
    public Scrollbar scrollbar; 
    public TMP_InputField  searchbar;
    public List<Sprite> temporarySprites;
    List<GameObject> cards;
    Rect boxArea;
    RectTransform cardSize;
    public int cardCount = 100;



    public float horizontalCardPadding = 4;
    public float verticalCardPadding = 4;
    public float intracardXPadding = 1;
    public float intracardYPadding =1 ;

    Vector2 cardDimensions;

    int cardsPerRow = 4;
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

        searchbar.onValueChanged.AddListener(updateSearchBox);
        boxArea = RectTransformUtility.PixelAdjustRect(transform.GetComponent<RectTransform>(),GetComponent<Canvas>());
        setCardDimensions();
        setScrollValues(cards);
        updateSearchBox("");
    }


    void setCardDimensions()
    {
        float horizontalCardSize = (boxArea.width - (horizontalCardPadding * 2) - (intracardXPadding * (cardsPerRow - 1)))/(cardsPerRow);
        float verticalCardSize = horizontalCardSize * (3.5f/2.5f);

        cardDimensions = new Vector2(horizontalCardSize,verticalCardSize );
    }


    void setScrollValues(List<GameObject> subset)
    {
        int verticalCardFit = (int) ((boxArea.height - verticalCardPadding * 2)/(cardDimensions.y));
        int yValues = (int) Mathf.Ceil(subset.Count / (float) cardsPerRow);
        Debug.Log($"Vertical Card Fit : {verticalCardFit}, yValues : {yValues}");
        int totalSteps = yValues - verticalCardFit;
        scrollbar.numberOfSteps = totalSteps + 1;
        scrollbar.size = 1f/(totalSteps + 1);

    }

    int getCurrentStep()
    {
        // Calculate the step size based on the number of steps
        float stepSize = 1.0f / (scrollbar.numberOfSteps - 1);

        // Calculate the current step based on the scrollbar's value
        int currentStep = Mathf.RoundToInt(scrollbar.value / stepSize);

        return (int) Mathf.Max(Mathf.Clamp(currentStep, 0, scrollbar.numberOfSteps - 1),0f);
    }
    

    public void updateSearchBox(string data)
    {

        List<GameObject> sub = new List<GameObject>(cards);
    
        int cardIter = 0;
        while (cardIter < sub.Count)
        {
            GameObject currCard = sub[cardIter];
            if (!currCard.name.Contains(searchbar.text))
            {
                currCard.SetActive(false);
                sub.RemoveAt(cardIter);
                continue;
            }
            currCard.SetActive(true);
            cardIter++;
        }
        setScrollValues(sub);
        
        
        float xStart = boxArea.width/-2 + horizontalCardPadding + cardDimensions.x/2;
     
        float xIterPos = xStart;
        float yIterPos = boxArea.height/2 - verticalCardPadding - cardDimensions.y/2;
        yIterPos += (cardDimensions.y + intracardYPadding) * getCurrentStep();
        Debug.Log($"Start Pos : {xStart}, Y Start : {yIterPos} Step : {getCurrentStep()}");
        for(int i =0 ; i < sub.Count; ++i)
        {
            if (i != 0 && i % cardsPerRow == 0)
            {
                yIterPos -=  cardDimensions.y + intracardYPadding;
                xIterPos = xStart;
            }

            GameObject currentCard = sub[i].gameObject;            
            RectTransform rt = currentCard.GetComponent<RectTransform>();
            rt.sizeDelta = cardDimensions;
            rt.anchoredPosition = new Vector2(xIterPos,yIterPos);
            
            xIterPos += cardDimensions.x + intracardXPadding;

        }
    }
}
