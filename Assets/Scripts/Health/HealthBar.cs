using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private RectTransform totalhealthBar;
    [SerializeField] private RectTransform currenthealthBar;
    // How much health each health image represeents
    // E.g. 1 - 1 image per health, 2 - each image contains 2 health
    [SerializeField] private float healthPerImg = 1;
    private Image img; // Assuming both healthbars use same size of health image

    private void Start()
    {
        img = totalhealthBar.GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        totalhealthBar.sizeDelta = new Vector2
            (
                img.sprite.bounds.size.x * img.sprite.pixelsPerUnit * player.GetMaxHealth() / healthPerImg,
                totalhealthBar.sizeDelta.y
            );

        currenthealthBar.sizeDelta = new Vector2
            (
                img.sprite.bounds.size.x * img.sprite.pixelsPerUnit * player.GetHealth() / healthPerImg,
                totalhealthBar.sizeDelta.y
            );
    }

}
