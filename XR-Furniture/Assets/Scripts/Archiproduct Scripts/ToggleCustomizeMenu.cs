using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCustomizeMenu : MonoBehaviour
{
    private Outline outline;

    [SerializeField] private GameObject menuPrefab;

    private GameObject instantiatedMenu;
    private bool isMenuActive = false;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        if (outline != null)
        {
            if (outline.enabled == true && CheckAInput())
            {

                Debug.Log("--------------Ainput----------------");
                ToggleMenu();
            }
        }

    }
    private void ToggleMenu()
    {
        Debug.Log("ToggleCustomizeMenu called.");
        if (instantiatedMenu == null)
        {
            Transform menuTransform = transform.Find("MenuTransform");
            instantiatedMenu = Instantiate(menuPrefab, menuTransform);
            isMenuActive = true;
        }
        else
        {
            isMenuActive = !isMenuActive;
            instantiatedMenu.SetActive(isMenuActive);
        }
    }


    private bool CheckAInput()
    {
        return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch);
    }
}
