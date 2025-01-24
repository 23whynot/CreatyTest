using System;

namespace CodeBase.Controllers.SaveLoadController
{
    public interface ISaveLoadEvent
    {
        public event Action OnSave;
        public event Action OnLoad;
    }
}