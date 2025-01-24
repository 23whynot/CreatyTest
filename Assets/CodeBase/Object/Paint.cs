using CodeBase.Brush;
using CodeBase.Infrastructure.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Object
{
    public class Paint : MonoBehaviour, ISavedTexture
    {
        [SerializeField] private MeshRenderer meshRenderer;

        private const int TextureSize = 512;
        private static readonly Color DefaultColor = new Color(188f / 255f, 188f / 255f, 188f / 255f);

        private IPaintBrush _paintBrush;
        private IInputService _inputService;

        public Texture2D texture { get; set; }

        [Inject]
        public void Construct(IInputService inputService, IPaintBrush paintBrush)
        {
            _inputService = inputService;
            _paintBrush = paintBrush;
        }

        private void Awake() => InitializeTexture();

        private void Update() => ProcessPainting();
        
        private void ProcessPainting()
        {
            Vector2 pointerPosition = _inputService.GetPointerPosition;
            if (IsPainting(pointerPosition) && TryGetUVCoordinates(pointerPosition, out int pixelX, out int pixelY))
            {
                _paintBrush.Paint(texture, pixelX, pixelY);
                texture.Apply();
            }
        }
        
        private bool TryGetUVCoordinates(Vector2 pointerPosition, out int pixelX, out int pixelY)
        {
            pixelX = pixelY = 0;
            Ray ray = Camera.main.ScreenPointToRay(pointerPosition);
            return Physics.Raycast(ray, out RaycastHit hit) && SetUVCoordinates(hit, out pixelX, out pixelY);
        }

        private bool SetUVCoordinates(RaycastHit hit, out int pixelX, out int pixelY)
        {
            Vector2 uv = hit.textureCoord;
            pixelX = Mathf.FloorToInt(uv.x * texture.width);
            pixelY = Mathf.FloorToInt(uv.y * texture.height);
            return true;
        }
        
        private void InitializeTexture()
        {
            texture = new Texture2D(TextureSize, TextureSize);
            FillTextureWithColor(DefaultColor);

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            meshRenderer.material.mainTexture = texture;
        }
        
        private void FillTextureWithColor(Color color)
        {
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, color);
                }
            }
        }
        
        private bool IsPainting(Vector2 pointerPosition) => pointerPosition != Vector2.zero && Input.GetMouseButton(0);
    }
}