using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

[DefaultExecutionOrder(-1)]
public class UIController : MonoBehaviour
{
    public static UIController instance;
    public GameObject UI;
    public Sprite[] SoundImgs;
    public Sprite[] SettingImgs;
    public int NextLevel;
    public GameObject Life;
    public GameObject LifeBackround;

    private GameObject UI_Instance;
    private GameObject GameUIObj;
    private Button SettingBtn;

    private GameObject GameOverObj;
    private Button PlayAgainBtn,
        BackToMenuBtn;

    private GameObject PauseMenuObj;
    private Button SoundBtn,
        ResumeBtn,
        RestartBtn,
        MenuBtn,
        QuitBtn;

    private bool _sound = true;
    private AudioSource _bgm;
    private GameObject[] _life;
    private GameObject[] _scrolls;

    private void Start()
    {
        UI_Instance = Instantiate(UI);
        instance = this;
        _bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
        //=============== Get UI========================
        GameUIObj = UI_Instance.transform.Find("GameUI").gameObject;
        GameOverObj = UI_Instance.transform.Find("GameOver").gameObject;
        PauseMenuObj = UI_Instance.transform.Find("PauseMenu").gameObject;
        //=============== Get GameUIObj Button========================
        SettingBtn = GameUIObj.transform.Find("SettingBtn").GetComponent<Button>();

        //=============== Get GameOverObj Button========================
        PlayAgainBtn = GameOverObj.transform.Find("Buttons/PlayAgain").GetComponent<Button>();
        BackToMenuBtn = GameOverObj.transform.Find("Buttons/BackToMenu").GetComponent<Button>();
        //=============== Get PauseMenuObj Button========================
        SoundBtn = PauseMenuObj.transform.Find("Selections/Sound").GetComponent<Button>();
        ResumeBtn = PauseMenuObj.transform.Find("Selections/Resume").GetComponent<Button>();
        RestartBtn = PauseMenuObj.transform.Find("Selections/Restart").GetComponent<Button>();
        MenuBtn = PauseMenuObj.transform.Find("Selections/Menu").GetComponent<Button>();
        QuitBtn = PauseMenuObj.transform.Find("Selections/Quit").GetComponent<Button>();
        //=============== Set Listener ========================
        SettingBtn.onClick.AddListener(PauseMenu);
        PlayAgainBtn.onClick.AddListener(PlayAgain);
        BackToMenuBtn.onClick.AddListener(BackToMenu);
        SoundBtn.onClick.AddListener(SoundToggle);
        ResumeBtn.onClick.AddListener(Resume);
        RestartBtn.onClick.AddListener(PlayAgain);
        MenuBtn.onClick.AddListener(BackToMenu);
        QuitBtn.onClick.AddListener(Quit);
        //=============== Set Life and Scrolls ========================
        
        _life = new GameObject[GameManager.PlayerHP];
        Transform life = GameUIObj.transform.Find("LifeBar/Life");
        Transform lifeBackground = GameUIObj.transform.Find("LifeBar/LifeBackground");
        for (int j = 0; j < GameManager.PlayerHP; j++)
        {
            _life[j] = Instantiate(Life, life);
            Instantiate(LifeBackround, lifeBackground);
        }
        //foreach (Transform child in life)
        //{
        //    _life[i++] = child.gameObject;
        //}
        int i = 0;
        _scrolls = new GameObject[4];
        Transform scrolls = GameUIObj.transform.Find("LifeBar/Scrolls");
        foreach (Transform child in scrolls)
        {
            _scrolls[i++] = child.gameObject;
        }
    }

    public void Injure()
    {
        for (int i = _life.Length - 1; i >= 0; i--)
        {
            if (_life[i].GetComponent<Image>().enabled != false)
            {
                _life[i].GetComponent<Image>().enabled = false;
                break;
            }
        }
    }

    public void GetScroll()
    {
        for (int i = 0; i < _scrolls.Length; i++)
        {
            if (_scrolls[i].GetComponent<Image>().enabled != true)
            {
                _scrolls[i].GetComponent<Image>().enabled = true;
                break;
            }
        }
    }

    public void Won()
    {
        Time.timeScale = 0;
        SaveData save = SaveSystem.Load();
        save.SetLevelActive(NextLevel);
        SaveSystem.Save(save);
        SceneManager.LoadScene("LevelSelectionScene");
    }

    public void PauseMenu()
    {
        Time.timeScale = 0;
        SettingBtn.GetComponent<Image>().sprite = SettingImgs[0];
        PauseMenuObj.SetActive(true);
    }

    public void GameOver()
    {
        _bgm.Pause();
        GameOverObj.SetActive(true);
        Time.timeScale = 0;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelectionScene");
    }

    public void Resume()
    {
        PauseMenuObj.SetActive(false);
        SettingBtn.GetComponent<Image>().sprite = SettingImgs[1];
        Time.timeScale = 1;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SoundToggle()
    {
        _sound = !_sound;
        if (_sound)
        {
            SoundBtn.GetComponent<Image>().sprite = SoundImgs[3];
            _bgm.UnPause();
        }
        else
        {
            SoundBtn.GetComponent<Image>().sprite = SoundImgs[0];
            _bgm.Pause();
        }
    }

    public void Quit()
    {
        Application.Quit();
        EditorApplication.isPlaying = false;
    }
}
