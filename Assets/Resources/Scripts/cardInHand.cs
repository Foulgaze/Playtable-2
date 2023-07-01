using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cardInHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public handManager hand;
    public void OnPointerEnter(PointerEventData eventData)
    {
        // transform.SetAsLastSibling();
        // Debug.Log($"Enter {transform.name}");
        GameObject lowestUIElement = eventData.pointerCurrentRaycast.gameObject;

        // Keep checking parent objects until we find the lowest one
        while (lowestUIElement.GetComponentInParent<Canvas>() != null)
        {
            lowestUIElement = lowestUIElement.transform.parent.gameObject;
        }
        lowestUIElement.transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Exit {transform.name}");
        hand.updateHand();
    }
}
