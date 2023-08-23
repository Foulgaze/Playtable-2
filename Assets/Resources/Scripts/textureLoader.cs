using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class textureLoader : MonoBehaviour
{
    public Texture2D cardback;
    Vector2 cardDimensions = new Vector2(488,680);
    public void setCardTextures(string cardName, GameObject inHandCard, GameObject cardOnField)
    {

        string filepath = $"Assets/Resources/Textures/{cardName}.jpg";

        if (File.Exists(filepath))
        {
            Texture2D texture = new Texture2D(2, 2); // You can specify the initial size or resize it later
            byte[] imageBytes = System.IO.File.ReadAllBytes(filepath);
            // texture.filterMode = FilterMode.Point;
            bool success = texture.LoadImage(imageBytes);

            if (success)
            {
                 // Create a new Sprite using the Texture2D
                setSprite(texture,inHandCard);
                generateTexture(texture, cardOnField);
                return;
            }
        }

        // Else Retrieve
        string url = $"https://api.scryfall.com/cards/named?exact={cardName}&format=image&version=normal";
        StartCoroutine(getTexture(url,inHandCard,cardOnField, filepath));
    }

    IEnumerator getTexture(string url, GameObject inHandCard,GameObject cardOnField, string savepath)
    {
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(url);
        yield return textureRequest.SendWebRequest();

        if (textureRequest.result != UnityWebRequest.Result.Success )
        {
            Debug.Log($"{url} Error"); // LOG ERRORS
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
            // myTexture.filterMode = FilterMode.Point;
            byte[] pngBytes = myTexture.EncodeToPNG();
            File.WriteAllBytes(savepath, pngBytes);
            setSprite(myTexture, inHandCard);
            generateTexture(myTexture, cardOnField);
        }
    }

    void setSprite(Texture2D texture, GameObject go)
    {
        Sprite convertedSprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f) 
        );
        Image img = go.transform.GetComponent<Image>();
        img.color = new Vector4(255,255,255,1);
        img.sprite = convertedSprite;
    }

    void generateTexture(Texture2D myTexture, GameObject go) 
    {
        Texture2D atlas = new Texture2D(myTexture.width *2, myTexture.height);
        atlas.PackTextures(new Texture2D[] { myTexture, cardback },0);
        go.transform.GetComponent<Renderer>().material.mainTexture = atlas;
        byte[] pngBytes = atlas.EncodeToPNG();
        File.WriteAllBytes("Assets/Resources/Textures/atlas.png", pngBytes);
        applyTexture(go);
    }

    void applyTexture(GameObject go) 
    {
        Mesh mesh = go.GetComponent<MeshFilter>().mesh;
        Vector2[] UVs = new Vector2[mesh.vertices.Length];
        float xPercent = cardDimensions.x/1024;
        float yPercent = cardDimensions.y/1024;
        // Front
        UVs[0] = new Vector2(0.8f, 0.8f);
        UVs[1] = new Vector2(0.8f, 0.8f);
        UVs[2] = new Vector2(0.8f, 0.8f);
        UVs[3] = new Vector2(0.8f, 0.8f);
        
        // Top
        UVs[4] = new Vector2(xPercent, 0.0f);
        UVs[5] = new Vector2(0.0f, 0.0f);
        UVs[8] = new Vector2(xPercent, yPercent);
        UVs[9] = new Vector2(0.0f, yPercent);
        
        // Back
        UVs[7] = new Vector2(0.8f, 0.8f);
        UVs[6] = new Vector2(0.8f, 0.8f);
        UVs[11] = new Vector2(0.8f, 0.8f);
        UVs[10] = new Vector2(0.8f, 0.8f);
        
        // Bottom
        UVs[12] = new Vector2(xPercent , 0.0f);
        UVs[13] = new Vector2(xPercent , yPercent);
        UVs[14] = new Vector2(xPercent*2, yPercent);
        UVs[15] = new Vector2(xPercent*2, 0.0f);

        
        // Left
        UVs[16] = new Vector2(0.8f, 0.8f);
        UVs[17] = new Vector2(0.8f, 0.8f);
        UVs[18] = new Vector2(0.8f, 0.8f);
        UVs[19] = new Vector2(0.8f, 0.8f);
        // Right        
        UVs[20] = new Vector2(0.8f, 0.8f);
        UVs[21] = new Vector2(0.8f, 0.8f);
        UVs[22] = new Vector2(0.8f, 0.8f);
        UVs[23] = new Vector2(0.8f, 0.8f);
        mesh.uv = UVs;
    }
}