using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EntrySceneMain : MonoBehaviour
{
    public GameObject StartMenu;
    public GameObject BGM;
    public VideoPlayer Intro;
    private string _Scene = "LevelSelectionScene";

    private void Awake()
    {
        StartMenu.SetActive(false);
        BGM.SetActive(false);
        Intro.loopPointReached += EndReached;
        Intro.playbackSpeed = GameManager.IntroPlayBackSpeed;
        this.enabled = false;
    }

    void EndReached(VideoPlayer vp)
    {
        StartMenu.SetActive(true);
        BGM.SetActive(true);
        this.enabled = true;
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(LoadAsyncScene());
            this.enabled = false;
        }
    }

    IEnumerator LoadAsyncScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        AsyncOperation levelSelectionScene = SceneManager.LoadSceneAsync(
            _Scene,
            LoadSceneMode.Additive
        );

        while (!levelSelectionScene.isDone)
        {
            yield return null;
        }

        SceneManager.MoveGameObjectToScene(BGM, SceneManager.GetSceneByName(_Scene));

        SceneManager.UnloadSceneAsync(currentScene);
    }
}
