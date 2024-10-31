using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Erased Start method, don't needed.


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

        
    }
}
