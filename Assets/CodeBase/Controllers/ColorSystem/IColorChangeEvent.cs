using System;
using UnityEngine;

namespace CodeBase.Controllers.ColorSystem
{
    public interface IColorChangeEvent
    {
        public event Action<Color> OnColorChange;
    }
}