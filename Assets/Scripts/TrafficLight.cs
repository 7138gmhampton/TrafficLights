using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void goGreen(bool green)
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
