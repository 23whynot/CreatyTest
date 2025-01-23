using System.Collections.Generic;

[System.Serializable]
public class TextureData
{
    public int width;
    public int height;
    public List<ColorData> colors;

    public TextureData(int width, int height, List<ColorData> colors)
    {
        this.width = width;
        this.height = height;
        this.colors = colors;
    }
}