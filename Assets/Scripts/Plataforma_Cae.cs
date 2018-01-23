using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma_Cae : MonoBehaviour {
	
	private Rigidbody2D Plataforma;
	private PolygonCollider2D Col;
	private Vector3 start;

	// Use this for initialization
	void Start () {
		Plataforma = GetComponent<Rigidbody2D> ();
		Col = GetComponentInParent<PolygonCollider2D> ();
		start = Plataforma.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D Tag){
		if (Tag.gameObject.CompareTag ("Player")) Invoke ("Dalay", 0.5f); Invoke ("Respawn", 4f);
	}

	void Dalay(){
		Plataforma.isKinematic = false;
		Col.isTrigger = true;
	}

	void Respawn(){
		Plataforma.isKinematic = true;
		Plataforma.velocity = Vector3.zero;
		Plataforma.transform.position = start;
		Col.isTrigger = false;
	}
}
