using UnityEngine;

[RequireComponent (typeof(Terrain))]
public class ProceduralTerrainGenerator : MonoBehaviour
{
    public int width = 513;
    public int height = 513;
    public float heightMultiplier = 20f;

    public float noiseScale = 50f;
    public int octaves = 4;
    public float persistence = 0.5f;
    public float lacunarity = 2f;

    public int seed = 0;
    public Vector2 offset;

    Terrain terrain;

    private void Start()
    {
        terrain = GetComponent<Terrain>();
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        float[,] heights = new float[width, height];

        System.Random rnd = new System.Random();
        float offsetX = offset.x + rnd.Next(-100000, 100000);
        float offsetY = offset.y + rnd.Next(-100000, 100000);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (x / noiseScale * frequency) + offsetX;
                    float yCoord = (y / noiseScale * frequency) + offsetY;

                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord);

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }
                heights[x, y] = noiseHeight * heightMultiplier / 100f;

            }
        }
        terrain.terrainData.heightmapResolution = width;
        terrain.terrainData.SetHeights(0, 0, heights);

        Debug.Log("Terrain Generated!");
    }
}
