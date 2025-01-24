using System.IO;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly string _savePath = Path.Combine(Application.persistentDataPath, "texture.png");

        public void Save(Texture2D texture)
        {
            if (texture != null)
                File.WriteAllBytes(_savePath, texture.EncodeToPNG());
        }

        public Texture2D Load(Texture2D texture)
        {
            if (File.Exists(_savePath))
            {
                byte[] imageData = File.ReadAllBytes(_savePath);
                if (texture == null)
                    texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                texture.Apply();
            }
            return texture;
        }
    }
}
