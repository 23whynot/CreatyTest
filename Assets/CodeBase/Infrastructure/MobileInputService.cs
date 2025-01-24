using CodeBase.Infrastructure.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class MobileInputService : InputService
    {
        public override Vector2 GetPointerPosition => PointerTouch();

        public override bool IsInputPressed
        {
            get
            {
                if (UnityEngine.Input.touchCount > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}