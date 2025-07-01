using UnityEngine;

public class TestCollider : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            print("Player Col");
        }
       
    }
}
