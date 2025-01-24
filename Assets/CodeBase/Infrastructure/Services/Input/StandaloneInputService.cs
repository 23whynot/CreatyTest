using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public class StandaloneInputService : InputService
    {
        public override Vector2 GetPointerPosition
        {
            get
            {
                Vector2 pointer = PointerTouch();

                if (pointer == Vector2.zero)
                {
                    pointer = PointerMouse();
                }

                return pointer;
            }
        }

        public override bool IsInputPressed
        {
            get
            {
                if (UnityEngine.Input.GetMouseButtonDown(0) && UnityEngine.Input.GetMouseButton(1))
                {
                    return true;
                }
                return false;
            }
        }
    }
}