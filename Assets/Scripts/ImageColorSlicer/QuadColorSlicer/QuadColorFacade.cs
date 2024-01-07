using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageProcessing.Utility;

namespace ImageProcessing.ImgColSlicer.Quad
{
    public class QuadColorFacade : MonoBehaviour
    {
        [SerializeField] private QuadColorSlicerData _data;
        [SerializeField] private QuadColorSlicerSettings _settings;
        [SerializeField] private QuadColorSlicerResult _result;

        private QuadSlicerStepBlackWhite blackWhiteStep;
        private QuadSlicerStepColor colorStep;
        private QuadSlicerTextureMerger merger;

        private void Awake()
        {
            Generate();
        }

        public void Generate()
        {
            blackWhiteStep = new QuadSlicerStepBlackWhite();
            colorStep = new QuadSlicerStepColor();
            merger = new QuadSlicerTextureMerger();

            //blackWhiteStep.ProcessTexture(_data.)
            var blackWhiteResult = blackWhiteStep.ProcessTexture(_data, _settings);
            //var colorResult = colorStep.ProcessTexture(_data, _settings);
            var finalResult = merger.MergeTextures(blackWhiteResult.processedTexture, _data, _settings);
            var colorSlicedResult = colorStep.ProcessTexture(_data, _settings);
            var textureAssetBlackWhite = TextureUtility.WriteTextureToNewPNG(blackWhiteResult.processedTexture,
                "QuadColorSlicer", _result.OutputID + "BlackWhite");
            //var textureAssetColor = TextureUtility.WriteTextureToNewPNG(colorResult.processedTexture, "QuadColorSlicer",
            //    "BeataGrzesiuTestColor");
            var textureFinal = TextureUtility.WriteTextureToNewPNG(finalResult.processedTexture, "QuadColorSlicer",
                _result.OutputID + "Final");
            var textureColorSliced = TextureUtility.WriteTextureToNewPNG(colorSlicedResult.processedTexture,
                "QuadColorSlicer", _result.OutputID + "ColorSliced");

            if (textureAssetBlackWhite != null && textureFinal != null && textureColorSliced != null)
            {
                _result.SetResult(textureAssetBlackWhite, textureColorSliced, textureFinal);
            }
        }

        public void SetData(QuadColorSlicerData data) => _data = data;
        public void SetSettings(QuadColorSlicerSettings settings) => _settings = settings;
        public void SetResult(QuadColorSlicerResult result) => _result = result;
    }
}