﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode {NoiseMap, ColorMap, Mesh};
    public DrawMode drawMode;
    public int mapWidht;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public float meshHeightMultiplier;

    public AnimationCurve meshHeightCurve;

    public int seed;
    public Vector2 offset;
    public bool autoUpdate;
    public TerrainType[] regions;


    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidht, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        Color[] colorMap = new Color[mapWidht*mapHeight];
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidht; x++) {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight <= regions[i].height) {
                        colorMap[y*mapWidht + x] = regions[i].color;
                        break;
                    } 
                }
                    
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap) display.DrawTexture (TextureGenerator.TextureFromHeightMap(noiseMap));
        else if(drawMode == DrawMode.ColorMap) display.DrawTexture (TextureGenerator.TextureFromColorMap(colorMap, mapWidht, mapHeight));
        else if(drawMode == DrawMode.Mesh) display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColorMap(colorMap, mapWidht, mapHeight));
    }

    void onValidate() {
        if (mapWidht < 1) mapWidht = 1;
        if (mapHeight < 1) mapHeight = 1;
        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 0) octaves = 0;

    }
}
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;

}