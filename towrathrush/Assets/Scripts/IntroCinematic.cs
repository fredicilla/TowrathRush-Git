using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class IntroCinematic : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public Transform tent;
    public Transform player;
    
    [Header("Camera Positions")]
    public Vector3 startCameraOffset = new Vector3(0, 1.5f, -10);
    public Vector3 endCameraOffset = new Vector3(0, 25, 100);
    
    [Header("Timing")]
    public float cinematicDuration = 12f;
    public float delayBeforePlayerRuns = 1f;
    
    [Header("Player Animation")]
    public Vector3 playerStartPositionOffset = new Vector3(0, 0, -3);
    public Vector3 playerEndPositionOffset = new Vector3(0, 0, 3);
    public float playerRunSpeed = 8f;
    
    [Header("Playback Settings")]
    public bool allowSkip = true;
    
    private bool cinematicPlaying = false;
    private Coroutine currentCinematic;
    
    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        if (tent == null)
            tent = GameObject.Find("TENT")?.transform;
            
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        Debug.Log("Starting intro cinematic - click/tap to skip");
        currentCinematic = StartCoroutine(PlayIntroCinematic());
        cinematicPlaying = true;
    }
    
    void Update()
    {
        if (cinematicPlaying && allowSkip)
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                SkipCinematic();
            }
            else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                SkipCinematic();
            }
        }
    }
    
    void SkipCinematic()
    {
        if (!cinematicPlaying)
            return;
            
        Debug.Log("Cinematic skipped by player");
        cinematicPlaying = false;
        
        if (currentCinematic != null)
        {
            StopCoroutine(currentCinematic);
        }
        
        StartCoroutine(SkipToGameplay());
    }
    
    IEnumerator SkipToGameplay()
    {
        DisablePlayerControl();
        
        Vector3 playerStartPosition = new Vector3(0, 2, 1);
        
        if (player != null)
        {
            player.position = playerStartPosition;
            player.rotation = Quaternion.Euler(0, 0, 0);
            
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        
        if (mainCamera != null)
        {
            Vector3 followPos = playerStartPosition + new Vector3(0, 5, -10);
            mainCamera.transform.position = followPos;
            mainCamera.transform.LookAt(playerStartPosition);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        EnablePlayerControl();
        
        CameraFollow camFollow = mainCamera.GetComponent<CameraFollow>();
        if (camFollow != null)
        {
            camFollow.enabled = true;
        }
    }
    
    IEnumerator PlayIntroCinematic()
    {
        DisablePlayerControl();
        
        Vector3 tentPos = tent != null ? tent.position : Vector3.zero;
        Vector3 playerStartPosition = tentPos + playerStartPositionOffset;
        Vector3 playerEndPosition = tentPos + playerEndPositionOffset;
        
        if (player != null)
        {
            player.position = playerStartPosition;
        }
        
        Vector3 startPos = tentPos + startCameraOffset;
        Vector3 endPos = tentPos + endCameraOffset;
        
        mainCamera.transform.position = startPos;
        mainCamera.transform.LookAt(tentPos + Vector3.up * 2);
        
        Debug.Log("Cinematic: Smooth camera movement starting...");
        
        float elapsed = 0f;
        
        while (elapsed < cinematicDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / cinematicDuration;
            
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, smoothT);
            
            float arcHeight = Mathf.Sin(smoothT * Mathf.PI) * 4f;
            currentPos.y += arcHeight;
            
            mainCamera.transform.position = currentPos;
            
            float horizonDistance = Mathf.Lerp(15f, 200f, smoothT);
            Vector3 lookTarget = tentPos + Vector3.forward * horizonDistance + Vector3.up * 5;
            
            mainCamera.transform.LookAt(lookTarget);
            
            yield return null;
        }
        
        Debug.Log("Cinematic: Camera movement complete");
        yield return new WaitForSeconds(delayBeforePlayerRuns);
        
        Debug.Log("Cinematic: Player running out of tent...");
        
        if (player != null)
        {
            StartCoroutine(MovePlayerOutOfTent(playerStartPosition, playerEndPosition));
            
            yield return new WaitForSeconds(0.5f);
            
            Vector3 followPos = player.position + new Vector3(0, 5, -10);
            Quaternion followRot = Quaternion.LookRotation(player.position - followPos);
            
            elapsed = 0f;
            Vector3 camStartPos = mainCamera.transform.position;
            Quaternion camStartRot = mainCamera.transform.rotation;
            
            Debug.Log("Cinematic: Camera following player...");
            while (elapsed < 1f)
            {
                elapsed += Time.deltaTime * 1.5f;
                mainCamera.transform.position = Vector3.Lerp(camStartPos, followPos, elapsed);
                mainCamera.transform.rotation = Quaternion.Lerp(camStartRot, followRot, elapsed);
                yield return null;
            }
        }
        
        yield return new WaitForSeconds(1.5f);
        
        EndCinematic();
    }
    
    IEnumerator MovePlayerOutOfTent(Vector3 startPosition, Vector3 endPosition)
    {
        if (player == null)
            yield break;
            
        Vector3 startPos = player.position;
        float elapsed = 0f;
        float duration = Vector3.Distance(startPosition, endPosition) / playerRunSpeed;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            player.position = Vector3.Lerp(startPos, endPosition, t);
            yield return null;
        }
        
        player.position = endPosition;
    }
    
    void DisablePlayerControl()
    {
        if (player != null)
        {
            PlayerController controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.canMove = false;
                controller.enabled = false;
            }
                
            PlayerShooting shooting = player.GetComponent<PlayerShooting>();
            if (shooting != null)
                shooting.enabled = false;
                
            WeaponSystem weaponSystem = player.GetComponent<WeaponSystem>();
            if (weaponSystem != null)
                weaponSystem.enabled = false;
                
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isGameActive = false;
        }
        
        CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.enabled = false;
        }
        
        Debug.Log("Player control DISABLED for cinematic");
    }
    
    void EnablePlayerControl()
    {
        if (player != null)
        {
            PlayerController controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.canMove = true;
                controller.enabled = true;
            }
                
            PlayerShooting shooting = player.GetComponent<PlayerShooting>();
            if (shooting != null)
                shooting.enabled = true;
                
            WeaponSystem weaponSystem = player.GetComponent<WeaponSystem>();
            if (weaponSystem != null)
                weaponSystem.enabled = true;
        }
        
        CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.enabled = true;
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isGameActive = true;
        }
        
        Debug.Log("Player control ENABLED - gameplay started!");
    }
    
    void EndCinematic()
    {
        cinematicPlaying = false;
        EnablePlayerControl();
        
        Debug.Log("Intro cinematic finished - gameplay started!");
        
        Destroy(gameObject);
    }
}
