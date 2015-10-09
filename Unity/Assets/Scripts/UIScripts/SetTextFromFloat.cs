using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetTextFromFloat : MonoBehaviour {

	public Text UIText;
	public string floatFormat = "F2";
	[SerializeField]
	private float _floatVal;
	public float floatVal {
		get {
			return _floatVal;
		}
		set {
			_floatVal = value;
			if (UIText != null) {
				UIText.text = floatVal.ToString(floatFormat);
			}
		}
	}
}
