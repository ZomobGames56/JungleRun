using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupsBridge_BridgeLevel : MonoBehaviour
{
    // Inputs
    [SerializeField] float radius;
    [SerializeField] float powerUpCooldown;
    [SerializeField] float glideHeight;
    [SerializeField] Transform coinDestinationPos;
    [SerializeField] LayerMask collectibleLayer;
    [SerializeField] GameObject gliderObj;

    // Components
    ForwardMovement_BridgeLevel forwardMovement_BridgeLevel;
    PlayerMovement_FloatingBridge playerMovement_FloatingBridge;
    PlayerMovement_Ice playerMovement_Ice;
    CoinSpawn_FloatingBridge coinSpawn_FloatingBridge;
    CoinSpawn_Ice coinSpawn_Ice;
    Animator animator;
    Rigidbody rb;
    Animator gliderAnim;

    // Variables
    public bool startMagnet = false;
    bool onceCalled = false;
    public float timer = 0f;

    void Start(){
        animator = transform.GetChild(0).GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody>();
        gliderAnim = gliderObj.transform.GetComponent<Animator>();
        forwardMovement_BridgeLevel = transform.GetComponent<ForwardMovement_BridgeLevel>();
        if (GameManager_BridgeLevel.Instance.levelType == "Ice")
        {
            coinSpawn_Ice = transform.GetComponent<CoinSpawn_Ice>();
            playerMovement_Ice = transform.GetComponent<PlayerMovement_Ice>();
        }
        else
        {
            coinSpawn_FloatingBridge = transform.GetComponent<CoinSpawn_FloatingBridge>();
            playerMovement_FloatingBridge = transform.GetComponent<PlayerMovement_FloatingBridge>();
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(coinDestinationPos.position, radius);
        Vector3 rayStart = transform.position + Vector3.forward * 2.0f;
        Gizmos.DrawRay(rayStart, Vector3.down * 5f);
    }

    void Update(){
        if(startMagnet){
            timer += Time.deltaTime;
            if(timer < powerUpCooldown){
                StartMagnet();
                if (GameManager_BridgeLevel.Instance.levelType == "Ice")
                {
                    coinSpawn_Ice.minCooldown = 3f;
                    coinSpawn_Ice.maxCooldown = 3f;
                }
                else
                {
                    coinSpawn_FloatingBridge.minCooldown = 3f;
                    coinSpawn_FloatingBridge.maxCooldown = 3f;
                }
            }
            else{
                startMagnet = false;
                timer = 0f;
                if (GameManager_BridgeLevel.Instance.levelType == "Ice")
                {
                    coinSpawn_Ice.minCooldown = 10f;
                    coinSpawn_Ice.maxCooldown = 20f;
                }
                else
                {
                    coinSpawn_FloatingBridge.minCooldown = 10f;
                    coinSpawn_FloatingBridge.maxCooldown = 20f;
                }
            }
        }
        if(GameManager_BridgeLevel.Instance.gliderCollected && !onceCalled){
            onceCalled = true;
            StartCoroutine(StartGlider());
        }
    }

    void StartMagnet(){
        Collider[] hitColliders = Physics.OverlapSphere(coinDestinationPos.position, radius, collectibleLayer);
        foreach(Collider colliders in hitColliders){
            colliders.gameObject.GetComponent<Coin_BridgeLevel>().Movement(coinDestinationPos);
        }
    }

    void Glider(){
        StartCoroutine(StartGlider());
    }

    IEnumerator StartGlider(){
        GameManager_BridgeLevel.Instance.startGlide = true;
        gliderObj.SetActive(true);
        Vector3 glidePos = new Vector3(transform.position.x, transform.position.y + glideHeight, transform.position.z);
        forwardMovement_BridgeLevel.enabled = false;
        if(GameManager_BridgeLevel.Instance.levelType == "Ice") playerMovement_Ice.enabled = false;
        else playerMovement_FloatingBridge.enabled = false;
        gliderAnim.Play("Spawn");
        animator.Play("Glide - Up",0,0f);
        rb.useGravity = false;
        
        float duration = 0.7f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        while(elapsed < duration){
            transform.position = Vector3.Lerp(startPos, glidePos, elapsed / duration);
            Debug.Log(transform.position);
            elapsed += Time.unscaledDeltaTime;
            Debug.Log("UPPPP");
            yield return null;
        }
        transform.position = glidePos;

        Debug.Log("FORWARD");
        animator.Play("Glide",0,0f);
        if(GameManager_BridgeLevel.Instance.levelType == "Ice") playerMovement_Ice.enabled = true;
        else playerMovement_FloatingBridge.enabled = true;
        forwardMovement_BridgeLevel.enabled = true;

        float glideElapsed = 0f;
        bool timeDone = false;
        bool groundDetected = false;
        while(!(timeDone && groundDetected)){
            rb.useGravity = false;
            glideElapsed += Time.deltaTime;
            timeDone = glideElapsed >= powerUpCooldown;
            Vector3 rayStart = transform.position + Vector3.forward * 2.0f;
            groundDetected = Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 3f, LayerMask.GetMask("Ground"));
            Debug.Log("Ground : " + groundDetected);
            if(groundDetected) Debug.Log("Hit: " + hit.collider.gameObject.name);
            yield return null;
        }

        Debug.Log("DOOWWWNNN");
        rb.useGravity = true;
        gliderAnim.Play("Down");
        animator.Play("Glide - Down");
        GameManager_BridgeLevel.Instance.startGlide = false;
        GameManager_BridgeLevel.Instance.gliderCollected = false;;
        onceCalled = false;

        yield return new WaitForSeconds(0.7f);
        gliderObj.SetActive(false);
        animator.Play("Run");
    }
}
