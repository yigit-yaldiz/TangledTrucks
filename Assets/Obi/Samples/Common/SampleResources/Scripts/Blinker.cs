using UnityEngine;

public class Blinker : MonoBehaviour {

	public UnityEngine.Color highlightColor;

 	private Renderer rend;
	private UnityEngine.Color original;

	void Awake(){
		rend = GetComponent<Renderer>();
		original = rend.material.color;
	}

	public void Blink(){
		rend.material.color = highlightColor;
	}

	void LateUpdate(){
		rend.material.color += (original - rend.material.color)*Time.deltaTime*5;
	}

}
