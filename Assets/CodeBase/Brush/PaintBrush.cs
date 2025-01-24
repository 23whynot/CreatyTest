using UnityEngine;

namespace CodeBase.Brush
{
    public class PaintBrush : IPaintBrush
    {
        private int _brushSize = 8;
        private Color _brushColor = Color.red;

        public void SetBrushSize(int size) => _brushSize = size;
        public void SetBrushColor(Color color) => _brushColor = color;

        public void Paint(Texture2D texture, int x, int y)
        {
            int radius = _brushSize / 2;
            float radiusSquared = radius * radius;

            for (int offsetY = -radius; offsetY < radius; offsetY++)
            {
                for (int offsetX = -radius; offsetX < radius; offsetX++)
                {
                    if (offsetX * offsetX + offsetY * offsetY < radiusSquared)
                    {
                        int pixelX = x + offsetX;
                        int pixelY = y + offsetY;

                        if (IsWithinTextureBounds(texture, pixelX, pixelY))
                        {
                            BlendPixel(texture, pixelX, pixelY);
                        }
                    }
                }
            }
        }

        private bool IsWithinTextureBounds(Texture2D texture, int x, int y)
        {
            return x >= 0 && x < texture.width && y >= 0 && y < texture.height;
        }

        private void BlendPixel(Texture2D texture, int x, int y)
        {
            Color existingColor = texture.GetPixel(x, y);
            Color blendedColor = Color.Lerp(existingColor, _brushColor, _brushColor.a);
            texture.SetPixel(x, y, blendedColor);
        }
    }
}