using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class tableGenerator : MonoBehaviour
{
    public GameObject cardHolderPrefab; 
    public GameObject card;
    public handManager hm;
    Vector3 cardHolderDimensions;
    int cardsPerRow = 10;
    int horiDistanceBetweenCards = 1;
    int boardCount = 0;

    int rowCount = 3;
    int vertDistanceBetweenCards = 1;
    void Start()
    {
        cardHolderDimensions = cardHolderPrefab.transform.GetComponent<MeshRenderer>().bounds.extents;
        generateBoard();
    }

    void generateBoard()
    {
        int cameraStartHeight = 30;
        int cameraBackDistance = 7;
        int cameraAngle = 70;
        GameObject boardCombination = new GameObject();
        GameObject boardParent = new GameObject();
        GameObject boardCamera = new GameObject();
        boardCombination.transform.name = "Board Combination #" + boardCount;
        boardParent.transform.name = "Board #" + boardCount;
        boardCamera.transform.name = "Camera #" + boardCount;
        Vector3 startPosition = Vector3.zero;
        boardController bc = boardParent.AddComponent<boardController>();
        hm.bc = bc;
        bc.cardsPerRow = cardsPerRow;
        bc.cardDimensions = card.transform.GetComponent<MeshRenderer>().bounds.extents;

        for(int j = 0; j < rowCount; ++j)
        {
            for(int i = 0; i < cardsPerRow; ++i)
            {
                GameObject cardHolder = GameObject.Instantiate(cardHolderPrefab,startPosition, Quaternion.identity);
                if(i != cardsPerRow - 1) // Don't update card position for last item in loop. Would set middle incorrectly
                {
                    startPosition += new Vector3(cardHolderDimensions.x*2 + horiDistanceBetweenCards,0,0);
                }
                cardHolder.transform.name = "(" + i + "," + j + ")";
                cardHolder.transform.parent = boardParent.transform;
                cardHolder cardHolderScript = cardHolder.AddComponent<cardHolder>();
                cardHolderScript.setValues(0.6f,0.2f, card.transform.GetComponent<MeshRenderer>().bounds.extents);
                bc.addCard(cardHolderScript);
            }
            if(j != rowCount - 1) // Don't update card position for last item in loop. Would set middle incorrectly
            {
                startPosition.x =0;
                startPosition -= new Vector3(0,0,cardHolderDimensions.z*2 + vertDistanceBetweenCards);
            }
        }
        Vector3 middlePosition = new Vector3(startPosition.x/2f, startPosition.y + cameraStartHeight,startPosition.z/2f);
        // boardParent.transform.position = middlePosition;
        boardCamera.transform.position = middlePosition;
        boardCamera.transform.position -= new Vector3(0,0,cameraBackDistance);
        boardCamera.transform.Rotate(new Vector3(cameraAngle,0,0));
        boardCamera.AddComponent<Camera>();

        boardCamera.transform.parent = boardCombination.transform;
        boardParent.transform.parent = boardCombination.transform;
        
        if(boardCount == 0)
        {
            boardCamera.tag = "MainCamera";   
            boardCamera.AddComponent<cameraMovement>();

        }
        boardCount++;
        
            
    }

    void getMiddlePosition(GameObject board)
    {
        if(board.transform.childCount <= 0)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
