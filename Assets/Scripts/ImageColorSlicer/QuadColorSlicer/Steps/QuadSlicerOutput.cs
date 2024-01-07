using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ImageProcessing.ImgColSlicer.Quad
{
    public struct QuadSlicerOutput
    {
        [FormerlySerializedAs("blackWhiteTexture")] public Texture2D processedTexture;
    }
}