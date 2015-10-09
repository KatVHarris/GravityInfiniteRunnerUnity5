using UnityEngine;
using System.Collections;

public class MoveFoward : MonoBehaviour {

    float forwardSpeed;
    float initalSpeed = 6.0f;
    GameController gc; 
    // Use this for initialization
	void Start () {
        //GameController
        GameObject gameController = GameObject.Find("GameController");
        gc = gameController.GetComponent<GameController>();

        float difficulty = gc.difficulty;
        forwardSpeed = forwardSpeed + (initalSpeed / 3 * difficulty);
	}
	
	// Update is called once per frame
	void Update () {
        //add one third to basic speed
		transform.Translate(Vector3.forward * Time.deltaTime * (forwardSpeed));
	}
}
