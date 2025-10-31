using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon References")]
    public PlayerShooting rangedWeapon;
    public MeleeWeapon meleeWeapon;
    
    [Header("Weapon Settings")]
    public WeaponType currentWeapon = WeaponType.Ranged;
    
    public enum WeaponType
    {
        Ranged,
        Melee
    }
    
    private InputAction attackAction;
    private InputAction switchWeaponAction;
    
    void Start()
    {
        if (rangedWeapon == null)
        {
            rangedWeapon = GetComponent<PlayerShooting>();
        }
        
        if (meleeWeapon == null)
        {
            meleeWeapon = GetComponent<MeleeWeapon>();
        }
        
        var playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (playerInput != null)
        {
            attackAction = playerInput.actions["Attack"];
            
            var switchAction = playerInput.actions.FindAction("SwitchWeapon");
            if (switchAction != null)
            {
                switchWeaponAction = switchAction;
            }
            else
            {
                Debug.LogWarning("SwitchWeapon action not found in Input Actions. Weapon switching disabled.");
            }
        }
        
        UpdateWeaponVisuals();
    }
    
    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.isGameActive)
            return;
        
        if (switchWeaponAction != null && switchWeaponAction.WasPressedThisFrame())
        {
            SwitchWeapon();
        }
        
        if (attackAction != null && attackAction.WasPressedThisFrame())
        {
            if (currentWeapon == WeaponType.Melee && meleeWeapon != null)
            {
                meleeWeapon.Attack();
            }
        }
    }
    
    public void SwitchWeapon()
    {
        if (currentWeapon == WeaponType.Ranged)
        {
            currentWeapon = WeaponType.Melee;
            Debug.Log("Switched to Melee weapon");
        }
        else
        {
            currentWeapon = WeaponType.Ranged;
            Debug.Log("Switched to Ranged weapon");
        }
        
        UpdateWeaponVisuals();
    }
    
    void UpdateWeaponVisuals()
    {
        if (rangedWeapon != null)
        {
            rangedWeapon.enabled = (currentWeapon == WeaponType.Ranged);
        }
        
        if (meleeWeapon != null && meleeWeapon.swordVisual != null)
        {
            meleeWeapon.swordVisual.SetActive(currentWeapon == WeaponType.Melee);
        }
        
        if (UIManager.Instance != null)
        {
            string weaponName = currentWeapon == WeaponType.Ranged ? "Gun" : "Sword";
            UIManager.Instance.UpdateWeapon(weaponName);
        }
    }
    
    public bool IsRangedWeaponActive()
    {
        return currentWeapon == WeaponType.Ranged;
    }
    
    public bool IsMeleeWeaponActive()
    {
        return currentWeapon == WeaponType.Melee;
    }
}
