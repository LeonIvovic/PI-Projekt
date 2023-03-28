using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level elements")]
    // These 2 points define the rectange in which the level will be generated
    [SerializeField]
    private Transform bottomLeft;
    [SerializeField]
    private Transform topRight;
    [Space(10)]
    public Vector2 padding; // Padding space between left and right of each generated element

    public float minHeighVariation; // Min height difference between 2 level elements
    public float maxHeighVariation; // Max height difference between 2 level elements

    private float maxLevelLength = 0f;


    [Header("Generation options")]
    public bool generateAllAtStart;
    public bool infiniteGeneration;

    // Used for adding objects that can be generated from the Editor
    public List<GeneratorElements> levelElements;

    // Start is called before the first frame update
    void Start()
    {
        // Bottom left is always required and top right only if infinite generation is disabled
        if (bottomLeft == null || (topRight == null && !infiniteGeneration))
        {
            Debug.LogError("Level generator limit points not set!");
        }

        if (!infiniteGeneration)
        {
            maxLevelLength = Math.Abs(topRight.position.x - bottomLeft.position.x);
        }

        if (generateAllAtStart)
        {
            Generate();
        }
    }

    void Generate()
    {
        float currentLevelLength = 0f;

        int tryCounter = 0;

        float height = 0;
        float previousElementHeight = bottomLeft.position.y;

        while (currentLevelLength < maxLevelLength)
        {
            int randomElementIndex = Random.Range(0, levelElements.Capacity);
            GeneratorElements randomElement = levelElements[randomElementIndex];

            // Check if we reached generated count limit for this object and if it's length is smaller than reamining map length
            if (randomElement.CanBeGenerated(maxLevelLength - currentLevelLength))
            {
                // Gets a random float in range minHeighVariation - maxHeighVariation
                // Multiplise the result with 1 or - 1 randomly so that the platform can either be placed above or below previous one
                float randomHeight = Random.Range(minHeighVariation, maxHeighVariation) * (Random.Range(0, 2) * 2 - 1);
                // Clamps the height to bottomLeft and topRight limits
                height = Math.Clamp(previousElementHeight + randomHeight, bottomLeft.position.y, topRight.position.y);
                previousElementHeight = height;

                randomElement.Generate
                (
                    new Vector2(bottomLeft.position.x + currentLevelLength + randomElement.levelElement.GetLength() / 2, height),
                    Quaternion.identity,
                    this.transform
                );

                currentLevelLength += randomElement.levelElement.GetLength();

                // Remove the object from the object list if it can't be generated in the future (limit reached or too big)
                // To avoid getting this element again in Random.Range
                if (!randomElement.CanBeGenerated(maxLevelLength - currentLevelLength)) levelElements.Remove(randomElement);
            }
            tryCounter++;
            if (tryCounter > 500)
            {
                break;
            }
        }
    }
}

[Serializable]
public class GeneratorElements{
    public LevelElement levelElement;
    public int maxTimesGenerated;
    private int timesGenerated = 0;

    public bool CanBeGenerated(float len) 
    {
        return timesGenerated <= maxTimesGenerated && levelElement.GetLength() <= len;
    }

    public void Generate(Vector2 position, Quaternion rotation, Transform parent)
    {
        levelElement.Generate(position, rotation, parent);
        timesGenerated++;
    }
}
