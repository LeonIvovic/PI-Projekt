using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    [Header("Generator limits")]
    // These 2 points define the rectange in which the level will be generated
    [SerializeField] private Transform bottomLeft;
    [SerializeField] private Transform topRight;
    private Transform player;
    [Space(10)]

    [Header("Generator options")]
    public float padding; // Padding between every element
    public float minHeightVariation; // Min height difference between 2 level elements
    public float maxHeightVariation; // Max height difference between 2 level elements
    private float maxLevelLength = 0f; // Calculated from bottomLeft and topRight positions

    [Header("Infinite generation")]
    [Tooltip("If enabled player position should be used as topRight")]
    public bool infiniteGeneration; // Not yet implemented, infinite level generations without top right limit
    [SerializeField] private float generationDistance;

    [Space(10)]
    public List<GeneratorElement> levelElements;

    private List<GeneratorElement> filteredElements;
    private GeneratorElement previousElement = null;
    private float currentLevelLength = 0;
    private float previousElementHeight;
    [SerializeField] private GameObject endPlatform;

    // Start is called before the first frame update
    void Start()
    {
        // Filter out elements which can only be generated from nextElements (generateOnlyFromNextElements = false)
        filteredElements = levelElements.Where(e => e.generateOnlyFromNextElements == false).ToList();

        if (bottomLeft == null || topRight == null)
        {
            Debug.LogError("Level generator limit points not set!");
        }
        else if (!infiniteGeneration)
        {
            // Start generation in the middle of corner limits
            previousElementHeight = (topRight.position.y + bottomLeft.position.y) / 2;
            maxLevelLength = Mathf.Abs(topRight.position.x - bottomLeft.position.x);
            GenerateLevel();
        }
        else
        {
            // Start generation in the middle of corner limits
            previousElementHeight = (topRight.position.y + bottomLeft.position.y) / 2;
            maxLevelLength = Mathf.Infinity;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    private void Update()
    {
        if (infiniteGeneration && Mathf.Abs(player.position.x - (bottomLeft.position.x + currentLevelLength)) < generationDistance)
        {
            GenerateNextElement();
        }
    }

    GeneratorElement GetNextRandomElement(GeneratorElement previousElement)
    {
        int randomWeight;
        int totalWeight = 0;

        if (previousElement != null && previousElement.nextElements.Length > 0)
        {
            for (int i = 0; i < previousElement.nextElements.Length; i++)
            {
                if (previousElement.nextElements.Contains(previousElement.nextElements[i]))
                {
                    totalWeight += levelElements[previousElement.nextElements[i]].weight;
                }
            }

            randomWeight = Random.Range(0, totalWeight);

            int weight = 0;
            for (int i = 0; i < previousElement.nextElements.Length; i++)
            {
                weight += levelElements[previousElement.nextElements[i]].weight;
                if (randomWeight < weight + i)
                {
                    return levelElements[previousElement.nextElements[i]];
                }
            }

            Debug.LogError("Error while getting weighted random next element!");
            return null;
        }
        else
        {
            // FIlteredElements is a filtered list containing only elements which can always be generated
            // Elements which can only be generated from nextElements (generateOnlyFromNextElements = false) are excluded

            foreach (var e in filteredElements)
            {
                totalWeight += e.weight;
            }

            randomWeight = Random.Range(0, totalWeight);

            int weight = 0;
            for (int i = 0; i < filteredElements.Count; i++)
            {
                weight += filteredElements[i].weight;
                if (randomWeight < weight)
                {
                    return filteredElements[i];
                }
            }

            Debug.LogError("Error while getting weighted random filtered element!");
            return null;
        }
    }

    void GenerateLevel()
    {
        // While loop should never get stuck in infinite loop, but just in case it does this breaks it after 500 tries
        int whileCounter = 0;

        while (currentLevelLength < maxLevelLength && filteredElements.Count > 0 && whileCounter ++ < 500)
        {
            GenerateNextElement();
        }

        // Last platform generation
        Instantiate
        (
            endPlatform,
            new Vector2(bottomLeft.position.x + maxLevelLength + padding + endPlatform.transform.localScale.x / 2, previousElementHeight - 5),
            Quaternion.identity,
            this.transform
        );
    }

    void GenerateNextElement()
    {
        GeneratorElement randomElement = GetNextRandomElement(previousElement);
        previousElement = randomElement;

        // Check if we reached generated count limit for this object and if it's length is smaller than reamining map length
        if (randomElement.CanBeGenerated(maxLevelLength - currentLevelLength - padding))
        {
            // Gets a random float in range minHeighVariation - maxHeighVariation
            // Multiplise the result with 1 or - 1 randomly so that the platform can either be placed above or below previous one
            float randomHeight = Random.Range(minHeightVariation, maxHeightVariation) * (Random.Range(0, 2) * 2 - 1);
            // Clamps the height to bottomLeft and topRight limits
            float height = Mathf.Clamp(previousElementHeight + randomHeight, bottomLeft.position.y, topRight.position.y);
            previousElementHeight = height;

            randomElement.Generate
            (
                new Vector2(bottomLeft.position.x + currentLevelLength + padding + randomElement.levelElement.GetLength() / 2, height),
                Quaternion.identity,
                this.transform
            );

            currentLevelLength += randomElement.levelElement.GetLength() + padding;

            // Remove the object from the object list if it can't be generated in the future (limit reached or too big)
            // To avoid getting this element again in Random.Range
            if (!randomElement.CanBeGenerated(maxLevelLength - currentLevelLength - padding)) filteredElements.Remove(randomElement);
        }
    }
}

[Serializable]
public class GeneratorElement{
    public LevelElement levelElement;
    public int maxTimesGenerated = 1000;
    private int timesGenerated = 0;
    public int weight = 1;
    [Space(5)]
    public bool generateOnlyFromNextElements = false;
    public int[] nextElements;

    public bool CanBeGenerated(float freeSpace) 
    {
        return timesGenerated < maxTimesGenerated && levelElement.GetLength() <= freeSpace;
    }

    public void Generate(Vector2 position, Quaternion rotation, Transform parent)
    {
        levelElement.Generate(position, rotation, parent);
        timesGenerated++;
    }
}
