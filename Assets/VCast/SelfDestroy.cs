using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject);
    }
}
