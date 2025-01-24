using System;

namespace CodeBase.Controllers.BrushSizeSystem
{
    public interface IBrushSizeEvent
    {
        public event Action<int> OnBrushSizeChanged;
    }
}