using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsAdjuster : MonoBehaviour
{
    [Header("Island Width")]
    public TMP_InputField widthInputField;

    [Header("Island Height")]
    public TMP_InputField heightInputField;

    [Header("Island Gradient Width")]
    public TMP_InputField gradientWidthInputField;

    [Header("Island Gradient Height")]
    public TMP_InputField gradientHeightInputField;

    [Header("Island Noise")]
    public TMP_InputField noiseScale1InputField;
    public TMP_InputField noiseScale2InputField;
    public TMP_InputField noiseWeight1InputField;
    public TMP_InputField noiseWeight2InputField;

    [Header("Water Level")]
    public TMP_InputField waterLevelInputField;

    [Header("Island Vertical Height")]
    [SerializeField] private TextMeshProUGUI islandHeightValue;
    public Slider islandHeightSlider;

    [Header("Voxel")]
    [SerializeField] private TextMeshProUGUI voxelSizeValue;
    public Slider voxelSizeSlider;

    [Header("Island Style")]
    [SerializeField] private Toggle voxelToggle;
    [SerializeField] private Toggle radialToggle;
    [SerializeField] private TMP_Dropdown biomeDropdown;
    private string biomeType = "Default";


    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TextMeshProUGUI loadingText;
    private string[] loadingTexts = { "Generating World", "Generating World.", "Generating World..", "Generating World..." };
    private float timer = 0.0f;
    private float duration = 0.5f;
    private int step = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration && step == 0)
        {
            step++;
            loadingText.text = loadingTexts[step];
            timer = 0.0f;
        }
        else if (timer > duration && step == 1)
        {
            step++;
            loadingText.text = loadingTexts[step];
            timer = 0.0f;
        }
        else if(timer > duration && step == 2)
        {
            step++;
            loadingText.text = loadingTexts[step];
            timer = 0.0f;
        }
        else if (timer > duration && step == 3)
        {
            step = 0;
            loadingText.text = loadingTexts[step];
            timer = 0.0f;
        }

    }

    public void UpdateWidthValue()
    {
        int widthValue = int.Parse(widthInputField.text);
        if (widthValue < 50 || widthValue > 3200)
        {
            widthInputField.text = "50";
        }
    }

    public void UpdateHeightValue()
    {
        int heightValue = int.Parse(heightInputField.text);
        if (heightValue < 50 || heightValue > 3200)
        {
            heightInputField.text = "50";
        }
    }

    public void UpdateGradientWidthValue()
    {
        int gradientWidthValue = int.Parse(gradientWidthInputField.text);
        if (gradientWidthValue < 4 || gradientWidthValue > 20)
        {
            gradientWidthInputField.text = "4";
        }
    }

    public void UpdateGradientHeightValue()
    {
        int gradientHeightValue = int.Parse(gradientHeightInputField.text);
        if (gradientHeightValue < 4 || gradientHeightValue > 20)
        {
            gradientHeightInputField.text = "4";
        }
    }

    public void UpdateNoiseScaleValue1()
    {
        float noiseScaleValue = float.Parse(noiseScale1InputField.text);
        if (noiseScaleValue < 0.0 || noiseScaleValue > 1.0)
        {
            noiseScale1InputField.text = "0.0";
        }
    }

    public void UpdateNoiseScaleValue2()
    {
        float noiseScaleValue = float.Parse(noiseScale2InputField.text);
        if (noiseScaleValue < 0.0 || noiseScaleValue > 1.0)
        {
            noiseScale2InputField.text = "0.0";
        }
    }
    
    public void UpdateNoiseWeightValue1()
    {
        float noiseWeightValue = float.Parse(noiseWeight1InputField.text);
        if (noiseWeightValue < 0.0 || noiseWeightValue > 1.0)
        {
            noiseWeight1InputField.text = "0.0";
        }
    }

    public void UpdateNoiseWeightValue2()
    {
        float noiseWeightValue = float.Parse(noiseWeight2InputField.text);
        if (noiseWeightValue < 0.0 || noiseWeightValue > 1.0)
        {
            noiseWeight2InputField.text = "0.0";
        }
    }

    public void UpdateWaterLevelValue()
    {
        float waterLevelValue = float.Parse(waterLevelInputField.text);
        if (waterLevelValue < 0.0 || waterLevelValue > 1.0)
        {
            waterLevelInputField.text = "0.0";
        }
    }
    
    public void UpdateIslandHeightValue()
    {
        islandHeightValue.text = islandHeightSlider.value.ToString();
    }
    
    public void UpdateVoxelSizeValue()
    {
        voxelSizeValue.text = voxelSizeSlider.value.ToString();
    }

    public void UpdateBiomeSelection()
    {
        biomeType = biomeDropdown.options[biomeDropdown.value].text;
        print("Selected Biome: " + biomeType);
    }

    public Color[] SetBiomeColors(string selectedBiome)
    {
        // order: deep, shallow, sand, grass, rock, snow 
        Color[] biomeColors;
        switch (selectedBiome)
        {
            case "Default":
                biomeColors =  new Color[] { 
                    new Color(0.0f, 0.2f, 0.5f), 
                    new Color(0.2f, 0.4f, 0.7f), 
                    new Color(0.9f, 0.9f, 0.6f), 
                    new Color(0.2f, 0.6f, 0.2f), 
                    new Color(0.5f, 0.5f, 0.5f), 
                    new Color(0.95f, 0.95f, 0.95f)
                };
                return biomeColors;
            case "Mountain":
                biomeColors = new Color[] {
                    new Color(0.0f, 0.3f, 0.1f),
                    new Color(0.1f, 0.4f, 0.15f),
                    new Color(0.2f, 0.2f, 0.2f),
                    new Color(0.45f, 0.45f, 0.45f),
                    new Color(0.8f, 0.8f, 0.8f),
                    new Color(0.95f, 0.95f, 0.95f)
                };
                return biomeColors;
            case "Desert":
                biomeColors = new Color[] {
                    new Color(0.2f, 0.02f, 0.0f),
                    new Color(0.45f, 0.0f, 0.05f),
                    new Color(0.9f, 0.4f, 0.0f),
                    new Color(1.0f, 0.8f, 0.4f),
                    new Color(0.55f, 0.2f, 0.0f),
                    new Color(0.55f, 0.2f, 0.0f)
                };
                return biomeColors;
            case "Island":
                biomeColors = new Color[] {
                    new Color(0.0f, 0.45f, 0.5f),
                    new Color(0.2f, 0.75f, 0.75f),
                    new Color(0.7f, 0.5f, 0.2f),
                    new Color(1.0f, 0.9f, 0.5f),
                    new Color(1.0f, 0.95f, 0.8f),
                    new Color(1.0f, 0.95f, 0.8f)
                };
                return biomeColors;
        }
        return null;
    }


    public void LoadScene()
    {
        SettingsData.width = int.Parse(widthInputField.text); ;
        SettingsData.height = int.Parse(heightInputField.text);
        SettingsData.gradientWidth = int.Parse(gradientWidthInputField.text);
        SettingsData.gradientHeight = int.Parse(gradientHeightInputField.text);
        SettingsData.noiseScale1 = float.Parse(noiseScale1InputField.text);
        SettingsData.noiseScale2 = float.Parse(noiseScale2InputField.text);
        SettingsData.noiseWeight1 = float.Parse(noiseWeight1InputField.text);
        SettingsData.noiseWeight2 = float.Parse(noiseWeight2InputField.text);
        SettingsData.waterLevel = float.Parse(waterLevelInputField.text);
        SettingsData.islandHeight = (int)islandHeightSlider.value;
        SettingsData.voxelSize = (int)voxelSizeSlider.value;
        SettingsData.isVoxel = voxelToggle.isOn;
        SettingsData.isRadial = radialToggle.isOn;
        SettingsData.biomeType = biomeType;
        Color[] biomeColors = SetBiomeColors(biomeType);
        
        SettingsData.deepColor = biomeColors[0];
        SettingsData.shallowColor = biomeColors[1];
        SettingsData.sandColor = biomeColors[2];
        SettingsData.grassColor = biomeColors[3];
        SettingsData.rockColor = biomeColors[4];
        SettingsData.snowColor = biomeColors[5];
        

        settingsPanel.SetActive(false);
        loadingPanel.SetActive(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }
}
