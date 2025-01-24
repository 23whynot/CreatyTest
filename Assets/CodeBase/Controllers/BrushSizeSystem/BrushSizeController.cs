using System;

namespace CodeBase.Controllers.BrushSizeSystem
{
    public class BrushSizeController : IBrushSizeEvent, IBrushSizeController
    {
        public event Action<int> OnBrushSizeChanged;


        public void SetBrushSize(int brushSize)
        {
            OnBrushSizeChanged?.Invoke(brushSize);
        }
    }
}
