using UnityEngine;

public static class ColorUtils 
{
    public static Color GetGreyTone(Color color)
    {
        Color32 newColor = color;
        byte r = (byte)Mathf.Clamp(newColor.r - 30f, 0, 255f);
        byte g = (byte)Mathf.Clamp(newColor.g - 30f, 0, 255f);
        byte b = (byte)Mathf.Clamp(newColor.b - 30f, 0, 255f);
        newColor.r = r;
        newColor.g = g;
        newColor.b = b;
        return newColor;
    }
}
