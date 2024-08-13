using UnityEngine;
using UnityEditor;

public class EditorMenu : MonoBehaviour
{
    [MenuItem("MyMenu/Clear Data")]
    static void ClearData()
    {
        SaveSystem.ClearData();
    }
}
