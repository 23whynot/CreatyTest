using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void Save(Texture2D texture);
        Texture2D Load(Texture2D texture);
    }
}