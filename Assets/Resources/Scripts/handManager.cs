using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class handManager : MonoBehaviour
{
    public GameObject handHolder;
    public GameObject cardHolder;
    public GameObject card;
    public GameObject cardOnField;
    public boardController bc;
    public textureLoader tl;
    heldCardInfo heldCard;
    Image img;
    Vector3 handCenter;
    List<GameObject> hand;
    List<float> cardCoordinates;
    public List<Sprite> tempSprites;
    string[] cardNames = {"Madcap Skills", "Swamp", "Jabari's Banner", "Lobotomy"};
    int lastHighlightedCard = -1;
    float mouseOffset; 
    Vector3 cardDimensions;
    Vector3 cardHolderDimensions;


    Rect handArea;
    Rect cardArea;

    struct heldCardInfo
    {
        public GameObject card;
        public GameObject onField;
        public int previousHandIndex;
        public cardHolder lastPosition;
        public bool fromField;
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
        cardDimensions = cardOnField.transform.GetComponent<MeshRenderer>().bounds.extents;
        cardHolderDimensions = cardHolder.transform.GetComponent<MeshRenderer>().bounds.extents;
    }

    void addCardToHand()
    {
        GameObject cardParent = new GameObject();
        
        string randomCard = cardNames[UnityEngine.Random.Range(0, cardNames.Length)];
        GameObject cardSprite = GameObject.Instantiate(card);
        cardSprite.transform.SetParent(cardParent.transform);
        cardSprite.name = "hand";

        GameObject cardModel = GameObject.Instantiate(cardOnField);
        // Material randColor = new Material(Shader.Find("Standard"));
        // randColor.color = UnityEngine.Random.ColorHSV(); 
        // cardModel.GetComponent<Renderer>().material = randColor;

        tl.setCardTextures(randomCard,cardSprite,cardModel);
        cardModel.transform.SetParent(cardParent.transform);
        cardModel.name = "field";

        
        
        cardParent.transform.SetParent(handHolder.transform);
        cardParent.transform.position = handHolder.transform.position;
        cardParent.name = "cardParent";

        cardModel.SetActive(false);
        
        hand.Add(cardParent);
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
            GameObject cardParent = hand[i];
            GameObject currentCard = cardParent.transform.GetChild(0).gameObject;
            
            RectTransform rt = currentCard.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(startPos,horiPosition);
            currentCard.transform.localScale = new Vector3(scaleVal,scaleVal,scaleVal);
            cardParent.name = $"Position {i}";
            cardParent.transform.SetSiblingIndex(i);

            cardCoordinates.Add(startPos - cardArea.width/4);
            startPos += addAmount;

        }
        lastHighlightedCard = -1;
    }

    int binarySearch(List<float> nums, float target)
    {
        if (hand.Count == 0)
        {
            return 0;
        }
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
            // int insertPosition = heldCard.previousHandIndex;
            if(mouseInHand())
            {
                float mousePos = Input.mousePosition.x - Screen.width/2;
                int insertPosition = binarySearch(cardCoordinates,mousePos);
                if(insertPosition == cardCoordinates.Count-1)
                {
                    if(mousePos > cardCoordinates[insertPosition])
                    {
                        insertPosition = cardCoordinates.Count;
                    }
                }
                hand.Insert(insertPosition, heldCard.card.transform.parent.gameObject);
                updateHand();
            }
            else
            {
                heldCard.lastPosition.addCard(heldCard.onField);
                heldCard.onField.GetComponent<cardBehavior>().parent = heldCard.lastPosition;
            }
            
            heldCard.card = null;
        }
    }

    void switchCardState()
    {

    }

    void loadHandState(GameObject card, bool onField)
    {
        if (onField)
        {
            heldCard.onField = card;
            heldCard.card = card.transform.parent.GetChild(0).gameObject;
            heldCard.card.SetActive(false);
        }
        else
        {
            heldCard.onField = card.transform.parent.GetChild(1).gameObject;
            heldCard.card = card;
            heldCard.onField.SetActive(false);
            // heldCard.previousHandIndex = 0;
        }
        
    }
    void scanForHoverCard()
    {
        if(!mouseInHand()) // Fixes card hover when leaving hand box
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
            loadHandState(hand[closestCardIndex].transform.GetChild(0).gameObject,false);
            // heldCard.previousHandIndex = closestCardIndex;
            int removeIndex = hand.IndexOf(heldCard.card.transform.parent.gameObject);
            hand.RemoveAt(removeIndex);
            cardCoordinates.RemoveAt(removeIndex);
        }
        inHandBox = true;

    }


    void scanForOnFieldCard()
    {
        if(Input.GetMouseButtonDown(0) && !holdingCard() && !mouseInHand())
        {
            (GameObject closestCard, cardHolder closestHolder) = bc.mouseOverClosestCard();
            if (closestCard != null)
            {
                loadHandState(closestCard,true);
                closestHolder.removeCard(closestCard);
            }
        }
    }

    void updateHeldCard()
    {
        if(holdingCard())
        {
            if(mouseInHand())
            {
                heldCard.card.transform.position = Input.mousePosition;
                heldCard.card.transform.parent.transform.SetAsLastSibling();
                heldCard.onField.SetActive(false);
                heldCard.card.SetActive(true);

            }
            else
            {
                heldCard.onField.SetActive(true);
                heldCard.card.SetActive(false);

                cardHolder closestCard = bc.getClosestCardHolder();
                if (closestCard == null)
                {
                    Debug.LogError("Error, can't find closest card.");
                    return;
                }
                Vector3 position = closestCard.getNextPosition(heldCard.onField);
                heldCard.onField.transform.position = position;
                heldCard.lastPosition = closestCard;

            }
        }
    }

    void checkForOnFieldCard()
    {
        if(!(holdingCard() && !mouseInHand()))
        {
            return;
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
        checkForOnFieldCard();
        scanForHoverCard();
        updateHeldCard();
        releaseCard();
        scanForOnFieldCard();
    }
}
