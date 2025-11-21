using UnityEngine;

/// <summary>
/// Visualizes your custom Perlin noise as an image texture
/// Attach this to a GameObject to see your noise in action
/// </summary>
public class NoiseVisualizer : MonoBehaviour
{
    [Header("Noise Parameters")]
    [SerializeField] private int width = 512;
    [SerializeField] private int height = 512;
    [SerializeField] private int gridSizeX = 50;
    [SerializeField] private int gridSizeY = 50;
    [SerializeField] private float amplitude = 1.0f;
    [SerializeField] private int seed = 12345;

    [Header("Visualization")]
    [SerializeField] private bool generateOnStart = true;
    [SerializeField] private bool showInSceneView = true;
    [SerializeField] private KeyCode regenerateKey = KeyCode.Space;

    [Header("Color Mapping")]
    [SerializeField] private bool useColorGradient = false;
    [SerializeField] private Gradient colorGradient;

    private Texture2D noiseTexture;
    private float[,] noiseData;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    void Start()
    {
        // Create default gradient if none exists
        if (colorGradient == null)
        {
            CreateDefaultGradient();
        }

        if (generateOnStart)
        {
            GenerateAndDisplayNoise();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(regenerateKey))
        {
            seed = Random.Range(0, 100000);
            GenerateAndDisplayNoise();
        }
    }

    /// <summary>
    /// Main method: Generate noise and convert to texture
    /// </summary>
    public void GenerateAndDisplayNoise()
    {
        // Step 1: Generate noise data using your custom Perlin noise
        GenerateNoise();

        // Step 2: Convert to texture
        CreateTextureFromNoise();

        // Step 3: Display on a quad mesh
        DisplayTexture();

        Debug.Log($"Noise texture generated! Press {regenerateKey} to regenerate with new seed.");
    }

    /// <summary>
    /// Generate noise using your custom Perlin noise function
    /// </summary>
    void GenerateNoise()
    {
        // Create array to hold noise values
        noiseData = new float[width, height];

        // Use your custom Perlin noise implementation
        SimpleProceduralGeneration.PerlinNoise(noiseData, gridSizeX, gridSizeY, amplitude);

        Debug.Log($"Generated noise array: {width}x{height}");
    }

    /// <summary>
    /// Convert noise data to a Unity Texture2D
    /// </summary>
    void CreateTextureFromNoise()
    {
        // Create texture
        noiseTexture = new Texture2D(width, height);
        noiseTexture.filterMode = FilterMode.Bilinear;
        noiseTexture.wrapMode = TextureWrapMode.Clamp;

        // Find min and max for normalization
        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float value = noiseData[x, y];
                if (value < minValue) minValue = value;
                if (value > maxValue) maxValue = value;
            }
        }

        Debug.Log($"Noise range: [{minValue}, {maxValue}]");

        // Convert noise values to colors
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Normalize to 0-1 range
                float normalizedValue = (noiseData[x, y] - minValue) / (maxValue - minValue);

                Color pixelColor;
                if (useColorGradient && colorGradient != null)
                {
                    // Use gradient for coloring
                    pixelColor = colorGradient.Evaluate(normalizedValue);
                }
                else
                {
                    // Default: grayscale
                    pixelColor = new Color(normalizedValue, normalizedValue, normalizedValue);
                }

                noiseTexture.SetPixel(x, y, pixelColor);
            }
        }

        // Apply all SetPixel changes
        noiseTexture.Apply();

        Debug.Log("Texture created from noise data");
    }

    /// <summary>
    /// Display the texture on a quad mesh in the scene
    /// </summary>
    void DisplayTexture()
    {
        // Get or create MeshRenderer
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
        }

        // Get or create MeshFilter
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
        }

        // Create quad mesh if needed
        if (meshFilter.sharedMesh == null)
        {
            meshFilter.mesh = CreateQuadMesh();
        }

        // Create material and assign texture
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.mainTexture = noiseTexture;
        meshRenderer.material = mat;

        Debug.Log("Texture displayed on quad");
    }

    /// <summary>
    /// Create a simple quad mesh to display the texture
    /// </summary>
    Mesh CreateQuadMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Noise Visualization Quad";

        float aspectRatio = (float)width / height;
        float quadHeight = 10f;
        float quadWidth = quadHeight * aspectRatio;

        // Vertices
        mesh.vertices = new Vector3[]
        {
            new Vector3(-quadWidth/2, -quadHeight/2, 0),
            new Vector3(quadWidth/2, -quadHeight/2, 0),
            new Vector3(-quadWidth/2, quadHeight/2, 0),
            new Vector3(quadWidth/2, quadHeight/2, 0)
        };

        // UVs
        mesh.uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        // Triangles
        mesh.triangles = new int[] { 0, 2, 1, 2, 3, 1 };

        mesh.RecalculateNormals();
        return mesh;
    }

    /// <summary>
    /// Save the texture as a PNG file
    /// </summary>
    public void SaveTextureAsPNG(string filename = "noise_output.png")
    {
        if (noiseTexture == null)
        {
            Debug.LogError("No texture to save! Generate noise first.");
            return;
        }

        byte[] bytes = noiseTexture.EncodeToPNG();
        string path = Application.dataPath + "/" + filename;
        System.IO.File.WriteAllBytes(path, bytes);

        Debug.Log($"Texture saved to: {path}");
    }

    /// <summary>
    /// Create a default color gradient (black to white)
    /// </summary>
    void CreateDefaultGradient()
    {
        colorGradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(Color.black, 0.0f);
        colorKeys[1] = new GradientColorKey(Color.white, 1.0f);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphaKeys[1] = new GradientAlphaKey(1.0f, 1.0f);

        colorGradient.SetKeys(colorKeys, alphaKeys);
    }

    /// <summary>
    /// Get the generated texture (useful for external scripts)
    /// </summary>
    public Texture2D GetTexture()
    {
        return noiseTexture;
    }

    /// <summary>
    /// Get the raw noise data array
    /// </summary>
    public float[,] GetNoiseData()
    {
        return noiseData;
    }

    // Optional: Draw texture in Scene view
    void OnDrawGizmos()
    {
        if (!showInSceneView || noiseTexture == null) return;

        // This will show the noise in the Scene view
        // (Game view display is handled by the mesh)
    }
}