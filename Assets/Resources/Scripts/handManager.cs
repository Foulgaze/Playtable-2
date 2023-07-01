using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class handManager : MonoBehaviour
{
    public GameObject handHolder;
    public GameObject card;

    heldCardInfo heldCard;
    Image img;
    Vector3 handCenter;
    List<GameObject> hand;
    List<float> cardCoordinates;
    public List<Sprite> tempSprites;
    int lastHighlightedCard = -1;
    float mouseOffset; 

    Rect handArea;
    Rect cardArea;

    struct heldCardInfo
    {
        public int previousHandIndex;
        public GameObject card;
    }

    bool inHandBox;
    // Start is called before the first frame update
    void Start()
    {
        img = handHolder.GetComponent<Image>();
        handCenter = transform.InverseTransformPoint (img.transform.position);
        hand = new List<GameObject>();
        cardCoordinates = new List<float>();
        handArea = RectTransformUtility.PixelAdjustRect(handHolder.transform.GetComponent<RectTransform>(),transform.GetComponent<Canvas>());
        card.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(handArea.height*0.7f, handArea.height);
        cardArea = RectTransformUtility.PixelAdjustRect(card.transform.GetComponent<RectTransform>(),transform.GetComponent<Canvas>());
        handArea.width -= cardArea.width;
        mouseOffset += handArea.width;
        inHandBox = mouseInHand();
        heldCard.card = null;
    }

    void addCardToHand()
    {
        GameObject newCard = GameObject.Instantiate(card);
        newCard.transform.SetParent(handHolder.transform);
        newCard.transform.GetComponent<Image>().color = new Vector4(255,255,255,1);
        // newCard.transform.
        newCard.transform.GetComponent<Image>().sprite = tempSprites[UnityEngine.Random.Range(0,tempSprites.Count)];
        hand.Add(newCard);
    }
    public void updateHand()
    {
        cardCoordinates.Clear();
        float horiPadding = 0;
        float startPos = handArea.width/-2 + handArea.width * horiPadding;
        float scaleVal = 1;
        RectTransform cardSize = card.transform.GetComponent<RectTransform>();
        float addAmount = Mathf.Min(cardSize.sizeDelta.x * cardSize.localScale.x,(handArea.width*(1-horiPadding*2)/(Math.Max(1,hand.Count-1))));
        float horiPosition = 0;

        for(int i =0 ; i < hand.Count; ++i)
        {
            RectTransform rt = hand[i].GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(startPos,horiPosition);
            hand[i].transform.localScale = new Vector3(scaleVal,scaleVal,scaleVal);
            hand[i].name = $"{i}";
            hand[i].transform.SetSiblingIndex(i);

            cardCoordinates.Add(startPos - cardArea.width/4);
            startPos += addAmount;

        }
        lastHighlightedCard = -1;
    }

    int binarySearch(List<float> nums, float target)
    {
        int left = 0;
        int right = nums.Count - 1;
        while (left < right)
        {
            int mid = (left + right) / 2;
            if (nums[mid] < target)
            {
                left = mid + 1;
            }
            else
            {
                right = mid;
            }
        }
        // At this point, left == right and we have found the index of the closest value.
        float closest = nums[left];
        if (left > 0 && Math.Abs(nums[left - 1] - target) < Math.Abs(closest - target))
        {
            return left - 1;
        }
        if (left < nums.Count - 1 && Math.Abs(nums[left + 1] - target) < Math.Abs(closest - target))
        {
            return left + 1;
        }
        return left;
    }

    bool mouseInHand()
    {
        float adjustedXPosition = Input.mousePosition.x - Screen.width/2;
        bool inXBox = adjustedXPosition > handArea.position.x && adjustedXPosition < Math.Abs(handArea.position.x);
        bool inYBox = Input.mousePosition.y < handArea.height;
        return inXBox && inYBox;
    }

    public bool holdingCard()
    {
        return heldCard.card != null;
    }
    void releaseCard()
    {
        if(!Input.GetMouseButton(0) && holdingCard())
        {
            int insertPosition = heldCard.previousHandIndex;
            if(mouseInHand())
            {
                float mousePos = Input.mousePosition.x - Screen.width/2;
                insertPosition = binarySearch(cardCoordinates,mousePos);
                if(insertPosition == cardCoordinates.Count-1)
                {
                    if(mousePos > cardCoordinates[insertPosition])
                    {
                        insertPosition = cardCoordinates.Count;
                    }
                }
            }
            hand.Insert(insertPosition, heldCard.card);
            updateHand();
            heldCard.card = null;
        }
    }
    void scanForHoverCard()
    {
        if(!mouseInHand())
        {
            if(inHandBox)
            {
                inHandBox = false;
                updateHand();
            }
            return;
        }
        if (hand.Count == 0)
        {
            return;
        }
        int closestCardIndex = binarySearch(cardCoordinates,Input.mousePosition.x - Screen.width/2);
        if(closestCardIndex != lastHighlightedCard)
        {
            updateHand();
            hand[closestCardIndex].transform.SetAsLastSibling();
        }


        if(Input.GetMouseButtonDown(0) && !holdingCard())
        {
            heldCard.card = hand[closestCardIndex];
            heldCard.previousHandIndex = closestCardIndex;
            int removeIndex = hand.IndexOf(heldCard.card);
            hand.RemoveAt(removeIndex);
            cardCoordinates.RemoveAt(removeIndex);
            // hand.Remove(heldCard.card);
        }
        inHandBox = true;

    }

    void updateHeldCard()
    {
        if(holdingCard())
        {
            heldCard.card.transform.position = Input.mousePosition;
            heldCard.card.transform.SetAsLastSibling();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            addCardToHand();
            updateHand();
        }
        
        scanForHoverCard();
        updateHeldCard();
        releaseCard();
        // Debug.Log(mouseInHand());
    }
}
