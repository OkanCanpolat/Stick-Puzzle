using UnityEngine;

public class Line : MonoBehaviour
{
    public bool IsEmpty;
    public int ConnectedCellCount;
    public SpriteRenderer HighlightedSprite;
    public Dot[] ConnectedDots;
    private Color initialHighlightColor;
    private void Awake()
    {
        ConnectedDots = new Dot[2];
        initialHighlightColor = HighlightedSprite.color;
        IsEmpty = true;
    }

    public void BackToInitialColor(Color color)
    {
        HighlightedSprite.gameObject.SetActive(false);
        foreach (var item in ConnectedDots)
        {
            item.BackToInitialColor(color);
        }
    }
    public void Highligh(Color color)
    {
        HighlightedSprite.gameObject.SetActive(true);
        
        foreach (var item in ConnectedDots)
        {
            item.Highlight(color);
        }
    }
    public void OnPlacement(Color color)
    {
        HighlightedSprite.gameObject.SetActive(true);
        HighlightedSprite.color = color;

        foreach (var item in ConnectedDots)
        {
            item.OnPlacement(color);
        }
    }
    public void OnDisplacement()
    {
        HighlightedSprite.gameObject.SetActive(false);
        HighlightedSprite.color = initialHighlightColor;

        foreach (var item in ConnectedDots)
        {
            item.OnDisplacement();
        }
    }
}
