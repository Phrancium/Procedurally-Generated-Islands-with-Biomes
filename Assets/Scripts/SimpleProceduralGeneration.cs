using UnityEngine;
using System.Collections;

public class SimpleProceduralGeneration : MonoBehaviour
{
    [Header("Island Dimensions")]
    [SerializeField] private int width = 800;
    [SerializeField] private int height = 600;

    [Header("Noise Settings")]
    [SerializeField] private float noiseScale1 = 0.003f;
    [SerializeField] private float noiseScale2 = 0.008f;

    [SerializeField] private float noiseWeight1 = 0.7f;
    [SerializeField] private float noiseWeight2 = 0.3f;
    [SerializeField] private float noiseStrength = 0.25f;

    [Header("Island Shape")]
    [SerializeField] private float waterLevel = 0.4f;
    [SerializeField] private bool useDistanceDistortion = true;
    [SerializeField] private float distortionAmount = 1.0f;

    [Header("Shading")]
    [SerializeField] private bool enableHillshading = true;
    [SerializeField] private float hillshadeStrength = 10000f;

    [Header("Colors")]
    [SerializeField] private Color deepWaterColor = new Color(0.0f, 0.2f, 0.5f);
    [SerializeField] private Color shallowWaterColor = new Color(0.2f, 0.4f, 0.7f);
    [SerializeField] private Color sandColor = new Color(0.9f, 0.9f, 0.6f);
    [SerializeField] private Color grassColor = new Color(0.2f, 0.6f, 0.2f);
    [SerializeField] private Color rockColor = new Color(0.5f, 0.5f, 0.5f);
    [SerializeField] private Color snowColor = new Color(0.95f, 0.95f, 0.95f);

    [Header("Rendering")]
    [SerializeField] private bool generateOnStart = true;
    [SerializeField] private Material terrainMaterial;

    private float[,] terrainData;
    private Texture2D islandTexture;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    private int seed;

    void Start()
    {
        if (generateOnStart)
        {
            GenerateIsland();
        }
    }

    public void GenerateIsland()
    {
        seed = Random.Range(0, 100000);

        terrainData = GenerateRadialGradient();
        ApplyNoiseLayers();

        if (useDistanceDistortion)
        {
            ApplyDistanceDistortion();
        }

        CreateIslandTexture();

        ApplyToMesh();
    }


    private float[,] GenerateRadialGradient()
    {
        float[,] gradient = new float[width, height];

        // Calculate center point
        float centerX = width / 2f;
        float centerY = height / 2f;

        // Calculate maximum radius for normalization
        float maxRadius = Mathf.Sqrt(centerX * centerX + centerY * centerY);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Calculate Euclidean distance
                float xDistance = Mathf.Abs(centerX - x);
                float yDistance = Mathf.Abs(centerY - y);

                float distance = Mathf.Sqrt(xDistance * xDistance + yDistance * yDistance);
                float normalizedDistance = 1f - (distance / maxRadius);

                gradient[x, y] = distance;
            }
        }

        return gradient;
    }


    private void ApplyNoiseLayers()
    {
        // Use seed for consistent random offsets
        Random.InitState(seed);
        float offset1X = Random.Range(0f, 1000f);
        float offset1Y = Random.Range(0f, 1000f);
        float offset2X = Random.Range(0f, 1000f);
        float offset2Y = Random.Range(0f, 1000f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // First noise layer - large scale features
                float noise1 = Mathf.PerlinNoise(
                    (x + offset1X) * noiseScale1,
                    (y + offset1Y) * noiseScale1
                );

                // Second noise layer - fine details
                float noise2 = Mathf.PerlinNoise(
                    (x + offset2X) * noiseScale2,
                    (y + offset2Y) * noiseScale2
                );

                // Combine noise layers with weights
                float combinedNoise = (noise1 * noiseWeight1 + noise2 * noiseWeight2);

                // Apply noise to gradient
                terrainData[x, y] -= (combinedNoise * noiseStrength);

                // Clamp to valid range
                terrainData[x, y] = Mathf.Clamp01(terrainData[x, y]);
            }
        }
    }

    private void ApplyDistanceDistortion()
    {
        float centerX = width / 2f;
        float centerY = height / 2f;
        float maxRadius = Mathf.Sqrt(centerX * centerX + centerY * centerY);

        // Create a new distorted gradient
        float[,] distortedData = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xDistance = Mathf.Abs(centerX - x);
                float yDistance = Mathf.Abs(centerY - y);
                float distance = Mathf.Sqrt(xDistance * xDistance + yDistance * yDistance);

                Random.InitState(seed + x * height + y); // Deterministic randomness per pixel
                float randomPower = 2f - Random.Range(0f, distortionAmount);

                // Apply the distortion power
                float distortedDistance = Mathf.Pow(distance / maxRadius, randomPower);
                float distortedValue = 1f - distortedDistance;

                // Blend with existing terrain data
                distortedData[x, y] = terrainData[x, y] * distortedValue;
            }
        }

        terrainData = distortedData;
    }

    private void CreateIslandTexture()
    {
        islandTexture = new Texture2D(width, height);
        islandTexture.filterMode = FilterMode.Point;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float height = terrainData[x, y];

                // Calculate hillshading if enabled
                float hillshade = 0f;
                if (enableHillshading && x > 0)
                {
                    float heightDifference = terrainData[x, y] - terrainData[x - 1, y];
                    hillshade = heightDifference * hillshadeStrength;
                }

                // Get base color based on height
                Color pixelColor = GetColorForHeight(height);

                // Apply hillshading to RGB channels
                if (enableHillshading)
                {
                    pixelColor.r = Mathf.Clamp01(pixelColor.r + hillshade / 255f);
                    pixelColor.g = Mathf.Clamp01(pixelColor.g + hillshade / 255f);
                    pixelColor.b = Mathf.Clamp01(pixelColor.b + hillshade / 255f);
                }

                islandTexture.SetPixel(x, y, pixelColor);
            }
        }

        islandTexture.Apply();
    }

    private Color GetColorForHeight(float height)
    {
        if (height < waterLevel)
        {
            float waterDepth = height / waterLevel;
            return Color.Lerp(deepWaterColor, shallowWaterColor, waterDepth);
        }

        float landHeight = (height - waterLevel) / (1f - waterLevel);

        // Create elevation-based color bands
        if (landHeight < 0.1f)
        {
            return Color.Lerp(shallowWaterColor, sandColor, landHeight * 10f);
        }
        else if (landHeight < 0.5f)
        {
            return Color.Lerp(sandColor, grassColor, (landHeight - 0.1f) / 0.4f);
        }
        else if (landHeight < 0.8f)
        {
            return Color.Lerp(grassColor, rockColor, (landHeight - 0.5f) / 0.3f);
        }
        else
        {
            return Color.Lerp(rockColor, snowColor, (landHeight - 0.8f) / 0.2f);
        }
    }

    private void ApplyToMesh()
    {
        // Get or create mesh components
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
        }

        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
        }

        // Create a simple quad mesh if none exists
        if (meshFilter.sharedMesh == null)
        {
            CreateQuadMesh();
        }

        // Apply texture to material
        if (terrainMaterial == null)
        {
            terrainMaterial = new Material(Shader.Find("Unlit/Texture"));
        }

        terrainMaterial.mainTexture = islandTexture;
        meshRenderer.material = terrainMaterial;
    }

    private void CreateQuadMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Island Quad";

        // Calculate aspect ratio
        float aspectRatio = (float)width / height;
        float quadHeight = 10f;
        float quadWidth = quadHeight * aspectRatio;

        // Create vertices for a quad
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-quadWidth/2, -quadHeight/2, 0),
            new Vector3(quadWidth/2, -quadHeight/2, 0),
            new Vector3(-quadWidth/2, quadHeight/2, 0),
            new Vector3(quadWidth/2, quadHeight/2, 0)
        };

        // UV coordinates
        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        // Triangle indices
        int[] triangles = new int[6]
        {
            0, 2, 1,
            2, 3, 1
        };

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    public void RegenerateWithNewSeed()
    {
        seed = Random.Range(0, 100000);
        GenerateIsland();
    }

    public float[,] GetTerrainData()
    {
        return terrainData;
    }


    public Texture2D GetTexture()
    {
        return islandTexture;
    }


    public Mesh Generate3DMesh(float heightMultiplier = 2f, int resolution = 4)
    {
        int meshWidth = width / resolution;
        int meshHeight = height / resolution;

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[meshWidth * meshHeight];
        Vector2[] uv = new Vector2[meshWidth * meshHeight];
        int[] triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];

        // Generate vertices
        for (int y = 0; y < meshHeight; y++)
        {
            for (int x = 0; x < meshWidth; x++)
            {
                int terrainX = x * resolution;
                int terrainY = y * resolution;

                float height = terrainData[terrainX, terrainY];

                vertices[y * meshWidth + x] = new Vector3(
                    x - meshWidth / 2f,
                    height * heightMultiplier,
                    y - meshHeight / 2f
                );

                uv[y * meshWidth + x] = new Vector2(
                    (float)x / meshWidth,
                    (float)y / meshHeight
                );
            }
        }

        // Generate triangles
        int triIndex = 0;
        for (int y = 0; y < meshHeight - 1; y++)
        {
            for (int x = 0; x < meshWidth - 1; x++)
            {
                int topLeft = y * meshWidth + x;
                int topRight = topLeft + 1;
                int bottomLeft = (y + 1) * meshWidth + x;
                int bottomRight = bottomLeft + 1;

                triangles[triIndex++] = topLeft;
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = topRight;

                triangles[triIndex++] = topRight;
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = bottomRight;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}