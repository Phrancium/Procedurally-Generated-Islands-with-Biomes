using UnityEngine;

public static class SettingsData
{
    public static int width = 500;
    public static int height = 500;
    public static int gradientWidth = 4;
    public static int gradientHeight = 4;

    public static float noiseScale1 = 0.05f;
    public static float noiseScale2 = 0.08f;
    public static float noiseWeight1 = 0.7f;
    public static float noiseWeight2 = 0.3f;

    public static float waterLevel = 0.1f;
    public static float islandHeight = 15f;
    public static int voxelSize = 20;

    public static bool isVoxel = true;
    public static bool isRadial = false;


    public static string biomeType = "Default";
    public static Color deepColor = new Color(0.0f, 0.2f, 0.5f);
    public static Color shallowColor = new Color(0.2f, 0.4f, 0.7f);
    public static Color sandColor = new Color(0.9f, 0.9f, 0.6f);
    public static Color grassColor = new Color(0.2f, 0.6f, 0.2f);
    public static Color rockColor = new Color(0.5f, 0.5f, 0.5f);
    public static Color snowColor = new Color(0.95f, 0.95f, 0.95f);
}
