using CodeBase.Controllers.BrushSizeSystem;
using CodeBase.Controllers.ColorSystem;
using CodeBase.Infrastructure.Services.SaveLoad.SaveLoadController;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace CodeBase.UI
{
    public class HudInputController : MonoBehaviour
    {
        [Header("Color Input")] 
        [SerializeField] Button colorRed;
        [SerializeField] Button colorGreen;
        [SerializeField] Button colorBlue;
        [SerializeField] Button eraser;

        [Header("Brush Size Input")] 
        [SerializeField] Button eight;
        [SerializeField] Button sixteen;
        [SerializeField] Button thirtyTwo;

        [Header("SaveLoad Input")] 
        [SerializeField] Button load;
        [SerializeField] Button save;
        
        private readonly Color _eraserColor = new Color(188f / 255f, 188f / 255f, 188f / 255f);

        private IColorController _colorController;
        private IBrushSizeController _brushSizeController;
        private ISaveLoadController _saveLoadController;

        [Inject]
        public void Construct(IColorController colorController, IBrushSizeController brushSizeController, ISaveLoadController saveLoadController)
        {
            _saveLoadController = saveLoadController;
            _colorController = colorController;
            _brushSizeController = brushSizeController;
        }

        private void Start()
        {
            BindColorButton();
            BindBrushSizeButton();
            BindSaveLoadButton();
        }

        private void BindSaveLoadButton()
        {
            load.onClick.AddListener(() => _saveLoadController.Load());
            save.onClick.AddListener(() => _saveLoadController.Save());
        }

        private void BindBrushSizeButton()
        {
            eight.onClick.AddListener(() => _brushSizeController.SetBrushSize(8));
            sixteen.onClick.AddListener(() => _brushSizeController.SetBrushSize(16));
            thirtyTwo.onClick.AddListener(() => _brushSizeController.SetBrushSize(32));
        }

        private void BindColorButton()
        {
            colorRed.onClick.AddListener(() => _colorController.SetColor(Color.red));
            colorGreen.onClick.AddListener(() => _colorController.SetColor(Color.green));
            colorBlue.onClick.AddListener(() => _colorController.SetColor(Color.blue));
            eraser.onClick.AddListener(() => _colorController.SetColor(_eraserColor));
        }
    }
}