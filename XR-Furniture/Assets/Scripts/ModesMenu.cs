using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModesMenu : MonoBehaviour
{

    public GameObject categoryMenu;
    public GameObject modesMenu;
    public GameObject repaintMenu;

    public Button furnitures;
    public Button repaint;

    // Start is called before the first frame update
    void Start()
    {

        furnitures.onClick.AddListener(() => FurnishMode());
        repaint.onClick.AddListener(() => RepaintMode());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FurnishMode()
    {
        categoryMenu.SetActive(true);
        modesMenu.SetActive(false);
        GameManager.instance.ChangeState(GameManager.AppStates.PlacementMode);


    } 
    public void RepaintMode()
    {
        repaintMenu.SetActive(true);
        modesMenu.SetActive(false);
        GameManager.instance.ChangeState(GameManager.AppStates.RepaintMode);
    }
}
