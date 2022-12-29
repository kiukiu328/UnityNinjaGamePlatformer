using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector2[] MoveToPos;
    private float[] _pathWeight;

    public float MovingSpeed;
    private int _pointsIndex;
    private Vector2 _vel;
    private Vector2 _move;
    private float _totalDistance;

    private void Start()
    {
        transform.position = MoveToPos[0];
        _pathWeight = new float[MoveToPos.Length];
        for (int i = 0; i < MoveToPos.Length; i++)
        {
            _pathWeight[i] = Vector2.Distance(MoveToPos[i], MoveToPos[(i + 1) % MoveToPos.Length]);
            _totalDistance += _pathWeight[i];
        }
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, MoveToPos[_pointsIndex]) < 0.1f)
        {
            _pointsIndex = ++_pointsIndex % MoveToPos.Length;
            _vel = MoveToPos[_pointsIndex] - (Vector2)transform.position;
            _move = _vel / _pathWeight[(_pointsIndex + MoveToPos.Length - 1) % MoveToPos.Length];
        }

        GetComponent<Rigidbody2D>().velocity = _move * MovingSpeed;

    }
}
