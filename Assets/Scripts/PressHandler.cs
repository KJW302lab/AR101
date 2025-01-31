using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UnityEvent onPressed;
    [SerializeField] private UnityEvent onReleased;

    private bool _pressed = false;


    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
        onReleased.Invoke();
    }

    private void Update()
    {
        if (_pressed)
            onPressed.Invoke();
    }
}
