using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TerrainUIController : MonoBehaviour 
{
    public ProceduralTerrainGenerator generator;

    public Slider noiseScaleSlider;
    public Slider heightMultiplierSlider;
    public Slider octavesSlider;
    public Slider persistenceSlider;
    public Slider lacunaritySlider;

    public Button regenerateButton;

    public TMP_Text seedValueText;
    public Button randomiseSeedButton;

    private void Start()
    {
        if (generator == null)
        {
            Debug.LogError("TerrainUIController: Generator is missing!");
            return;
        }

        if (seedValueText != null )
        {
            seedValueText.text = generator.seed.ToString();
        }

        noiseScaleSlider.onValueChanged.AddListener(OnNoiseScaleChanged);
        heightMultiplierSlider.onValueChanged.AddListener(OnHeightMultiplierChanged);
        octavesSlider.onValueChanged.AddListener(OnOctavesChanged);
        persistenceSlider.onValueChanged.AddListener(OnPersistenceChanged);
        lacunaritySlider.onValueChanged.AddListener(OnLacunarityChanged);

        // Regenerate Terrain Button
        regenerateButton.onClick.AddListener(() =>
        {
            generator.GenerateTerrain();
        });

        // Randomise Seed Button
        randomiseSeedButton.onClick.AddListener(() =>
        {
            generator.RandomiseSeed();
            seedValueText.text = generator.seed.ToString();
        });

        noiseScaleSlider.value = generator.noiseScale;
        heightMultiplierSlider.value = generator.heightMultiplier;
        octavesSlider.value = generator.octaves;
        persistenceSlider.value = generator.persistence;
        lacunaritySlider.value = generator.lacunarity;
    }

    private void OnNoiseScaleChanged(float value)
    {
        generator.noiseScale = value;
    }

    private void OnHeightMultiplierChanged(float value)
    {
        generator.heightMultiplier = value;
    }

    private void OnOctavesChanged(float value)
    {
        generator.octaves = Mathf.RoundToInt(value);
    }

    private void OnPersistenceChanged(float value)
    {
        generator.persistence = value;
    }

    private void OnLacunarityChanged(float value)
    {
        generator.lacunarity = value;
    }
}
