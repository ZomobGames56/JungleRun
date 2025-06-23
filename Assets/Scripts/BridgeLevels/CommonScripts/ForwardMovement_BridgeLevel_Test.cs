using UnityEngine;

public class ForwardMovement_BridgeLevel_Test : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fixedGlideHeight;
    private Rigidbody rb;
    CharacterController characterController;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate(){
        if(GameManager_BridgeLevel.Instance.startGame){
            if (GameManager_BridgeLevel.Instance.startGlide)
            {
                float movePower = moveSpeed * GameManager_BridgeLevel.Instance.speedModifier * Time.deltaTime;
                Vector3 pos = rb.position;
                pos.y = fixedGlideHeight;
                rb.MovePosition(new Vector3(rb.position.x, fixedGlideHeight, rb.position.z + movePower));
            }
            else
            {
                Vector3 forward = transform.forward;
                // Vector3 moveVector = new Vector3(forward.x, 0f, forward.z) * moveSpeed * GameManager_BridgeLevel.Instance.speedModifier * Time.deltaTime;
                // characterController.Move(rb.position + moveVector);
                characterController.SimpleMove(forward * moveSpeed * Time.deltaTime);
                
            }
        }
    }
}
