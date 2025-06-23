using UnityEngine;

public class ForwardMovement_BridgeLevel : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fixedGlideHeight;
    private Rigidbody rb;

    private void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate(){
        if(GameManager_BridgeLevel.Instance.startGame){
            if(GameManager_BridgeLevel.Instance.startGlide){
                float movePower = moveSpeed * GameManager_BridgeLevel.Instance.speedModifier * Time.deltaTime;
                Vector3 pos = rb.position;
                pos.y = fixedGlideHeight;
                rb.MovePosition(new Vector3(rb.position.x, fixedGlideHeight, rb.position.z + movePower));
            }
            else{
                //Vector3 forward = transform.forward;
               // Vector3 moveVector = new Vector3(forward.x, 0f, forward.z) * moveSpeed * GameManager_BridgeLevel.Instance.speedModifier * Time.deltaTime;
                rb.MovePosition(rb.position + Vector3.forward*moveSpeed*GameManager_BridgeLevel.Instance.speedModifier * Time.fixedDeltaTime);
            }
        }
    }
}
