using System.Collections.Generic;
using UnityEngine;

public class ButtonDisplay : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public SpriteRenderer spriteRenderer;

    public Sprite ButtonA => sprites[2];
    public Sprite ButtonB => sprites[3];
    public Sprite ButtonX => sprites[1];
    public Sprite ButtonY => sprites[0];
    public Sprite ButtonL => sprites[4];
    public Sprite ButtonR => sprites[5];

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
