using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteDuplicator : MonoBehaviour
{

    private Image image;

    // Use this for initialization
    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetSprite(Sprite sprite)
    {
        if (image)
            image.sprite = sprite;
    }
}
