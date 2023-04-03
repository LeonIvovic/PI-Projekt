using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObsticleDetection : MonoBehaviour
{

  public int respawn;
  private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.tag == "Player")
    {
        Destroy(collision.gameObject);
        SceneManager.LoadScene(respawn);
        Debug.Log("sad");
    }
}
}
