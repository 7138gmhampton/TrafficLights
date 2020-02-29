using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public Sprite greenLight;
    public Sprite redLight;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void setLightMode(bool green)
    {
        if (green) {
            spriteRenderer.sprite = greenLight;
            boxCollider2D.enabled = false;
        }
        else {
            spriteRenderer.sprite = redLight;
            boxCollider2D.enabled = true;
        }
    }
}
