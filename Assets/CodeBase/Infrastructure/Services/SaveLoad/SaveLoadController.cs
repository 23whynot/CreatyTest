using CodeBase.Object;
using Zenject;

namespace CodeBase.Infrastructure.Services.SaveLoad
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

        public void Save() => _saveLoadService.Save(_savedTexture.texture);
        public void Load() => _savedTexture.texture = _saveLoadService.Load(_savedTexture.texture);
    }
}
