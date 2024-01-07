using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageProcessing.ImgColSlicer.Camera
{
    public class CameraController : MonoBehaviour
    {
        private static CameraController _instance;

        private UnityEngine.Camera _camera;
        private bool _isTransitioning;

        public static CameraController Instance => _instance;
        public bool IsTransitioning => _isTransitioning;

        private void Awake()
        {
            _instance = this;
            _camera = GetComponent<UnityEngine.Camera>();
        }

        public void SetPositionToView(GameObject target, float offset, bool transition, float transitionSpeed)
        {
            Vector3 desiredPos = target.transform.position + target.transform.forward * -1f * offset;
            //Vector3 desiredPos = CameraUtility.GetPositionToSeeWholeObject(target, _camera);
            _isTransitioning = true;

            if (transition)
            {
                gameObject.transform.rotation = Quaternion.LookRotation(target.transform.forward);
                StartCoroutine(TransitionToPos(desiredPos, transitionSpeed));
            }
            else
            {
                gameObject.transform.position = desiredPos;
                gameObject.transform.rotation = Quaternion.LookRotation(target.transform.forward);
                _isTransitioning = false;
            }
        }

        private IEnumerator TransitionToPos(Vector3 desiredPos, float transitionSpeed)
        {
            Vector3 orgPos = gameObject.transform.position;

            for (float i = 0; i < 1; i += Time.deltaTime * transitionSpeed)
            {
                gameObject.transform.position = Vector3.Lerp(orgPos, desiredPos, i);
                yield return null;
            }

            _isTransitioning = false;
        }
    }
}