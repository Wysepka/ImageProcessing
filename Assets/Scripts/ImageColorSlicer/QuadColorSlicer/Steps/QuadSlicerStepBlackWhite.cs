using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageProcessing.ImgColSlicer.Quad
{
    public class QuadSlicerStepBlackWhite : IQuadSlicerStep
    {
        public QuadSlicerOutput ProcessTexture(QuadColorSlicerData data, QuadColorSlicerSettings settings)
        {
            QuadSlicerOutput output = new QuadSlicerOutput();

            int width = data.NormalTexture.width;
            int height = data.NormalTexture.height;

            Texture2D blackWhiteTexture = new Texture2D(width, height);
            for (int i = 0; i < data.NormalTexture.width; i++)
            {
                for (int j = 0; j < data.NormalTexture.height; j++)
                {
                    Color pixelColor = data.NormalTexture.GetPixel(i, j);
                    //float colorSum =
                    if (pixelColor.r > settings.BlackWhiteCutTreeshold ||
                        pixelColor.g > settings.BlackWhiteCutTreeshold ||
                        pixelColor.b > settings.BlackWhiteCutTreeshold)
                    {
                        blackWhiteTexture.SetPixel(i, j, Color.white);
                    }
                    else
                    {
                        blackWhiteTexture.SetPixel(i, j, Color.black);
                    }

                }
            }

            blackWhiteTexture.Apply();
            output.processedTexture = blackWhiteTexture;
            output.processedTexture.Apply();

            return output;
        }
    }
}