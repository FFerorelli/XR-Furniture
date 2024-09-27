using UnityEngine;

public class MenuInstantiator : MonoBehaviour
{
    [Header("Menu Settings")]
    [Tooltip("Assign your menu prefab here.")]
    public GameObject menuPrefab;          // Assign your menu prefab in the Inspector

    [Tooltip("Name of the CenterEyeAnchor GameObject.")]
    public string centerEyeAnchorName = "CenterEyeAnchor"; // Ensure this matches your rig's object name

    [Tooltip("Distance to the right of the parent object where the menu will appear.")]
    public float offsetDistance = 2.0f;    // Distance to the right of the parent object

    [Tooltip("Height offset for the menu relative to the parent object.")]
    public float heightOffset = 1.0f;       // Optional: Height adjustment for the menu

    private Transform playerTransform;

    void Awake()
    {
        // Find the CenterEyeAnchor by name during Awake to ensure it's available when InstantiateMenu is called
        GameObject centerEye = GameObject.Find(centerEyeAnchorName);
        if (centerEye != null)
        {
            playerTransform = centerEye.transform;
        }
        else
        {
            Debug.LogError($"GameObject named '{centerEyeAnchorName}' not found! Ensure it exists in the scene.");
        }
    }

    /// <summary>
    /// Instantiates the menu with the calculated position and rotation.
    /// This method should be called externally when you need to display the menu.
    /// </summary>
    /// <returns>The instantiated menu GameObject, or null if instantiation failed.</returns>
    public GameObject InstantiateMenu()
    {
        // Validate menuPrefab assignment
        if (menuPrefab == null)
        {
            Debug.LogError("Menu Prefab is not assigned in the Inspector!");
            return null;
        }

        // Validate playerTransform assignment
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned! Ensure the CenterEyeAnchor exists in the scene.");
            return null;
        }

        // Calculate position and rotation
        Vector3 menuPosition = CalculateMenuPosition();
        Quaternion menuRotation = CalculateMenuRotation(menuPosition);

        // Instantiate the menu
        GameObject instantiatedMenu = Instantiate(menuPrefab, menuPosition, menuRotation, this.transform);

        return instantiatedMenu;
    }

    /// <summary>
    /// Calculates the desired position for the menu.
    /// </summary>
    /// <returns>The calculated world position for the menu.</returns>
    private Vector3 CalculateMenuPosition()
    {
        // Direction from parent to player
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Right direction relative to the player's view
        Vector3 rightDirection = Vector3.Cross(directionToPlayer, playerTransform.up).normalized;

        // Desired position: to the right of the parent object with optional height offset
        Vector3 desiredPosition = transform.position + rightDirection * offsetDistance + Vector3.up * heightOffset;

        return desiredPosition;
    }

    /// <summary>
    /// Calculates the desired rotation for the menu to face the player.
    /// </summary>
    /// <param name="menuPosition">The current position of the menu.</param>
    /// <returns>The calculated rotation for the menu.</returns>
    private Quaternion CalculateMenuRotation(Vector3 menuPosition)
    {
        // Direction from menu to player
        Vector3 directionToPlayer = (playerTransform.position - menuPosition).normalized;

        // Create a rotation that looks at the player
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

        // Ensure the menu remains upright by zeroing out pitch and roll
        Vector3 euler = lookRotation.eulerAngles;
        euler.x = 0;
        euler.z = 0;
        lookRotation = Quaternion.Euler(euler);

        // If the menu is still 180 degrees off, adjust here
        // This can be toggled or adjusted based on your prefab's orientation
        // For example, uncomment the next line if needed:
         lookRotation *= Quaternion.Euler(0, 180f, 0);

        return lookRotation;
    }

    // Removed the Update method as continuous updates are no longer needed
}
