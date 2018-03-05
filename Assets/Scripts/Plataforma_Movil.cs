using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma_Movil : MonoBehaviour {

	public Transform Target;
	private Vector3 start, End;
	public float Vel = 0.8f;

	// Use this for initialization
	void Start () {
		if (Target != null) Target.parent = null; start = transform.position; End = Target.position; transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		if (Target != null) transform.position = Vector3.MoveTowards (transform.position, Target.position, Vel * Time.deltaTime);
		if (transform.position == Target.position) Target.position = (Target.position == start) ? End : start;
	}
		
}
