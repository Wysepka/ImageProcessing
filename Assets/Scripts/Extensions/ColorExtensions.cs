using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageProcessing.Extensions
{
    public static class ColorExtensions
    {
        public static float GetHue(this Color color)
        {
            Color.RGBToHSV(color, out float hue, out float saturation, out float brightness);
            return hue;
        }

        public static float GetSaturation(this Color color)
        {
            Color.RGBToHSV(color, out float hue, out float saturation, out float brightness);
            return saturation;
        }
    }
}