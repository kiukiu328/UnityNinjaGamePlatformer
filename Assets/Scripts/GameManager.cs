using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    [SerializeField]
    private float _loadingSceneWaitSecond;

    [SerializeField]
    private float _introPlayBackSpeed;

    [SerializeField]
    private float _playerRunSpeed;

    [SerializeField]
    private float _playerJumpForce;

    [SerializeField]
    private int _howManyTimesPlayerCanJump;

    [SerializeField]
    private int _fireBallSpeed;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        Init();
        DontDestroyOnLoad(gameObject);
    }

    void Init()
    {
        LoadingSceneWaitSecond = _loadingSceneWaitSecond;
        IntroPlayBackSpeed = _introPlayBackSpeed;
        PlayerJumpForce = _playerJumpForce;
        PlayerJumpTimes = _howManyTimesPlayerCanJump;
        PlayerRunSpeed = _playerRunSpeed;
        FireBallSpeed = _fireBallSpeed;
    }

    [MenuItem("MyMenu/Clear Data")]
    static void ClearData()
    {
        SaveSystem.ClearData();
    }
}
