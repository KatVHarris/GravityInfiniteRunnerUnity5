using UnityEngine;
using System.Collections;

public class OVRShoot : MonoBehaviour {

    RaycastHit hit;
    GameObject hitObject; 
	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5))
        {
            hitObject = hit.collider.gameObject;
            Debug.Log(hitObject.tag);                       
        }
       
    }
}
