using UnityEngine;

[System.Serializable]
public class ColorData
{
    public int x;
    public int y;
    public float r;
    public float g;
    public float b;
    public float a;

    public ColorData(int x, int y, Color color)
    {
        this.x = x;
        this.y = y;
        this.r = color.r;
        this.g = color.g;
        this.b = color.b;
        this.a = color.a;
    }
}