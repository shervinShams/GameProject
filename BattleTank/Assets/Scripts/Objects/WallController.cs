using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{

    public Sprite[] WallStates;

    private int currentWallState = 0;

    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        currentWallState += damage;
        if (currentWallState >= WallStates.Length)
        {
            BlowTheWall();
        }
        else
        {
            spriteRenderer.sprite = WallStates[currentWallState];
        }
    }

    void BlowTheWall()
    {
        Destroy(gameObject);
    }
}
