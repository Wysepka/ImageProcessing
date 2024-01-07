using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageProcessing.Utility
{
    public static class CameraUtility
    {
        public static Vector3 GetPositionToSeeWholeObject(GameObject yourGameObject, Camera camera)
        {
            Renderer renderer = yourGameObject.GetComponent<Renderer>();
            Bounds bounds = renderer.bounds;

            var distanceNeededToFitWidth = bounds.size.x * 0.5f /
                                           Mathf.Tan(camera.fieldOfView * camera.aspect * 0.5f * Mathf.Deg2Rad);
            var distanceNeededToFitHeight = bounds.size.y * 0.5f / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);

            Vector3 center = camera.transform.position + camera.transform.forward *
                Mathf.Max(distanceNeededToFitWidth, distanceNeededToFitHeight);

            Vector3 desiredPosition = center -
                                      camera.transform.up * bounds.extents.y -
                                      camera.transform.right * bounds.extents.x;

            return desiredPosition;
        }
    }
}