using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour
{
    public Transform RaycastPosition;
    public LayerMask Mask;
    public Color Color;
    public Line lastCastedLine = null;
    private SpriteRenderer spriteRenderer;

    public SpriteRenderer SpriteRenderer => spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        this.Color = color;
    }
    private void OnDrawGizmos()
    {
        if(RaycastPosition != null)
        Gizmos.DrawSphere(RaycastPosition.position, 0.05f);
    }
}
