using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroller : MonoBehaviour
{
    public Vector2 CurrentOffset;
    public Vector2 TextureSize;
    public Vector2 PixelSize = new Vector2(0,0);
    public Vector2 UVPostition = new Vector2(0, 0);


    public void SetTexture(float cellType, float Humidity, Material material, bool isWater)
    {
        TextureSize = new Vector2(material.GetTexture("_MainTex").width, material.GetTexture("_MainTex").height);
        
        PixelSize.y = 1 / TextureSize.y;
        PixelSize.x = 1 / TextureSize.x;

        if (!isWater)
            Humidity = Mathf.Clamp(Humidity * 1.33f,0,1);

        // Refrence for the rounding
        //=ROUND(A1/0.125,0)*0.125
        //--------------------------

        Humidity = ((Mathf.Round(((Humidity * (TextureSize.x - 1)) * PixelSize.x) / 0.0125f) * 0.0125f) - 1) * -1;

        cellType = -1 - cellType;
        cellType = Mathf.Round((cellType * PixelSize.y) / 0.125f) * 0.125f;

        UVPostition = new Vector2(Humidity, cellType);

        CurrentOffset = (UVPostition);
        material.SetTextureOffset("_MainTex", CurrentOffset);
    }
}
