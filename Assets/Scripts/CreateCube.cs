using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CreateCube : MonoBehaviour
{
    [SerializeField] private ARRaycastManager rayCastManager;

    private List<ARRaycastHit> _hits = new();

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && rayCastManager.Raycast(touch.position, _hits, TrackableType.PlaneWithinPolygon))
            {
                if (_hits.Count >= 1)
                {
                    Pose hitPose = _hits[0].pose;
                    
                    Instantiate(rayCastManager.raycastPrefab, hitPose.position, hitPose.rotation);
                }
            }
        }
    }
}
