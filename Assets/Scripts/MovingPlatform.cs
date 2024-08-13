using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //public Vector2[] MoveToPos;
    public float MovingSpeed;
    public List<Vector2> MoveToPos;
    private float[] _pathWeight;


    private int _pointsIndex;
    private Rigidbody2D rb;

    private void Start()
    {
        // if dont have any point do nothing
        if (MoveToPos.Count == 0)
        {
            enabled = false;
            return;
        }
        transform.localPosition = MoveToPos[0];
        rb = GetComponent<Rigidbody2D>();
        _pathWeight = new float[MoveToPos.Count];
        for (int i = 0; i < MoveToPos.Count; i++)
        {
            _pathWeight[i] = Vector2.Distance(MoveToPos[i], MoveToPos[(i + 1) % MoveToPos.Count]);

        }

    }

    Vector2 move;
    // move all points
    private void Update()
    {
        if (Vector2.Distance(transform.localPosition, MoveToPos[_pointsIndex]) < 0.1f)
        {
            _pointsIndex = ++_pointsIndex % MoveToPos.Count;
            Vector2 vel = MoveToPos[_pointsIndex] - (Vector2)transform.localPosition;
            move = vel / _pathWeight[(_pointsIndex + MoveToPos.Count - 1) % MoveToPos.Count];
        }

        rb.linearVelocity = move * MovingSpeed;
    }
    // for editor add new points
    public void SetPoints()
    {
        MoveToPos.Add(transform.localPosition);
    }

}
