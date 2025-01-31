using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UnityEvent onPressed;
    [SerializeField] private UnityEvent onReleased;
    [SerializeField] private UnityEvent onPressStay;

    private bool _pressed = false;


    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
        onPressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
        onReleased?.Invoke();
    }

    private void Update()
    {
        if (_pressed)
            onPressStay?.Invoke();
    }
}
