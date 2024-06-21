using UnityEditor;
using UnityEngine;

public class AutoBoxColliderEditor : Editor
{
    [MenuItem("Tools/Add BoxCollider to Selected Object")]
    private static void AddBoxColliderToSelectedObject()
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            AutoBoxColliderForChildren autoBoxCollider = selectedObject.GetComponent<AutoBoxColliderForChildren>();
            if (autoBoxCollider != null)
            {
                autoBoxCollider.AddBoxCollider();
            }
            else
            {
                Debug.LogError("Selected object does not have an AutoBoxColliderForChildren component.");
            }
        }
        else
        {
            Debug.LogError("No object selected.");
        }
    }
}
