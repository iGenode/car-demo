using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickControls : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public static event Action<Vector2> OnMove;

    private RectTransform _thumbTransform;

    private const float DragThreshold = 0.6f;
    private const float DragMovementDistance = 30;
    private const int DragOffsetDistance = 50;

    private void Awake()
    {
        _thumbTransform = (RectTransform)transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_thumbTransform, eventData.position, null, out Vector2 offset);
        offset = Vector2.ClampMagnitude(offset, DragOffsetDistance) / DragOffsetDistance;
        _thumbTransform.anchoredPosition = offset * DragMovementDistance;

        Vector2 temp = CalculateMovementInput(offset);
        OnMove?.Invoke(temp);
    }

    private Vector2 CalculateMovementInput(Vector2 offset) => 
        new(Mathf.Abs(offset.x) > DragThreshold ? offset.x : 0, Mathf.Abs(offset.y) > DragThreshold ? offset.y : 0);

    public void OnPointerUp(PointerEventData eventData)
    {
        _thumbTransform.anchoredPosition = default;
        OnMove?.Invoke(default);
    }

    public void OnPointerDown(PointerEventData eventData) { }
}
