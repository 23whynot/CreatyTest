using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        public void SaveData(string json);
        public string LoadData();
    }
}