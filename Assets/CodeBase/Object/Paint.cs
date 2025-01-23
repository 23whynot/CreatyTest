using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class Paint3D : MonoBehaviour
{
    [SerializeField] private int _textureSize = 512;
    [SerializeField] private Color _brushColor = Color.red;
    [SerializeField] private int _brushSize = 8;
    [SerializeField] private MeshRenderer _meshRenderer;

    private Camera _camera;
    private Texture2D _texture;
    private int _lastRayX, _lastRayY;
    private string _savePath;

    private void Awake()
    {
        _camera = Camera.main;
        _savePath = Path.Combine(Application.persistentDataPath, "textureData.json");

        InitializeTexture();
    }

    private void Update()
    {
        HandleBrushSizeAdjustment();
        HandlePainting();
        HandleTextureSave();
    }

    private void HandleBrushSizeAdjustment()
    {
        _brushSize = Mathf.Max(1, _brushSize + (int)Input.mouseScrollDelta.y);
    }

    private void HandlePainting()
    {
        if (Input.GetMouseButton(0))
        {
            if (TryGetUVCoordinates(out int pixelX, out int pixelY))
            {
                if (_lastRayX != pixelX || _lastRayY != pixelY)
                {
                    DrawCircle(pixelX, pixelY);
                    _lastRayX = pixelX;
                    _lastRayY = pixelY;
                }

                _texture.filterMode = FilterMode.Point;
                _texture.Apply();
            }
        }
    }

    private void HandleTextureSave()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveTexture();
            Debug.Log($"Texture saved to {_savePath}");
        }
    }

    private bool TryGetUVCoordinates(out int pixelX, out int pixelY)
    {
        pixelX = pixelY = 0;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
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

    private bool IsWithinTextureBounds(int x, int y)
    {
        return x >= 0 && x < _texture.width && y >= 0 && y < _texture.height;
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

        _texture.Apply();
        _meshRenderer.material.mainTexture = _texture;
    }

    private void SaveTexture()
    {
        List<ColorData> colorDataList = new List<ColorData>();

        for (int y = 0; y < _texture.height; y++)
        {
            for (int x = 0; x < _texture.width; x++)
            {
                Color pixelColor = _texture.GetPixel(x, y);
                colorDataList.Add(new ColorData(x, y, pixelColor));
            }
        }

        string json = JsonUtility.ToJson(new TextureData(_texture.width, _texture.height, colorDataList), true);
        File.WriteAllText(_savePath, json);
    }
}