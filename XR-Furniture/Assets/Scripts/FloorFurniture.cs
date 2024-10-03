using UnityEngine;

public class FloorFurniture : Furniture
{
    protected override void Start()
    {
        base.Start();
        isPlaceable = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Furniture"))
        {
            isPlaceable = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Furniture"))
        {
            isPlaceable = true;
        }
    }
}
