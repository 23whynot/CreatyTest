using System;
using UnityEngine;

namespace CodeBase.Controllers.ColorSystem
{
    public class ColorController : IColorController, IColorChangeEvent
    {
        public event Action<Color> OnColorChange;

        public void SetColor(Color color)
        {
            OnColorChange?.Invoke(color);
        }
    }
}