using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelection
    : MonoBehaviour,
        IPointerEnterHandler,
        IPointerClickHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler
{
    public int Level;
    public bool IsLevelActive;

    public Sprite UnlockedImg;
    public Sprite LockedImg;
    public Color PressedColor;
    public Color UnlockedColor;
    public Image[] Images;

    private const int LockIndex = 0;
    private Animator _animator;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>();
        IsLevelActive = SaveSystem.Load().GetLevelActive(Level);
        LevelLock();
    }

    private void LevelLock()
    {
        if (IsLevelActive)
        {
            Images[LockIndex].sprite = UnlockedImg;
            this.enabled = true;
            foreach (Image image in Images)
            {
                image.color = UnlockedColor;
            }
        }
        else
        {
            Images[LockIndex].sprite = LockedImg;
            this.enabled = false;
            foreach (Image image in Images)
            {
                image.color = Color.white;
            }
        }
    }

    IEnumerator LoadAsyncScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] objects = currentScene.GetRootGameObjects();
        foreach (var item in objects)
        {
            if (item.name == "Canvas")
                item.GetComponent<Canvas>().enabled = false;
            else
                item.SetActive(false);
        }
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(
            "loadingScene",
            LoadSceneMode.Additive
        );

        while (!loadingScene.isDone)
        {
            yield return null;
        }
        AsyncOperation levelScene = SceneManager.LoadSceneAsync(
            "Level_" + Level,
            LoadSceneMode.Additive
        );
        levelScene.allowSceneActivation = false;
        //----only for testing
        yield return new WaitForSeconds(GameManager.LoadingSceneWaitSecond);
        //--------------------------------
        while (levelScene.progress < 0.9f)
        {
            yield return null;
        }
        levelScene.allowSceneActivation = true;
        SceneManager.LoadScene("Level_" + Level);
        // Debug.Log(SceneManager.GetSceneByName("LevelSelectionScene"));
        // SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("LoadingScene"));
        // Debug.Log("Unloaded LoadingScene");
        // SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("LevelSelectionScene"));
        // Debug.Log("Unloaded LevelSelectionScene");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(LoadAsyncScene());
        this.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (Image image in Images)
        {
            image.color = PressedColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (Image image in Images)
        {
            image.color = UnlockedColor;
        }
    }
}
