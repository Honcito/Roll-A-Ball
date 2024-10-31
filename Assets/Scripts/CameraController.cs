using System.Net.NetworkInformation;
using UnityEngine;

public class CameraController : MonoBehaviour 
{

    public GameObject player; // Will reference the player game object's position
    private Vector3 offset; // Will set the offset position from the player to the camera

    // Start is called before the first frame update
    private void Start()
    {

        // Calculate the offset position between the camera and the player at the start of the game
        // it subtracts the palyer's position from the camera's
        offset = transform.position - player.transform.position;
        
    }

    // Update is called once per frame
    private void Update()
    {

        // Sets the camera to where the player is plus the offset set above
        transform.position = player.transform.position + offset;
        
    }

}
