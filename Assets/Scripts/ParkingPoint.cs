using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {        
        if (collider.TryGetComponent<CarAgent>(out CarAgent carAgent))
        {
            Debug.Log("Parking Point");            
        }
    }

}
