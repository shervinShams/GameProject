using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableSprite : MonoBehaviour
{

    public SpriteRenderer spriteRender;
    public float hidingAnimationSpeed = 2.0f;

    private Color hidingAlpha;

    // Use this for initialization
    protected virtual void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        hidingAlpha = spriteRender.color;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!hidingAlpha.Equals(spriteRender.color))
        {
            spriteRender.color = Color.Lerp(spriteRender.color, hidingAlpha, Time.deltaTime * 2.0f);
        }
    }

    public void Hide()
    {
        hidingAlpha = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    public void Show()
    {
        hidingAlpha = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
