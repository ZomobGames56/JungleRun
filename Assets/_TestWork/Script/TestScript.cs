using UnityEngine;

public class TestScript : MonoBehaviour
{
    private CharacterController cc;
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }
    private void Update()
    {
        cc.Move(transform.forward * 5 * Time.deltaTime);
    }
    void OnTriggerStay(Collider colldier)
    {
        print("Triggering");
}
}
