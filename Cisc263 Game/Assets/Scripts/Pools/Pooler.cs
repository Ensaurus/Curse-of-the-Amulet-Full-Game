using UnityEngine;

public interface Pooler
{
    GameObject GetObject();
    void ResetPool();
}
