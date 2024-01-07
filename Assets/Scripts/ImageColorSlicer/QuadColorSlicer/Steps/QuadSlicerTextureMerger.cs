using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageProcessing.ImgColSlicer.Quad
{
    public class QuadSlicerTextureMerger
    {
        public QuadSlicerOutput MergeTextures(Texture2D blackWhiteTexture, QuadColorSlicerData data,
            QuadColorSlicerSettings settings)
        {
            QuadSlicerOutput output = new QuadSlicerOutput();

            int widthBlackWhite = blackWhiteTexture.width;
            int widthColor = data.NormalTexture.width;

            int heightBlackWhite = blackWhiteTexture.height;
            int heightColor = data.NormalTexture.height;

            if (widthColor != widthBlackWhite)
            {
                Debug.LogError("Width of textures are different");
                return output;
            }

            if (heightColor != heightBlackWhite)
            {
                Debug.LogError("Heights of textures are different");
                return output;
            }

            Texture2D processedTexture = new Texture2D(widthColor, heightColor);

            for (int i = 0; i < widthBlackWhite; i++)
            {
                for (int j = 0; j < heightBlackWhite; j++)
                {
                    Color blackWhitePixel = blackWhiteTexture.GetPixel(i, j);

                    if (blackWhitePixel == Color.black)
                    {
                        processedTexture.SetPixel(i, j, Color.black);
                    }
                    else
                    {
                        processedTexture.SetPixel(i, j, data.NormalTexture.GetPixel(i, j));
                    }
                }
            }

            processedTexture.Apply();
            output.processedTexture = processedTexture;
            output.processedTexture.Apply();

            return output;
        }
    }
}