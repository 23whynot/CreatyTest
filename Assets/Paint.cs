using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class Paint3D : MonoBehaviour
{
    [Range(2, 512)] [SerializeField] private int _textureSize = 128;
    [SerializeField] private TextureWrapMode _textureWrapMode;
    [SerializeField] private FilterMode _filterMode;
    [SerializeField] private Color _color;
    [SerializeField] private int _brushSize = 8;
    [SerializeField] private Camera _camera;
    [SerializeField] private MeshRenderer _meshRenderer;

    private Texture2D _texture;
    private int _oldRayX, _oldRayY;

    private string _savePath;

    void Start()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "textureData.json");

        if (_meshRenderer.material.mainTexture == null)
        {
            _texture = new Texture2D(_textureSize, _textureSize);
            LoadTexture();
            _meshRenderer.material.mainTexture = _texture;
        }
        else
        {
            _texture = (Texture2D)_meshRenderer.material.mainTexture;
        }

        _texture.wrapMode = _textureWrapMode;
        _texture.filterMode = _filterMode;
        _texture.Apply();
    }

    private void Update()
    {
        _brushSize += (int)Input.mouseScrollDelta.y;

        if (Input.GetMouseButton(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector2 uv = hit.textureCoord;
                int rayX = (int)(uv.x * _texture.width);
                int rayY = (int)(uv.y * _texture.height);

                if (_oldRayX != rayX || _oldRayY != rayY)
                {
                    DrawCircle(rayX, rayY);
                    _oldRayX = rayX;
                    _oldRayY = rayY;
                }

                _texture.Apply();
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveTexture();
            Debug.Log("Texture saved to JSON.");
        }
    }

    void DrawCircle(int rayX, int rayY)
    {
        for (int y = 0; y < _brushSize; y++)
        {
            for (int x = 0; x < _brushSize; x++)
            {
                float x2 = Mathf.Pow(x - _brushSize / 2, 2);
                float y2 = Mathf.Pow(y - _brushSize / 2, 2);
                float r2 = Mathf.Pow(_brushSize / 2 - 0.5f, 2);

                if (x2 + y2 < r2)
                {
                    int pixelX = rayX + x - _brushSize / 2;
                    int pixelY = rayY + y - _brushSize / 2;

                    if (pixelX >= 0 && pixelX < _texture.width && pixelY >= 0 && pixelY < _texture.height)
                    {
                        Color oldColor = _texture.GetPixel(pixelX, pixelY);
                        Color resultColor = Color.Lerp(oldColor, _color, _color.a);
                        _texture.SetPixel(pixelX, pixelY, resultColor);
                    }
                }
            }
        }
    }

    void SaveTexture()
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

        string json = JsonUtility.ToJson(new TextureData(_texture.width, _texture.height, colorDataList));
        
        File.WriteAllText(_savePath, json);
    }

    void LoadTexture()
    {
        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            TextureData textureData = JsonUtility.FromJson<TextureData>(json);

            _texture = new Texture2D(textureData.width, textureData.height);

            foreach (var colorData in textureData.colors)
            {
                _texture.SetPixel(colorData.x, colorData.y,
                    new Color(colorData.r, colorData.g, colorData.b, colorData.a));
            }

            _texture.Apply();
        }
        else
        {
            _texture = new Texture2D(_textureSize, _textureSize);
        }
    }
}