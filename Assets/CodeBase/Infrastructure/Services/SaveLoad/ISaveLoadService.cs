using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        public void SaveTexture(Texture2D texture);
        public Texture2D LoadTexture(Texture2D texture);
    }
}