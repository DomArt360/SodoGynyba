using UnityEngine;

public interface IMovement
{
    float MoveSpeed { get; }
    void FixedUpdateMovement(Transform target);
}