using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    MovingPlatform mp;
    private void OnEnable()
    {
        mp = (target as MovingPlatform);
    }
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        if(GUILayout.Button("Add moving position"))
        {
            mp.SetPoints();

        }
    }
    private void OnSceneGUI()
    {
        int length = mp.MoveToPos.Count;
        if (length < 2)
            return;
        Vector2 pos = mp.transform.parent.position;
        mp.MoveToPos[0] = Handles.PositionHandle(pos + mp.MoveToPos[0], Quaternion.identity) - (new Vector3(pos.x, pos.y, 0));
        for (int i = 1; i < length; i++)
        {
            Handles.DrawLine(pos + mp.MoveToPos[i], pos + mp.MoveToPos[i-1]);
            mp.MoveToPos[i] = Handles.PositionHandle(pos + mp.MoveToPos[i], Quaternion.identity)-(new Vector3(pos.x,pos.y,0));
        }
        Handles.DrawLine(pos + mp.MoveToPos[0], pos + mp.MoveToPos[length-1]);
        if (GUI.changed)
        {
            Debug.Log("GUI.changed", mp.gameObject);
            EditorUtility.SetDirty(mp);
        }
    }
    //private static Vector3[] V2ArrToV3Arr(Vector2[] v2) {
    //    return System.Array.ConvertAll<Vector2, Vector3>(v2, (v2)=> { return new Vector3(v2.x, v2.y, 0); });
    //}
}
