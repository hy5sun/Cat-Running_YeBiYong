using UnityEngine;
using System.Collections;

public class Scroll_Mapping1 : MonoBehaviour {

	public float ScrollSpeed = 0.5f;
	float Target_offset;
	
	// Update is called once per frame
	void Update () {
		Target_offset += Time.deltaTime * ScrollSpeed;
		renderer.material.mainTextureOffset = new Vector2 (Target_offset, 0);
	}
}
