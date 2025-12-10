using UnityEngine;
using System.Collections.Generic;

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

    public TerrainLayer grassLayer;
    public TerrainLayer rockLayer;
    public TerrainLayer snowLayer;

    [Range(0f, 1f)]
    public float rockStartHeight = 0.4f;
    [Range(0f, 1f)]
    public float snowStartHeight = 0.7f;
    [Range(0f, 90f)]
    public float slopeAngleThreshold = 35f;

    Terrain terrain;

    private void Start()
    {
        terrain = GetComponent<Terrain>();
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        float[,] heights = new float[width, height];

        System.Random rnd = new System.Random(seed);
        float offsetX = offset.x + rnd.Next(-99999, 99999);
        float offsetY = offset.y + rnd.Next(-99999, 99999);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (x / noiseScale * frequency) + offsetX + seed * 0.1f;
                    float yCoord = (y / noiseScale * frequency) + offsetY + seed * 0.1f;

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

        ApplyTextures();

        Debug.Log("Terrain Generated!");
    }

    public void ApplyTextures()
    {
        TerrainData data = terrain.terrainData;

        TerrainLayer[] layers = new TerrainLayer[3];
        layers[0] = grassLayer;
        layers[1] = rockLayer;
        layers[2] = snowLayer;

        data.terrainLayers = layers;

        int width = data.alphamapWidth;
        int height = data.alphamapHeight;

        float[,,] splatmapData = new float[width, height, 3];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float normX = x * 1.0f / (width - 1);
                float normY = y * 1.0f / (height - 1);

                float terrainHeight = data.GetHeight(Mathf.RoundToInt(normY * data.heightmapResolution),
                    Mathf.RoundToInt(normX * data.heightmapResolution))
                    / data.size.y;

                float slope = data.GetSteepness(normX, normY);

                float[] weights = new float[3];

                weights[0] = 1f;
                weights[1] = 0f;
                weights[2] = 0f;

                if(slope > slopeAngleThreshold)
                {
                    weights[0] = 0.4f;
                    weights[1] = 0.6f;
                }

                if(terrainHeight > snowStartHeight)
                {
                    weights[0] = 0f;
                    weights[1] = 0f;
                    weights[2] = 1f;
                }

               
                float total = weights[0] + weights[1] + weights[2];

                for(int i = 0; i < 3; i++)
                {
                    weights[i] /= total;
                }

                splatmapData[x, y, 0] = weights[0];
                splatmapData[x, y, 1] = weights[1];
                splatmapData[x, y, 2] = weights[2];
            }
        }
        data.SetAlphamaps(0, 0, splatmapData);

        Debug.Log("Procedural textures applied.");
    }
    public void RandomiseSeed()
    {
        seed = Random.Range(0, 999999);
        GenerateTerrain();
    }
}
