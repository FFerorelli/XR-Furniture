using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Define public states
    public enum AppStates
    {
        PlacementMode,
        RepaintMode
    }

    // Public variable to hold the current state
    public AppStates currentState;

    // You can use this script as a singleton to access it from other parts of your app
    public static GameManager instance;

    private void Awake()
    {

        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Function to change the state
    public void ChangeState(AppStates newState)
    {
        currentState = newState;

        // You can add additional logic or callbacks based on state changes if needed
        Debug.Log("Changed to state: " + newState);
    }

    // Example usage from another script
    //void Start()
    //{
    //    // Change to PlacementMode
    //    GameManager.instance.ChangeState(GameManager.AppStates.PlacementMode);

    //    // Access the current state
    //    GameManager.AppStates currentState = GameManager.instance.currentState;
    //    Debug.Log("Current State: " + currentState);
    //}

}
