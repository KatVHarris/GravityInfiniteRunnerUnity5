using UnityEngine;
using System.Collections;

public class TestEvents : MonoBehaviour {

	private bool _testBool;
	public bool testBool {
		get {
			return _testBool;
		}
		set {
			_testBool = value;
			Debug.Log ("Test Bool set to "+value);
		}
	}

	private float _testFloat;
	public float testFloat {
		get {
			return _testFloat;
		} 
		set {
			_testFloat = value;
			Debug.Log ("Test Float set to "+value);
		}
	}

	public void TestButton() {
		Debug.Log ("Test Button pressed");
	}

}
