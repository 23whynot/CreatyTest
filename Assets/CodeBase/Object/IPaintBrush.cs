using UnityEngine;

namespace CodeBase.Object
{
    public interface IPaintBrush
    {
        void Paint(Texture2D texture, int x, int y);
        void SetBrushSize(int size);
        void SetBrushColor(Color color);
    }
}