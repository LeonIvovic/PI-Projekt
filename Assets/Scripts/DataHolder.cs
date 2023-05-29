using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DataHolder", order = 1)]
public class DataHolder : ScriptableObject
{
    // Saved health (for checkpoints and level loading)
    public int savedHp = 10;
    public int savedMaxHp = 10;

    // Score
    // Lower score better
    public float currentLevelScore = 0;
    public int[] scores;
    public int scorePerDeath = 20;
    public int scorePerSecond = 1;
}