using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelElement : MonoBehaviour
{
    public GameObject element;
    private float length = 0;

    public float GetLength()
    {
        if (length != 0)
        {
            return length;
        }
      
        CalculateLength();
        return length;
    }

    private void CalculateLength()
    {
        element = this.gameObject;

        SpriteRenderer spriteRenderer = element.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            length = spriteRenderer.bounds.size.x;
        }
        else
        {
            length = element.transform.localScale.x;
        }
    }

    public void Generate(Vector2 position, Quaternion rotation, Transform parent)
    {
        Instantiate(element, position, rotation, parent);
    }
}
