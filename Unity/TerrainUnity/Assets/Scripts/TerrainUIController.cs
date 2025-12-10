using UnityEngine;
using UnityEngine.UI;
public class TerrainUIController : MonoBehaviour 
{
    public ProceduralTerrainGenerator generator;

    public Slider noiseScaleSlider;
    public Slider heightMultiplierSlider;
    public Slider octavesSlider;
    public Slider persistenceSlider;
    public Slider lacunaritySlider;

    public Button regenerateButton;

    private void Start()
    {
        if (generator == null)
        {
            Debug.LogError("TerrainUIController: Generator is missing!");
            return;
        }

        noiseScaleSlider.onValueChanged.AddListener(OnNoiseScaleChanged);
        heightMultiplierSlider.onValueChanged.AddListener(OnHeightMultiplierChanged);
        octavesSlider.onValueChanged.AddListener(OnOctavesChanged);
        persistenceSlider.onValueChanged.AddListener(OnPersistenceChanged);
        lacunaritySlider.onValueChanged.AddListener(OnLacunarityChanged);

        // Button
        regenerateButton.onClick.AddListener(() =>
        {
            generator.GenerateTerrain();
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
