using UnityEngine;

public class Dot : MonoBehaviour
{
    private Color initialColor;
    private SpriteRenderer spriteRenderer;
    private int connectedLineCount = 0;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
    }

    public void Highlight(Color color)
    {
        Color newColor = ColorUtils.GetGreyTone(color);
        spriteRenderer.color = newColor;
    }

    public void BackToInitialColor(Color color)
    {
        if (connectedLineCount <= 0)
        {
            spriteRenderer.color = initialColor;
        }
        else
        {
            spriteRenderer.color = color;
        }
    }

    public void OnPlacement(Color color)
    {
        spriteRenderer.color = color;
        connectedLineCount++;
    }
    public void OnDisplacement()
    {
        connectedLineCount--;

        if (connectedLineCount <= 0)
        {
            spriteRenderer.color = initialColor;
        }
    }
}
