using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using ImageProcessing.ImgColSlicer.Quad;
using ImageProcessing.ImgColSlicer.Camera;

namespace ImageProcessing.ImgColSlicer.QuadVis
{
    public class QuadSlicerVisualizerFacade : MonoBehaviour
    {
        public const string LastViewedXIDKey = "QuadColorSlicer.XID";
        public const string LastViewedYIDKey = "QuadColorSlicer.YID";

        [SerializeField] private Shader defaultShader;
        [SerializeField] private QuadColorSlicerResult _result;
        [SerializeField] private QuadColorSlicerSettings _settings;
        [SerializeField] private QuadColorSlicerData _data;
        [SerializeField, Range(0f, 1f)] private float _space;
        [SerializeField] private float _cameraDistanceOffset;
        [SerializeField] private KeyCode _finalMatKeycode;
        [SerializeField] private KeyCode _blackWhiteKeycode;
        [SerializeField] private KeyCode _colorKeycode;
        [SerializeField] private KeyCode _colorSlicedKeycode;
        [SerializeField] private Material _backgroundMat;

        private Material _colorMat;
        private Material _blackWhiteMat;
        private Material _finalMat;
        private Material _colorSlicedMat;



        private SlicedSquare[,] _slicedSquares;

        private float _spaceCached;
        private float _cameraDistanceOffsetCached;
        private KeyCode _currentKeycode;
        private Vector2Int _currentFocusedID;

        public event Action<SlicedSquare> NewSquareFocused;

        private void Start()
        {
            int colorWidth = _data.NormalTexture.width;
            int colorHeight = _data.NormalTexture.height;

            int blackWhiteWidth = _result.BlackWhiteResult.width;
            int blackWhiteHeight = _result.BlackWhiteResult.height;

            int finalWidth = _result.FinalResult.width;
            int finalHeight = _result.FinalResult.height;

            _colorMat = new Material(defaultShader);
            _colorMat.mainTexture = _data.NormalTexture;

            _blackWhiteMat = new Material(defaultShader);
            _blackWhiteMat.mainTexture = _result.BlackWhiteResult;

            _finalMat = new Material(defaultShader);
            _finalMat.mainTexture = _result.FinalResult;

            _colorSlicedMat = new Material(defaultShader);
            _colorSlicedMat.mainTexture = _result.ColorSlidedResult;

            int squaresWidthCount = Mathf.CeilToInt(colorWidth / _settings.PixelSlicingSquareDim);
            int squaresHeightCount = Mathf.CeilToInt(colorHeight / _settings.PixelSlicingSquareDim);

            float perSquareUVWidthOffset = 1f / (float)squaresWidthCount;
            float perSquareUVHeightOffset = 1f / (float)squaresHeightCount;

            _slicedSquares = new SlicedSquare[squaresWidthCount, squaresHeightCount];

            for (int i = 0; i < squaresWidthCount; i++)
            {
                for (int j = 0; j < squaresHeightCount; j++)
                {
                    SlicedSquare slicedSquare = new SlicedSquare();

                    GameObject squareObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    GameObject squareBackgroundObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    squareObj.transform.position = new Vector3(i,
                        j);
                    MeshRenderer backgroundObjRend = squareBackgroundObj.GetComponent<MeshRenderer>();
                    backgroundObjRend.material = _backgroundMat;
                    squareBackgroundObj.SetActive(false);
                    squareBackgroundObj.transform.localScale = Vector3.one * 1.25f;
                    squareBackgroundObj.transform.position = new Vector3(i, j, +0.75f);

                    MeshRenderer squareRenderer = squareObj.GetComponent<MeshRenderer>();
                    MeshFilter squareFilter = squareObj.GetComponent<MeshFilter>();
                    Mesh squareMesh = squareFilter.mesh;

                    Vector2[] uvs = new Vector2[4];
                    float widthPercentage = (float)i / (float)squaresWidthCount;
                    float heightPercentage = (float)j / (float)squaresHeightCount;
                    uvs[0] = new Vector2(widthPercentage, heightPercentage);
                    uvs[1] = new Vector2(widthPercentage + perSquareUVWidthOffset, heightPercentage);
                    uvs[2] = new Vector2(widthPercentage, heightPercentage + perSquareUVHeightOffset);
                    uvs[3] = new Vector2(widthPercentage + perSquareUVWidthOffset,
                        heightPercentage + perSquareUVHeightOffset);

                    squareMesh.uv = uvs;

                    squareFilter.mesh = squareMesh;

                    squareRenderer.material = _finalMat;

                    slicedSquare.xID = i + 1;
                    slicedSquare.yID = j + 1;
                    slicedSquare.RootPos = new Vector3(i, j, 0);
                    slicedSquare.Renderer = squareRenderer;
                    slicedSquare.GameObject = squareObj;
                    slicedSquare.BackgroundObj = squareBackgroundObj;

                    _slicedSquares[i, j] = slicedSquare;
                }
            }

            TrySetPositionToLastSquare();
        }

        private void TrySetPositionToLastSquare()
        {
            if (_slicedSquares.GetLength(0) <= 0)
            {
                return;
            }

            if (PlayerPrefs.HasKey(LastViewedXIDKey) && PlayerPrefs.HasKey(LastViewedYIDKey))
            {
                int xID = PlayerPrefs.GetInt(LastViewedXIDKey);
                int yID = PlayerPrefs.GetInt(LastViewedYIDKey);

                if (xID > _slicedSquares.GetLength(0) || yID > _slicedSquares.GetLength(1))
                {
                    xID = 0;
                    yID = 0;
                }

                _currentFocusedID = new Vector2Int(xID, yID);
                var slicedSquare = _slicedSquares[xID, yID];

                slicedSquare.BackgroundObj.SetActive(true);
                CameraController.Instance.SetPositionToView(slicedSquare.GameObject, _cameraDistanceOffset, false, 0f);
                NewSquareFocused?.Invoke(_slicedSquares[_currentFocusedID.x, _currentFocusedID.y]);
            }
            else
            {
                _currentFocusedID = new Vector2Int(0, 0);
                var slicedSquare = _slicedSquares[_currentFocusedID.x, _currentFocusedID.y];

                slicedSquare.BackgroundObj.SetActive(true);
                CameraController.Instance.SetPositionToView(_slicedSquares[0, 0].GameObject, _cameraDistanceOffset,
                    false, 0f);
                NewSquareFocused?.Invoke(_slicedSquares[_currentFocusedID.x, _currentFocusedID.y]);
            }
        }

        private void OnValidate()
        {
            if (CameraController.Instance == null)
            {
                return;
            }

            if (!Application.isPlaying)
            {
                return;
            }

            if (!Mathf.Approximately(_space, _spaceCached))
            {
                for (int i = 0; i < _slicedSquares.GetLength(0); i++)
                {
                    for (int j = 0; j < _slicedSquares.GetLength(1); j++)
                    {
                        float xOffset = i * _space;
                        float yOffset = j * _space;
                        _slicedSquares[i, j].GameObject.transform.position = new Vector3(
                            _slicedSquares[i, j].RootPos.x + xOffset, _slicedSquares[i, j].RootPos.y + yOffset,
                            _slicedSquares[i, j].RootPos.z);
                        _slicedSquares[i, j].BackgroundObj.transform.position = new Vector3(
                            _slicedSquares[i, j].RootPos.x + xOffset, _slicedSquares[i, j].RootPos.y + yOffset,
                            _slicedSquares[i, j].RootPos.z + 0.75f);
                    }
                }

                _spaceCached = _space;
            }

            if (!Mathf.Approximately(_cameraDistanceOffset, _cameraDistanceOffsetCached))
            {
                CameraController.Instance.SetPositionToView(
                    _slicedSquares[_currentFocusedID.x, _currentFocusedID.y].GameObject, _cameraDistanceOffset, true,
                    2f);

                _cameraDistanceOffsetCached = _cameraDistanceOffset;
            }
        }

        private void Update()
        {
            CheckMaterialInputs();

            CheckTransitionInput();
        }

        private void CheckMaterialInputs()
        {
            if (Input.GetKeyDown(_finalMatKeycode))
            {
                if (_finalMatKeycode != _currentKeycode)
                {
                    ApplyMatOnSlicedSquares(_finalMat);
                }
            }
            else if (Input.GetKeyDown(_blackWhiteKeycode))
            {
                if (_blackWhiteKeycode != _currentKeycode)
                {
                    ApplyMatOnSlicedSquares(_blackWhiteMat);
                }
            }
            else if (Input.GetKeyDown(_colorKeycode))
            {
                if (_colorKeycode != _currentKeycode)
                {
                    ApplyMatOnSlicedSquares(_colorMat);
                }
            }
            else if (Input.GetKeyDown(_colorSlicedKeycode))
            {
                if (_colorSlicedKeycode != _currentKeycode)
                {
                    ApplyMatOnSlicedSquares(_colorSlicedMat);
                }
            }
        }

        private void CheckTransitionInput()
        {
            if (_slicedSquares.GetLength(0) <= 0)
            {
                return;
            }

            if (CameraController.Instance.IsTransitioning)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Vector2Int desiredID = new Vector2Int(_currentFocusedID.x, _currentFocusedID.y + 1);
                if (CanTransitionTo(desiredID.x, desiredID.y))
                {
                    _slicedSquares[_currentFocusedID.x, _currentFocusedID.y].BackgroundObj.SetActive(false);
                    CameraController.Instance.SetPositionToView(_slicedSquares[desiredID.x, desiredID.y].GameObject,
                        _cameraDistanceOffset, true, 2f);
                    _currentFocusedID = desiredID;
                    _slicedSquares[desiredID.x, desiredID.y].BackgroundObj.SetActive(true);
                    PlayerPrefs.SetInt(LastViewedXIDKey, _currentFocusedID.x);
                    PlayerPrefs.SetInt(LastViewedYIDKey, _currentFocusedID.y);
                    NewSquareFocused?.Invoke(_slicedSquares[_currentFocusedID.x, _currentFocusedID.y]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Vector2Int desiredID = new Vector2Int(_currentFocusedID.x - 1, _currentFocusedID.y);
                if (CanTransitionTo(desiredID.x, desiredID.y))
                {
                    _slicedSquares[_currentFocusedID.x, _currentFocusedID.y].BackgroundObj.SetActive(false);
                    CameraController.Instance.SetPositionToView(_slicedSquares[desiredID.x, desiredID.y].GameObject,
                        _cameraDistanceOffset, true, 2f);
                    _currentFocusedID = desiredID;
                    _slicedSquares[desiredID.x, desiredID.y].BackgroundObj.SetActive(true);
                    PlayerPrefs.SetInt(LastViewedXIDKey, _currentFocusedID.x);
                    PlayerPrefs.SetInt(LastViewedYIDKey, _currentFocusedID.y);
                    NewSquareFocused?.Invoke(_slicedSquares[_currentFocusedID.x, _currentFocusedID.y]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Vector2Int desiredID = new Vector2Int(_currentFocusedID.x, _currentFocusedID.y - 1);
                if (CanTransitionTo(desiredID.x, desiredID.y))
                {
                    _slicedSquares[_currentFocusedID.x, _currentFocusedID.y].BackgroundObj.SetActive(false);
                    CameraController.Instance.SetPositionToView(_slicedSquares[desiredID.x, desiredID.y].GameObject,
                        _cameraDistanceOffset, true, 2f);
                    _currentFocusedID = desiredID;
                    _slicedSquares[desiredID.x, desiredID.y].BackgroundObj.SetActive(true);
                    PlayerPrefs.SetInt(LastViewedXIDKey, _currentFocusedID.x);
                    PlayerPrefs.SetInt(LastViewedYIDKey, _currentFocusedID.y);
                    NewSquareFocused?.Invoke(_slicedSquares[_currentFocusedID.x, _currentFocusedID.y]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Vector2Int desiredID = new Vector2Int(_currentFocusedID.x + 1, _currentFocusedID.y);
                if (CanTransitionTo(desiredID.x, desiredID.y))
                {
                    _slicedSquares[_currentFocusedID.x, _currentFocusedID.y].BackgroundObj.SetActive(false);
                    CameraController.Instance.SetPositionToView(_slicedSquares[desiredID.x, desiredID.y].GameObject,
                        _cameraDistanceOffset, true, 2f);
                    _currentFocusedID = desiredID;
                    _slicedSquares[desiredID.x, desiredID.y].BackgroundObj.SetActive(true);
                    PlayerPrefs.SetInt(LastViewedXIDKey, _currentFocusedID.x);
                    PlayerPrefs.SetInt(LastViewedYIDKey, _currentFocusedID.y);
                    NewSquareFocused?.Invoke(_slicedSquares[_currentFocusedID.x, _currentFocusedID.y]);
                }
            }
        }

        private bool CanTransitionTo(int i, int j)
        {
            if (_slicedSquares.GetLength(0) < i || i < 0)
            {
                return false;
            }

            if (_slicedSquares.GetLength(1) < j || j < 0)
            {
                return false;
            }

            return true;
        }

        private void ApplyMatOnSlicedSquares(Material material)
        {
            for (int i = 0; i < _slicedSquares.GetLength(0); i++)
            {
                for (int j = 0; j < _slicedSquares.GetLength(1); j++)
                {
                    _slicedSquares[i, j].SetMaterial(material);
                }
            }
        }
    }

    public struct SlicedSquare
    {
        public Vector3 RootPos;
        public GameObject GameObject;
        public GameObject BackgroundObj;
        public int xID;
        public int yID;
        public Renderer Renderer;

        public void SetMaterial(Material material)
        {
            Renderer.material = material;
        }
    }
}