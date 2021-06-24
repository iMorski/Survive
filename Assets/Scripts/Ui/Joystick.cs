using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private GameObject HandleArea;
    [SerializeField] private GameObject Handle;
    [Range(0.0f, 1.0f)][SerializeField] private float NoDataDistance;
    
    [NonSerialized] public Vector2 Direction;
    [NonSerialized] public float Distance;

    private Image HandleAreaImage;
    private Image HandleImage;
    
    private void Awake()
    {
        HandleAreaImage = HandleArea.GetComponent<Image>();
        HandleImage = Handle.GetComponent<Image>();
    }
    
    private float HandleAreaRadius;
    private void Start()
    {
        Rect HandleAreaRect = HandleArea.GetComponent<RectTransform>().rect;
        
        HandleAreaRadius = Math.Max(HandleAreaRect.width, HandleAreaRect.height) / 2.0f *
                           GetComponentInParent<Canvas>().scaleFactor;
    }

    private Vector2 BeginMousePosition;
    
    public void OnPointerDown(PointerEventData Data)
    {
        if (!InScreen(Data)) return;
        
        BeginMousePosition = MousePosition(Data);

        HandleArea.transform.position = BeginMousePosition;
        Handle.transform.position = BeginMousePosition;

        HandleAreaImage.enabled = true;
        HandleImage.enabled = true;
    }

    public void OnDrag(PointerEventData Data)
    {
        Vector2 DirectionInPixel = MousePosition(Data) - BeginMousePosition;
        Vector2 DirectionInPixelInCircle = Vector2.ClampMagnitude(DirectionInPixel, HandleAreaRadius);

        float DragDistance = Vector2.Distance(MousePosition(Data), BeginMousePosition);

        if (DragDistance > HandleAreaRadius ||
            DragDistance > HandleAreaRadius)
        {
            BeginMousePosition = BeginMousePosition + (DirectionInPixel - DirectionInPixelInCircle);
        }
        
        HandleArea.transform.position = BeginMousePosition;
        Handle.transform.position = MousePosition(Data);
        
        Distance = Vector2.Distance(new Vector2(), DirectionInPixelInCircle) / HandleAreaRadius;
        
        if (Distance > NoDataDistance) Direction = new Vector2(
            DirectionInPixelInCircle.x / HandleAreaRadius,
            DirectionInPixelInCircle.y / HandleAreaRadius);
        else Direction = new Vector2();
    }

    public void OnPointerUp(PointerEventData Data)
    {
        Direction = new Vector2(0.0f, 0.0f);
        Distance = 0.0f;
        
        HandleAreaImage.enabled = false;
        HandleImage.enabled = false;
    }

    private bool InScreen(PointerEventData Data)
    {
        return Data.hovered.Contains(gameObject);
    }

    private Vector2 MousePosition(PointerEventData Data)
    {
        return Data.position;
    }
}