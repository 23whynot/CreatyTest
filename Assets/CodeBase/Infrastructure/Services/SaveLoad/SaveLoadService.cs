using System.IO;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private string _savePath = Path.Combine(Application.persistentDataPath, "textureData.json");

        public void SaveData(string jsonData)
        {
            File.WriteAllText(_savePath, jsonData);
        }
        

        public string LoadData()
        {
            if (File.Exists(_savePath))
                return File.ReadAllText(_savePath);
            
            return null;
        }
    }
}