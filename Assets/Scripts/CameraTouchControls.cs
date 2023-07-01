using UnityEngine;
using UnityEngine.EventSystems;

public class CameraTouchControls : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 TouchDist;

    private Vector2 _pointerOld;
    private int _pointerId;
    private bool _isPressed;

    void Update()
    {
        if (_isPressed)
        {
            if (_pointerId >= 0 && _pointerId < Input.touches.Length)
            {
                TouchDist = Input.touches[_pointerId].position - _pointerOld;
                _pointerOld = Input.touches[_pointerId].position;
            }
            // For debug purposes in Editor
            else
            {
                TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _pointerOld;
                _pointerOld = Input.mousePosition;
            }
        }
        else
        {
            TouchDist = default;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        _pointerId = eventData.pointerId;
        _pointerOld = eventData.position;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
    }

}
