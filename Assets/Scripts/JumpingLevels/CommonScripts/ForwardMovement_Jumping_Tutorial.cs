using UnityEngine;

public class ForwardMovement_Jumping_Tutorial : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody rb;

    private void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
        if(GameManager_Jumping_Tutorial.Instance.startGame && !GameManager_Jumping_Tutorial.Instance.paused){
            Vector3 forward = transform.forward;
            Vector3 moveVector = new Vector3(forward.x, 0f, forward.z) * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveVector);
        }
    }
}
