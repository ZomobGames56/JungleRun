using UnityEngine;

public class DestroyObjectAfter : MonoBehaviour
{
    [SerializeField] float timer;

    void Start()
    {
        Invoke("DestroyObj", timer);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }
}
