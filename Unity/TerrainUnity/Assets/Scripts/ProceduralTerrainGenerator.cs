using UnityEngine;

[RequireComponent (typeof(Terrain))]
public class ProceduralTerrainGenerator : MonoBehaviour
{
    public int width = 513;
    public int height = 513;

    public float noiseScale = 50f;
    public float heightMultiplier = 20f;

    Terrain terrain;

    private void Start()
    {
        terrain = GetComponent<Terrain>();
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        float[,] heights = new float[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                float xCoord = (float)x / noiseScale;
                float yCoord = (float)y / noiseScale;

                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);

                heights[x, y] = noiseValue * heightMultiplier / 100f;
            }
        }

        terrain.terrainData.heightmapResolution = width;
        terrain.terrainData.SetHeights(0, 0, heights);

        Debug.Log("Terrain Generated!");
    }
}
