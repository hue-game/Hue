using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Image _joystickContainer;
    private Image _joystickKnob;

    public Vector2 InputDirection;

    // Use this for initialization
    void Start()
    {
        _joystickContainer = GetComponent<Image>();
        _joystickKnob = transform.GetChild(0).GetComponent<Image>();
        InputDirection = Vector2.zero;
    }

    void Update()
    {
        //print(InputDirection);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = Vector2.zero;

        //To get InputDirection
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                (_joystickContainer.rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out position);

        position.x = (position.x / _joystickContainer.rectTransform.sizeDelta.x);
        position.y = (position.y / _joystickContainer.rectTransform.sizeDelta.y);

        float x = (_joystickContainer.rectTransform.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;
        float y = (_joystickContainer.rectTransform.pivot.y == 1f) ? position.y * 2 + 1 : position.y * 2 - 1;

        InputDirection = new Vector2(x, y);
        InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

        //to define the area in which joystick can move around
        _joystickKnob.rectTransform.anchoredPosition = new Vector3(InputDirection.x * (_joystickContainer.rectTransform.sizeDelta.x / 3)
                                                               , InputDirection.y * (_joystickContainer.rectTransform.sizeDelta.y) / 3);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputDirection = Vector2.zero;
        _joystickKnob.rectTransform.anchoredPosition = Vector2.zero;
    }
}
