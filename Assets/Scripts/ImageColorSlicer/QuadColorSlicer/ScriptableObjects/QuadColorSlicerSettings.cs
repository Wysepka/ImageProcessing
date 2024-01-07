using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum ColorComparerType
{
    RGB,
    Hue,
    HSV
}

namespace ImageProcessing.ImgColSlicer.Quad
{
    [CreateAssetMenu(menuName = "QuadSlicer/QuadSlicerSettings", fileName = "QuadSlicerSettings")]
    public class QuadColorSlicerSettings : ScriptableObject
    {
        [Header("Processing Settings")] [SerializeField]
        private bool _isBlackAndWhite;

        [SerializeField, Range(0f, 1f)] private float _blackWhiteCutTreeshold;
        [SerializeField, Min(1)] private int colorSlicingSpace;
        [SerializeField] private Color[] dominantColors;

        [FormerlySerializedAs("colorComparer")] [SerializeField]
        private ColorComparerType colorComparerType;

        [SerializeField] private float saturationFactor;
        [SerializeField] private float brightnessFactor;

        [Header("Slicing Settings")] [SerializeField]
        private int pixelSlicingSquareDim = 25;

        private float slicedSquareSpace = 1f;


        public bool IsBlackAndWhite => _isBlackAndWhite;
        public float BlackWhiteCutTreeshold => _blackWhiteCutTreeshold;
        public int ColorSlicingSpace => colorSlicingSpace;
        public Color[] DominantColors => dominantColors;
        public ColorComparerType ColorComparerType => colorComparerType;
        public float SaturationFactor => saturationFactor;
        public float BrightnessFactor => brightnessFactor;

        public int PixelSlicingSquareDim => pixelSlicingSquareDim;
        public float SlicedSquareSpace => slicedSquareSpace;
    }
}