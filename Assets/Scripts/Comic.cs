using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Comic : MonoBehaviour
{
    public string nextScene;
    public Button prevButton;
    public Button nextButton;
    public CinemachineVirtualCamera virtualCamera;
    public float ortographicSize = 1.5f;
    public float speed = 1;
    public int frame = 0;
    public int frameCount = 6;
    public Animation animation2;
    public Animation animation4;

    private void OnEnable()
    {
        nextButton.onClick.Add(NextFrame);
        prevButton.onClick.Add(PrevFrame);
        prevButton.gameObject.SetActive(false);
    }

    private IEnumerator Start()
    {
        yield return null;
        virtualCamera.Follow = transform.GetChild(frame);
    }

    private void NextFrame()
    {
        if (frame >= frameCount - 1)
        {
            LoadNextScene();
            return;
        }
        prevButton.gameObject.SetActive(true);
        frame++;
        virtualCamera.Follow = transform.GetChild(frame);
        if (frame == 1) animation2.Play(PlayMode.StopAll);
        if (frame == 3) animation4.Play(PlayMode.StopAll);
        if (frame >= frameCount - 1) nextButton.image.color = Color.green;
    }

    private void PrevFrame()
    {
        if (frame <= 0) return;
        nextButton.image.color = Color.white;
        frame--;
        virtualCamera.Follow = transform.GetChild(frame);
        if (frame <= 0) prevButton.gameObject.SetActive(false); ;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    private void Update()
    {
        ZoomToFrame();
    }

    private void ZoomToFrame()
    {
        virtualCamera.m_Lens.OrthographicSize =
                    Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, ortographicSize, speed * Time.deltaTime);
    }
}
