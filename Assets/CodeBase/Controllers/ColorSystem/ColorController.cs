using CodeBase.Brush;
using CodeBase.Object;
using UnityEngine;
using Zenject;

namespace CodeBase.Controllers.ColorSystem
{
    public class ColorController : IColorController
    {
        private IPaintBrush _paintBrush;

        [Inject]
        public void Construct(IPaintBrush paintBrush)
        {
            _paintBrush = paintBrush;
        }

        public void SetColor(Color color)
        {
            _paintBrush.SetBrushColor(color);
        }
    }
}