using UnityEngine;
using UnityEditor;
using Newtonsoft.Json.Linq;


// for other script get setting from the resources/GameSetting.json file
[DefaultExecutionOrder(-5)]
public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public static float LoadingSceneWaitSecond;
    public static float IntroPlayBackSpeed;
    public static float PlayerJumpForce;
    public static float PlayerRunSpeed;
    public static int PlayerJumpTimes;
    public static float FireBallSpeed;
    public static float JumpingCoolDown;
    public static float AttackCoolDown;
    public static float FireBallCoolDown;
    public static int PlayerHP;
    public static int FireBallDamage;
    public static int AttackDamage;

    // for every scenes to use this script
    void Awake()
    {
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

        JObject setting = JObject.Parse(rawJson);
        LoadingSceneWaitSecond = (float)setting.GetValue("LoadingSceneWaitSecond");
        IntroPlayBackSpeed = (float)setting.GetValue("IntroPlayBackSpeed");
        PlayerJumpForce = (float)setting.GetValue("PlayerJumpForce");
        PlayerJumpTimes = (int)setting.GetValue("PlayerJumpTimes");
        PlayerRunSpeed = (float)setting.GetValue("PlayerRunSpeed");
        FireBallSpeed = (float)setting.GetValue("FireBallSpeed");
        FireBallCoolDown = (float)setting.GetValue("FireBallCoolDown");
        AttackCoolDown = (float)setting.GetValue("AttackCoolDown");
        JumpingCoolDown = (float)setting.GetValue("JumpingCoolDown");
        FireBallDamage = (int)setting.GetValue("FireBallDamage");
        AttackDamage = (int)setting.GetValue("AttackDamage");
        PlayerHP = (int)setting.GetValue("PlayerHP");


    }



    [MenuItem("MyMenu/Clear Data")]
    static void ClearData()
    {
        SaveSystem.ClearData();
    }
}
