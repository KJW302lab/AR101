using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARRuler : MonoBehaviour
{
    public float Distance { get; private set; }

    [SerializeField] Image            imgCenterPoint;
    [SerializeField] ARRaycastManager raycastManager;

    private List<ARRaycastHit> _hits = new();
    private Vector2            _screenCenter;
    private bool               _canMeasure;
    private bool               _isMeasuring;

    private Pose    _currentPose;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private void Awake()
    {
        _screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    private void Update()
    {
        if (raycastManager.Raycast(_screenCenter, _hits, TrackableType.PlaneWithinPolygon))
        {
            SetCenterPointColor(Color.green);

            _currentPose = _hits[0].pose;

            _canMeasure = true;
        }
        else
        {
            SetCenterPointColor(Color.red);

            _canMeasure = false;
        }

        if (_canMeasure && _isMeasuring)
        {
            Distance = Vector3.Distance(_startPosition, _currentPose.position);
        }
        else
        {
            Distance = 0f;
        }
    }

    private void SetCenterPointColor(Color color)
    {
        imgCenterPoint.color = color;
    }

    public void SetStartPosition()
    {
        _startPosition = _currentPose.position;

        _isMeasuring = true;
    }

    public void SetEndPosition()
    {
        _endPosition = _currentPose.position;

        Distance = Vector3.Distance(_startPosition, _endPosition);

        _isMeasuring = false;
    }
}
