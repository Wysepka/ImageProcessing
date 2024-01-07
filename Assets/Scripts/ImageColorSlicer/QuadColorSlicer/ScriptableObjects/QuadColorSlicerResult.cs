using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace ImageProcessing.ImgColSlicer.Quad
{
    [CreateAssetMenu(menuName = "QuadSlicer/QuadSlicerResult", fileName = "QuadSlicerResult")]
    public class QuadColorSlicerResult : ScriptableObject
    {
        [SerializeField] private string outputID;
        [SerializeField] private Texture2D blackWhiteResult;

        [FormerlySerializedAs("quadSlicedResult")] [SerializeField]
        private Texture2D colorSlicedResult;

        [SerializeField] private Texture2D finalResult;

        public void SetResult(Texture2D blackWhite, Texture2D colorSliced, Texture2D final)
        {

            blackWhiteResult = blackWhite;
            blackWhiteResult.Apply();

            if (colorSliced != null)
            {
                colorSlicedResult = colorSliced;
                colorSliced.Apply();
            }

            finalResult = final;
            finalResult.Apply();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public string OutputID => outputID;

        public Texture2D BlackWhiteResult => blackWhiteResult;
        public Texture2D ColorSlidedResult => colorSlicedResult;
        public Texture2D FinalResult => finalResult;
    }
}