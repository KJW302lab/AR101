using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CameraReticle : MonoBehaviour
{
    public ARPlane CurrentPlane { get; private set; }

    [SerializeField] GameObject       reticleModel;

    [SerializeField] ARPlaneManager   planeManager;
    [SerializeField] ARRaycastManager raycastManager;
    [SerializeField] SurfaceManager   surfaceManager;
    [SerializeField] Camera           mainCam;

    private List<ARRaycastHit> _hits = new();
    private ARRaycastHit? _hit;

    private void Update()
    {
        var center = mainCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        raycastManager.Raycast(center, _hits, TrackableType.PlaneWithinBounds);

        _hit = null;

        if (_hits.Count > 0)
        {
            var fixedPlane = surfaceManager.FixedPlane;

            _hit = fixedPlane == null
                ? _hits[0]
                : _hits.SingleOrDefault(point => point.trackableId == fixedPlane.trackableId);
        }

        if (_hit.HasValue)
        {
            CurrentPlane = planeManager.GetPlane(_hit.Value.trackableId);

            transform.position = _hit.Value.pose.position;
        }

        reticleModel.SetActive(CurrentPlane != null);
    }
}
