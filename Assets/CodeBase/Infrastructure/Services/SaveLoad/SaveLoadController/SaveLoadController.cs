using System.Collections.Generic;
using CodeBase.Infrastructure.Data;
using CodeBase.Object;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Services.SaveLoad.SaveLoadController
{
    public class SaveLoadController : ISaveLoadController
    {
        private ISaveLoadService _saveLoadService;
        private ISavedTexture _savedTexture;

        [Inject]
        public void Construct(ISaveLoadService saveLoadService, ISavedTexture savedTexture)
        {
            _saveLoadService = saveLoadService;
            _savedTexture = savedTexture;
        }

        public void Save()
        {
            Texture2D texture = _savedTexture.texture;
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
            _saveLoadService.SaveData(json);
        }

        public void Load()
        {
            string jsonData = _saveLoadService.LoadData();
            if (string.IsNullOrEmpty(jsonData))
                return;

            TextureData textureData = JsonUtility.FromJson<TextureData>(jsonData);
            Texture2D texture = _savedTexture.texture;

            for (int y = 0; y < textureData.height; y++)
            {
                for (int x = 0; x < textureData.width; x++)
                {
                    Color color = textureData.colors[y * textureData.width + x].ToColor();
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
        }
    }
}