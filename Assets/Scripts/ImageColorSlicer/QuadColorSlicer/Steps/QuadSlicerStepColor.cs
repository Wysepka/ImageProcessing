using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ColorUtility = ImageProcessing.Utility.ColorUtility;

namespace ImageProcessing.ImgColSlicer.Quad
{
    public class QuadSlicerStepColor : IQuadSlicerStep
    {
        public QuadSlicerOutput ProcessTexture(QuadColorSlicerData data, QuadColorSlicerSettings settings)
        {
            QuadSlicerOutput output = new QuadSlicerOutput();
            int width = data.NormalTexture.width;
            int height = data.NormalTexture.height;
            Color[] pixels = data.NormalTexture.GetPixels();
            int space = settings.ColorSlicingSpace;
            Color[] processedPixels = new Color[width * height];

            for (int i = 0; i < width; i += space)
            {
                for (int j = 0; j < height; j += space)
                {
                    Color averagePixelColor = ReturnAveragePixelColor(pixels, i, width, j, height, space);
                    Color dominantColor = GetClosestDominantColor(averagePixelColor, settings.DominantColors,
                        settings.ColorComparerType, settings.SaturationFactor, settings.BrightnessFactor);

                    FillQuadWithColor(ref processedPixels, dominantColor, i, width, j, height, space);
                }
            }

            Texture2D processedTexture = new Texture2D(width, height);
            processedTexture.SetPixels(processedPixels);
            processedTexture.Apply();
            output.processedTexture = processedTexture;

            return output;
        }

        private Color ReturnAveragePixelColor(Color[] pixels, int i, int width, int j, int height, int space)
        {
            Vector4 colorSum = Vector4.zero;
            Color closestColor = new Color();
            int widthToLoop = space;
            int heightToLoop = space;

            if (i + space > width)
            {
                widthToLoop = (i + space) - width - 1;
            }

            if (j + space > height)
            {
                heightToLoop = (j + space) - height - 1;
            }

            for (int k = 0; k < widthToLoop; k++)
            {
                for (int l = 0; l < heightToLoop; l++)
                {
                    try
                    {
                        var color = pixels[(i + k) * width + j + l];
                        colorSum += new Vector4(color.r, color.g, color.b, color.a);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }

            Vector4 colorSumDivided = colorSum / (widthToLoop * heightToLoop);

            return new Color(colorSumDivided.x, colorSumDivided.y, colorSumDivided.z, colorSumDivided.w);
        }

        private Color GetClosestDominantColor(Color pixel, Color[] dominantColors, ColorComparerType comparerTypeType,
            float factorSaturation, float factorBrightness)
        {
            List<Color> dominantColorsList = dominantColors.ToList();
            switch (comparerTypeType)
            {
                case ColorComparerType.RGB:
                    return dominantColorsList[ColorUtility.ClosestColorByRGB(dominantColorsList, pixel)];
                case ColorComparerType.Hue:
                    return dominantColorsList[ColorUtility.ClosestColorByHue(dominantColorsList, pixel)];
                case ColorComparerType.HSV:
                    return dominantColorsList[
                        ColorUtility.ClosestColorByHSV(dominantColorsList, pixel, factorSaturation, factorBrightness)];
                default:
                    return dominantColorsList[ColorUtility.ClosestColorByRGB(dominantColorsList, pixel)];
            }
        }

        private void FillQuadWithColor(ref Color[] pixels, Color colorToFill, int i, int width, int j, int height,
            int space)
        {
            Vector4 colorSum = Vector4.zero;
            Color closestColor = new Color();
            int widthToLoop = space;
            int heightToLoop = space;

            if (i + space > width)
            {
                widthToLoop = (i + space) - width - 1;
            }

            if (j + space > height)
            {
                heightToLoop = (j + space) - height - 1;
            }

            for (int k = 0; k < widthToLoop; k++)
            {
                for (int l = 0; l < heightToLoop; l++)
                {
                    try
                    {
                        pixels[(i + k) * width + j + l] = colorToFill;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }

    }
}