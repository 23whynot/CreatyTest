using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public interface IInputService
    {
        Vector2 GetPointerPosition { get; }
        bool IsInputPressed { get; }
    }
}