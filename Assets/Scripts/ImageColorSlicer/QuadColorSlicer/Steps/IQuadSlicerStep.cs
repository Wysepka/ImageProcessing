using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageProcessing.ImgColSlicer.Quad
{
    public interface IQuadSlicerStep
    {
        QuadSlicerOutput ProcessTexture(QuadColorSlicerData data, QuadColorSlicerSettings settings);
    }
}