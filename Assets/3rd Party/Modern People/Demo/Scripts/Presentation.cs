using UnityEngine;
using System.Collections;

public class Presentation : MonoBehaviour {
	private bool left 	= true;
	private bool rotated= false;
	private bool move	= false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.x<35.5 && left){
			transform.Translate(Vector3.left * (Time.deltaTime*0.75f));
		}
		else if(!rotated){
			left = false;
			transform.Rotate(0, 180, 0, Space.World);
			rotated = true;
		}
		else if(transform.position.x>20){
			transform.Translate(Vector3.left * (Time.deltaTime*0.75f));
		}
		else if(!move){
			transform.Rotate(0, -90, 0, Space.World);
			move = true;
		}    
	}
}
