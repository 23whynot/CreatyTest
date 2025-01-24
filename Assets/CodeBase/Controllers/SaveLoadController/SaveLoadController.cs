using System;

namespace CodeBase.Controllers.SaveLoadController
{
    public class SaveLoadController : ISaveLoadController, ISaveLoadEvent
    {
        public event Action OnSave;
        public event Action OnLoad;

        public void Save()
        {
            OnSave?.Invoke();
        }

        public void Load()
        {
            OnLoad?.Invoke();
        }
    }
}