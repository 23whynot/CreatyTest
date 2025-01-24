using UnityEngine;

namespace CodeBase.Brush
{
    public interface IPaintBrush
    {
        void Paint(Texture2D texture, int x, int y);
        void SetBrushSize(int size);
        void SetBrushColor(Color color);
    }
}