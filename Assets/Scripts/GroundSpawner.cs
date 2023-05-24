using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject Ground1, Ground2, Ground3;

    //varijabla koja govori dali GroundSpawner objekt dira ground
    bool hasGround = true;
    bool endRequired = false;

    [SerializeField]
    float speed = 25f; // The speed of the movement
    [SerializeField]
    float duration = 40f;

    float minY = -1.5f;
    float maxY = 1.2f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveRightForDuration());
    }

    //korutina koja pomocu varijabli speed i duration pomiče objekt GroundSpawner i poziva funckiju SpawnGround
    private IEnumerator MoveRightForDuration()
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0, Space.Self);
            timeElapsed += Time.deltaTime;
            if (!hasGround && !endRequired)
            {
                SpawnGround();
                hasGround = true;
            }
            yield return null;
        }
    }

    //funkcija koja random poziva neki od 3 definirana objekta za ground i postavlja ih na random generirana mjesta u sceni
    public void SpawnGround()
    {
        //random odabir koji ground ce se stvorit
        int randomNum = Random.Range(1, 4);
        //random odabir udaljenosti izmedu dva grounda
        int x = Random.Range(4, 6);
        //random odabir udaljensoti visina dva grounda
        float y = Random.Range(minY, maxY);

        if (randomNum == 1)
        {
            Instantiate(Ground1, new Vector3(transform.position.x + x, y, 0), Quaternion.identity);
        }
        if (randomNum == 2)
        {
            Instantiate(Ground2, new Vector3(transform.position.x + x, y, 0), Quaternion.identity);
        }
        if (randomNum == 3)
        {
            Instantiate(Ground3, new Vector3(transform.position.x + x, y, 0), Quaternion.identity);
        }
    }

    //funkcija koja provjerava dali se objekt sudario sa objektnom
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            hasGround = true;
        }
        if (collision.gameObject.CompareTag("PlatformDetecter"))
        {
            endRequired = true;
            Destroy(gameObject);
        }
    }
    //funkcija koja provjerava dali se objekt maknuo sa objekta na kojemu je bio
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            hasGround = false;
        }
    }
}
