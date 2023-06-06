using UnityEngine;

public class LevelElement : MonoBehaviour
{
    public GameObject element;
    private float length = 0;

    public GameObject objectOnTop;
    public int minObjectsOnTop = 0;
    public int maxObjectsOnTop = 0;

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

        if (objectOnTop != null && maxObjectsOnTop > 0)
        {
            int randomElementsCount = Random.Range(minObjectsOnTop, maxObjectsOnTop + 1);
            float offset = length / randomElementsCount;
            for (int i = 0; i < randomElementsCount; i++)
            {
                Instantiate
                (
                    objectOnTop,
                    new Vector2(position.x - length / 2 + offset / 2 + i * offset, position.y - 0.22f + transform.localScale.y / 2 + objectOnTop.transform.localScale.y / 2),
                    Quaternion.identity,
                    parent
                 );
            }
        }
    }
}
