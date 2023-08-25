using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface cardBox {}
public class cardMover : MonoBehaviour
{
    struct heldCardInfo
    {
        public GameObject card;
        public GameObject onField;
        public int previousHandIndex;
        public cardHolder lastPosition;
        public bool fromField;
    }

    List<cardBox> hands = new List<cardBox>();
    Dictionary<cardBox, int> priority = new Dictionary<cardBox, int>();


    public bool holdingCard()
    {
        return holdingCard != null;
    }

    void orderHand()
    {
        hands = priority.Keys;
        hands.Sort((obj1, obj2) => priority[obj1].CompareTo(priority[obj2]));
    }

    void mouseInBox()
    {
        for(int i = 0; i < hands.Count; ++i)
        {
            if(hands[i].inBoundingBox())
            {
                
            }
        }
    }

    public void addCardBox(cardBox box, int priority)
    {
        priority[box] = priority;
        orderHand();
    }
}
