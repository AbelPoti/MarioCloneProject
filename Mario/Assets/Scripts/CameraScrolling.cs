using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    //A reference to Mario
    private Transform playerTransform;

    public float height = 6.5f;
    public float undergroundHeight = -8.5f;

    private Color oGColor;

    private void Awake()
    {
        //since Mario is tagged with built-in Player tag
        playerTransform = GameObject.FindWithTag("Player").transform;

        oGColor = Camera.main.backgroundColor;
    }

    //Applies after frame updates
    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        //We call math function in order to prevent the camera to scroll to the left,
        //since in the original game you cannot go back to the left of the map
        cameraPosition.x = Mathf.Max(playerTransform.position.x, cameraPosition.x);
        transform.position = cameraPosition;
    }

    public void SetUnderground(bool underground)
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? undergroundHeight : height;
        transform.position = cameraPosition;
        
        Camera.main.backgroundColor = underground ? Color.black : oGColor;
    }
}
