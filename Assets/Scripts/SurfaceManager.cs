using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SurfaceManager : MonoBehaviour
{
    public ARPlane FixedPlane { get; private set; }

    [SerializeField] private ARPlaneManager   planeManager;
    [SerializeField] private ARRaycastManager raycastManager;

    public void FixPlane(ARPlane planeToFix)
    {
        if (FixedPlane == planeToFix)
            return;

        FixedPlane = planeToFix;

        foreach (var plane in planeManager.trackables)
            plane.gameObject.SetActive(plane == FixedPlane);

        planeManager.planesChanged += DisableNewPlanes;
    }

    private void DisableNewPlanes(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
            plane.gameObject.SetActive(false);
    }

    private void Update()
    {
        // �� ū Plane�� ���յǾ�����, ū Plane�� _fixedPlane���� ����

        if (FixedPlane?.subsumedBy != null)
            FixedPlane = FixedPlane.subsumedBy;
    }
}
