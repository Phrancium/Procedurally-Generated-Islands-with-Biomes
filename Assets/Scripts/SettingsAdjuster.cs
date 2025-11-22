using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsAdjuster : MonoBehaviour
{
    [Header("Island Width")]
    [SerializeField] private TextMeshProUGUI widthValue;
    public Slider widthSlider;

    [Header("Island Height")]
    [SerializeField] private TextMeshProUGUI heightValue;
    public Slider heightSlider;

    [Header("Island Gradient Width")]
    [SerializeField] private TextMeshProUGUI gradientWidthValue;
    public Slider gradientWidthSlider;

    [Header("Island Gradient Height")]
    [SerializeField] private TextMeshProUGUI gradientHeightValue;
    public Slider gradientHeightSlider;

    [Header("Island Noise")]
    [SerializeField] private TextMeshProUGUI noiseScaleValue1;
    public Slider noiseScale1Slider;

    [SerializeField] private TextMeshProUGUI noiseScaleValue2;
    public Slider noiseScale2Slider;

    [SerializeField] private TextMeshProUGUI noiseWeightValue1;
    public Slider noiseWeight1Slider;

    [SerializeField] private TextMeshProUGUI noiseWeightValue2;
    public Slider noiseWeight2Slider;

    [Header("Island Vertical Height")]
    [SerializeField] private TextMeshProUGUI islandHeightValue;
    public Slider islandHeightSlider;

    [Header("Voxel")]
    [SerializeField] private TextMeshProUGUI voxelSizeValue;
    public Slider voxelSizeSlider;

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
        widthValue.text = widthSlider.value.ToString();
    }

    public void UpdateHeightValue()
    {
        heightValue.text = heightSlider.value.ToString();
    }

    public void UpdateGradientWidthValue()
    {
        gradientWidthValue.text = gradientWidthSlider.value.ToString();
    }

    public void UpdateGradientHeightValue()
    {
        gradientHeightValue.text = gradientHeightSlider.value.ToString();
    }

    public void UpdateNoiseScaleValue1()
    {
        noiseScaleValue1.text = noiseScale1Slider.value.ToString();
    }

    public void UpdateNoiseScaleValue2()
    {
        noiseScaleValue2.text = noiseScale2Slider.value.ToString();
    }
    
    public void UpdateNoiseWeightValue1()
    {
        noiseWeightValue1.text = noiseWeight1Slider.value.ToString();
    }

    public void UpdateNoiseWeightValue2()
    {
        noiseWeightValue2.text = noiseWeight2Slider.value.ToString();
    }
    
    public void UpdateIslandHeightValue()
    {
        islandHeightValue.text = islandHeightSlider.value.ToString();
    }
    
    public void UpdateVoxelSizeValue()
    {
        voxelSizeValue.text = voxelSizeSlider.value.ToString();
    }


    public void LoadScene()
    {
        SettingsData.width = (int)widthSlider.value;
        SettingsData.height = (int)heightSlider.value;
        SettingsData.gradientWidth = (int)gradientWidthSlider.value;
        SettingsData.gradientHeight = (int)gradientHeightSlider.value;
        SettingsData.noiseScale1 = (int)noiseScale1Slider.value;
        SettingsData.noiseScale2 = (int)noiseScale2Slider.value;
        SettingsData.noiseWeight1 = (int)noiseWeight1Slider.value;
        SettingsData.noiseWeight2 = (int)noiseWeight2Slider.value;
        SettingsData.islandHeight = (int)islandHeightSlider.value;
        SettingsData.voxelSize = (int)voxelSizeSlider.value;

        settingsPanel.SetActive(false);
        loadingPanel.SetActive(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }
}
