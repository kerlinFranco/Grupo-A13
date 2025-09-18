using UnityEngine;

public interface IStunnable
{
    void ApplyStun(float duration);
    bool IsStunned { get; }
}

