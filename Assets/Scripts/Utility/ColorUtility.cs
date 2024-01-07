using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ImageProcessing.Extensions;

namespace ImageProcessing.Utility
{
//Source: https://stackoverflow.com/questions/27374550/how-to-compare-color-object-and-get-closest-color-in-an-color  answear by TaW

    public static class ColorUtility
    {
// closed match for hues only:
        public static int ClosestColorByHue(List<Color> colors, Color target)
        {
            var hue1 = target.GetHue();
            var diffs = colors.Select(n => GetHueDistance(n.GetHue(), hue1));
            var diffMin = diffs.Min(n => n);
            return diffs.ToList().FindIndex(n => n == diffMin);
        }

// closed match in RGB space
        public static int ClosestColorByRGB(List<Color> colors, Color target)
        {
            var colorDiffs = colors.Select(n => ColorDiff(n, target)).Min(n => n);
            return colors.FindIndex(n => ColorDiff(n, target) == colorDiffs);
        }

// weighed distance using hue, saturation and brightness
        public static int ClosestColorByHSV(List<Color> colors, Color target, float factorSaturation,
            float factorBrightness)
        {
            float hue1 = target.GetHue();
            var num1 = ColorNum(target, factorSaturation, factorBrightness);
            var diffs = colors.Select(n => Math.Abs(ColorNum(n, factorSaturation, factorBrightness) - num1) +
                                           GetHueDistance(n.GetHue(), hue1));
            var diffMin = diffs.Min(x => x);
            return diffs.ToList().FindIndex(n => n == diffMin);
        }

        // color brightness as perceived:
        private static float GetBrightness(Color c)
        {
            return (c.r * 0.299f + c.g * 0.587f + c.b * 0.114f) / 256f;
        }

// distance between two hues:
        private static float GetHueDistance(float hue1, float hue2)
        {
            float d = Math.Abs(hue1 - hue2);
            return d > 180 ? 360 - d : d;
        }

//  weighed only by saturation and brightness (from my trackbars)
        private static float ColorNum(Color c, float factorSat, float factorBri)
        {
            return c.GetSaturation() * factorSat + GetBrightness(c) * factorBri;
        }

// distance in RGB space
        private static int ColorDiff(Color c1, Color c2)
        {
            return (int)Math.Sqrt((c1.r - c2.r) * (c1.r - c2.r)
                                  + (c1.g - c2.g) * (c1.g - c2.g)
                                  + (c1.b - c2.b) * (c1.b - c2.b));
        }
    }
}