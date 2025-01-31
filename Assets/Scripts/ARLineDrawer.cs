using System.Collections.Generic;
using UnityEngine;

public class ARLineDrawer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRendererPrefab;
    [SerializeField] Transform    pivot;

    private List<LineRenderer> _lineList = new();
    private LineRenderer       _currentLineRenderer;
    private bool               _isDrawing;

    public void Draw()
    {
        if (_currentLineRenderer == null)
        {
            _currentLineRenderer = Instantiate(lineRendererPrefab).GetComponent<LineRenderer>();

            _currentLineRenderer.positionCount = 1;
            _currentLineRenderer.SetPosition(0, pivot.position);

            _currentLineRenderer.startColor = _currentLineRenderer.endColor = Color.red;

            _lineList.Add(_currentLineRenderer);
        }

        _isDrawing = true;
    }

    public void StopDraw()
    {
        _isDrawing = false;
        _currentLineRenderer = null;
    }

    private void Update()
    {
        if (_isDrawing == false)
            return;

        _currentLineRenderer.positionCount = _currentLineRenderer.positionCount + 1;
        _currentLineRenderer.SetPosition(_currentLineRenderer.positionCount - 1, pivot.position);
    }
}
