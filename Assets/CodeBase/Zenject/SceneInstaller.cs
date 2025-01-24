using CodeBase.Controllers.BrushSizeSystem;
using CodeBase.Controllers.ColorSystem;
using CodeBase.Controllers.SaveLoadController;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.SaveLoad;
using Zenject;
using UnityEngine;

namespace CodeBase.Zenject
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ColorController>().AsSingle();
            Container.BindInterfacesAndSelfTo<BrushSizeController>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveLoadController>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<IInputService>().FromMethod(GetInputService).AsSingle();
        }

        private IInputService GetInputService()
        {
            if (Application.isEditor)
                return new StandaloneInputService();
            else
                return new MobileInputService();
        }
    }
}
