using UnityEngine;
using System.Collections;

public class SimpleShoot : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject clonedBullet; 
            clonedBullet = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere),
                transform.position, transform.rotation) as GameObject;
            clonedBullet.AddComponent<Rigidbody>();
            clonedBullet.GetComponent<Rigidbody>().AddForce(clonedBullet.transform.forward * 2000);
        }

    }
}
