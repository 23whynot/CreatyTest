using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public abstract class InputService : IInputService
    {
        public abstract Vector2 GetPointerPosition { get; }
        
        public abstract bool IsInputPressed { get; }

        protected static Vector2 PointerTouch()
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                return UnityEngine.Input.GetTouch(0).position;
            }
            return Vector2.zero;
        }

        protected static Vector2 PointerMouse() =>
            new(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
    }
}