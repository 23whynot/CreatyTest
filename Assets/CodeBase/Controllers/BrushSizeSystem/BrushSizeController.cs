using CodeBase.Brush;
using CodeBase.Object;
using Zenject;

namespace CodeBase.Controllers.BrushSizeSystem
{
    public class BrushSizeController :  IBrushSizeController
    {
        IPaintBrush _paintBrush;

        [Inject]
        public void Construct(IPaintBrush paintBrush)
        {
            _paintBrush = paintBrush;
        }
        
        public void SetBrushSize(int brushSize)
        {
            _paintBrush.SetBrushSize(brushSize);
        }
    }
}
