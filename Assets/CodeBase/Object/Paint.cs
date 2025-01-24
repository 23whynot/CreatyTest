using CodeBase.Controllers.BrushSizeSystem;
using CodeBase.Controllers.ColorSystem;
using CodeBase.Controllers.SaveLoadController;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Object
{
    public class Paint : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
    
        private Color _brushColor;
        private Camera _camera;
        private Texture2D _texture;
        private int _textureSize = 512;
        private int _lastRayX, _lastRayY;
        private int _brushSize = 8;
    
        private IInputService _inputService;
        private ISaveLoadService _saveLoadService;
        private IColorChangeEvent _colorChangeEvent;
        private IBrushSizeEvent _brushSizeEvent;
        private ISaveLoadEvent _saveLoadEvent;

        [Inject]
        public void Construct(IColorChangeEvent colorChangeEvent, 
            IBrushSizeEvent brushSizeEvent, 
            IInputService inputService, 
            ISaveLoadService saveLoadService, 
            ISaveLoadEvent saveLoadEvent)
        {
            _inputService = inputService;
            _colorChangeEvent = colorChangeEvent;
            _brushSizeEvent = brushSizeEvent;
            _saveLoadEvent = saveLoadEvent;
            _saveLoadService = saveLoadService;

            _saveLoadEvent.OnSave += SaveTexture;
            _saveLoadEvent.OnLoad += LoadTexture;
            _colorChangeEvent.OnColorChange += ChangeColor;
            _brushSizeEvent.OnBrushSizeChanged += ChangeBrushSize;
        }

        private void Awake()
        {
            _camera = Camera.main;
            InitializeTexture();
        }

        private void Update()
        {
            HandlePainting();

            if (Input.GetKeyDown(KeyCode.R))
            {
                _saveLoadService.LoadTexture(_texture);
            }
        }

        private void HandlePainting()
        {
            Vector2 pointerPosition = _inputService.GetPointerPosition;
        
            if (pointerPosition != Vector2.zero && Input.GetMouseButton(0))
            {
                if (TryGetUVCoordinates(pointerPosition, out int pixelX, out int pixelY))
                {
                    if (_lastRayX != pixelX || _lastRayY != pixelY)
                    {
                        DrawCircle(pixelX, pixelY);
                        _lastRayX = pixelX;
                        _lastRayY = pixelY;
                    }
                    _texture.Apply();
                }
            }
        }


        private bool TryGetUVCoordinates(Vector2 pointerPosition, out int pixelX, out int pixelY)
        {
            pixelX = pixelY = 0;
            Ray ray = _camera.ScreenPointToRay(pointerPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2 uv = hit.textureCoord;
                pixelX = Mathf.FloorToInt(uv.x * _texture.width);
                pixelY = Mathf.FloorToInt(uv.y * _texture.height);
                return true;
            }
            return false;
        }

        private void DrawCircle(int centerX, int centerY)
        {
            int radius = _brushSize / 2;
            float radiusSquared = radius * radius;

            for (int y = -radius; y < radius; y++)
            {
                for (int x = -radius; x < radius; x++)
                {
                    if (x * x + y * y < radiusSquared)
                    {
                        int pixelX = centerX + x;
                        int pixelY = centerY + y;

                        if (IsWithinTextureBounds(pixelX, pixelY))
                        {
                            BlendPixel(pixelX, pixelY, _brushColor);
                        }
                    }
                }
            }
        }

        private void BlendPixel(int x, int y, Color color)
        {
            Color existingColor = _texture.GetPixel(x, y);
            Color blendedColor = Color.Lerp(existingColor, color, color.a);
            _texture.SetPixel(x, y, blendedColor);
        }

        private void InitializeTexture()
        {
            _texture = new Texture2D(_textureSize, _textureSize);
            Color initialColor = new Color(188f / 255f, 188f / 255f, 188f / 255f);

            for (int y = 0; y < _texture.height; y++)
            {
                for (int x = 0; x < _texture.width; x++)
                {
                    _texture.SetPixel(x, y, initialColor);
                }
            }

            _texture.filterMode = FilterMode.Point;
            _texture.Apply();
            meshRenderer.material.mainTexture = _texture;
        }

        private void LoadTexture() => _saveLoadService.LoadTexture(_texture);
        private void SaveTexture() => _saveLoadService.SaveTexture(_texture);
        private bool IsWithinTextureBounds(int x, int y) => x >= 0 && x < _texture.width && y >= 0 && y < _texture.height;
        private void ChangeBrushSize(int obj) => _brushSize = obj;
        private void ChangeColor(Color color) => _brushColor = color;
    }
}