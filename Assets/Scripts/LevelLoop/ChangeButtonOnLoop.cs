using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeButtonOnLoop : MonoBehaviour
{
    void Start()
    {
        foreach (var audioSource in FindObjectsOfType<AudioSource>())
            foreach (var buttonDisplay in FindObjectsOfType<ButtonDisplay>())
                StartCoroutine(LoopAndChangeButton(audioSource, buttonDisplay));
    }

    IEnumerator LoopAndChangeButton(AudioSource audioSource, ButtonDisplay buttonDisplay)
    {
        while (true)
        {
            audioSource.Play();
            buttonDisplay.spriteRenderer.sprite = ChooseRandomSprite(buttonDisplay.sprites);
            yield return new WaitForSeconds(audioSource.clip.length);
        }
    }

    Sprite ChooseRandomSprite(List<Sprite> sprites)
    {
        var randomI = Random.Range(0, sprites.Count);
        return sprites[randomI];
    }
}
