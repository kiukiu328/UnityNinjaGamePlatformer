using UnityEngine;
using System;


// for other script get setting from the resources/GameSetting.json file
[DefaultExecutionOrder(-5)]
public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    static public GameSetting gameSetting = null;

    [Serializable]
    public class GameSetting
    {
        public  float LoadingSceneWaitSecond;
        public  float IntroPlayBackSpeed;
        public  float PlayerJumpForce;
        public  float PlayerRunSpeed;
        public  int PlayerJumpTimes;
        public  float FireBallSpeed;
        public  float JumpingCoolDown;
        public  float AttackCoolDown;
        public  float FireBallCoolDown;
        public  int PlayerHP;
        public  int FireBallDamage;
        public  int AttackDamage;
    }

    public Texture2D cursor;
    // for every scenes to use this script
    void Awake()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        LoadGameSetting();
        DontDestroyOnLoad(gameObject);

    }
    void LoadGameSetting()
    { 
        string rawJson = Resources.Load<TextAsset>("GameSetting").text;
        gameSetting = JsonUtility.FromJson<GameSetting>(rawJson);

    }




}
