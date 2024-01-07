using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ImageProcessing.ImgColSlicer.Quad
{
    [CreateAssetMenu(menuName = "QuadSlicer/QuadSlicerData", fileName = "QuadSlicerData")]
    public class QuadColorSlicerData : ScriptableObject
    {
        [SerializeField] private Texture _blackWhiteTexture;
        [SerializeField] private Texture2D _normalTexture;

        public Texture BlackWhiteTexture => _blackWhiteTexture;
        public Texture2D NormalTexture => _normalTexture;
    }
}