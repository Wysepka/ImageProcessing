using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ImageProcessing.ImgColSlicer.Quad;
using ImageProcessing.ImgColSlicer.QuadVis;

namespace ImageProcessing.ImgColSlicer.UI
{
    public class QuadSlicerVisualizerCanvas : MonoBehaviour
    {
        [SerializeField] private TMP_Text xIDText;
        [SerializeField] private TMP_Text yIDText;

        private QuadSlicerVisualizerFacade _visualizerFacade;

        private void Awake()
        {
            _visualizerFacade = GetComponentInParent<QuadSlicerVisualizerFacade>();
            _visualizerFacade.NewSquareFocused += OnSlicedSquareChanged;
        }

        private void OnDestroy()
        {
            _visualizerFacade.NewSquareFocused -= OnSlicedSquareChanged;
        }

        private void OnSlicedSquareChanged(SlicedSquare slicedSquare)
        {
            xIDText.text = "X:" + (slicedSquare.xID).ToString();
            yIDText.text = "Y:" + (slicedSquare.yID).ToString();
        }
    }
}