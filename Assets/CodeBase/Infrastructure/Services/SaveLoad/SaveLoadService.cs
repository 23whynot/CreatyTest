using System.Collections.Generic;
using System.IO;
using CodeBase.Infrastructure.Data;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private string _savePath = Path.Combine(Application.persistentDataPath, "textureData.json");
        
        public void SaveTexture(Texture2D texture)
        {
            List<ColorData> colorDataList = new List<ColorData>();

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    Color pixelColor = texture.GetPixel(x, y);
                    colorDataList.Add(new ColorData(x, y, pixelColor));
                }
            }

            TextureData textureData = new TextureData(texture.width, texture.height, colorDataList);
            string json = JsonUtility.ToJson(textureData, true);
            File.WriteAllText(_savePath, json);
        }

        public Texture2D LoadTexture(Texture2D inputTexture)
        {
            if (!File.Exists(_savePath)) return inputTexture;

            string json = File.ReadAllText(_savePath);
            TextureData textureData = JsonUtility.FromJson<TextureData>(json);

            for (int y = 0; y < textureData.height; y++)
            {
                for (int x = 0; x < textureData.width; x++)
                {
                    Color color = textureData.colors[y * textureData.width + x].ToColor();
                    inputTexture.SetPixel(x, y, color);
                }
            }

            inputTexture.Apply();
            return inputTexture;
        }
    }
}