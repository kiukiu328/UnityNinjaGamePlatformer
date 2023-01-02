using System.Collections;
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
    // for press any to start
    private void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(LoadAsyncScene());
            this.enabled = false;
        }
    }
    // load LevelSelectionScene with current bgm Object
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
