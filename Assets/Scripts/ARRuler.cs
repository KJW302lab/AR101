using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(LineRenderer))]
public class ARRuler : MonoBehaviour
{
    [SerializeField] Image            imgCenterPoint;
    [SerializeField] ARRaycastManager raycastManager;

    [SerializeField] GameObject       worldCanvas;
    [SerializeField] TMP_Text         txtDistance;

    private List<ARRaycastHit> _hits = new();
    private Vector2            _screenCenter;
    private bool               _canMeasure;
    private bool               _isMeasuring;

    private LineRenderer _line;

    private Pose    _currentPose;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private float   _distance;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
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
            _distance = Vector3.Distance(_startPosition, _currentPose.position);

            _line.SetPosition(1, _currentPose.position);

            UpdateDistanceText();
        }
        else
        {
            _distance = 0f;
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

        _line.positionCount = 2;

        _line.SetPosition(0, _startPosition);
    }

    public void SetEndPosition()
    {
        _endPosition = _currentPose.position;

        _distance = Vector3.Distance(_startPosition, _endPosition);

        _isMeasuring = false;

        UpdateDistanceText();
    }

    private void UpdateDistanceText()
    {
        if (_line.positionCount < 2) return;

        // 라인의 시작점과 끝점을 가져옴
        Vector3 startPos = _line.GetPosition(0);
        Vector3 endPos = _line.GetPosition(1);

        // 라인의 중간 지점 계산 (텍스트 위치)
        Vector3 midPoint = (startPos + endPos) / 2;
        worldCanvas.transform.position = midPoint + Vector3.up * 0.05f; // 살짝 위로 이동

        // 라인의 방향을 계산
        Vector3 direction = (endPos - startPos).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // 라인의 방향과 평행하도록 텍스트 회전 적용
        worldCanvas.transform.rotation = lookRotation;

        // 거리 값을 텍스트로 설정
        txtDistance.text = $"{_distance:F2} m";
    }
}
