using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma_Cae : MonoBehaviour {
	
	private Rigidbody2D Plataforma;
	private PolygonCollider2D Col;
	private Vector3 start;
	public GameObject Player;

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
		if (Tag.gameObject.CompareTag ("Player"))Invoke ("Dalay", 1f); Invoke ("Respawn", 4f);
	}

	void OnCollisionExit2D(Collision2D Tag){
		if (Tag.gameObject.CompareTag ("Player")) Tag.gameObject.GetComponent<PlayerController> ().Suelo = false;  Tag.transform.parent = null;
	}

	void Dalay(){
		Player.transform.parent = null;
		Player.GetComponent<PlayerController> ().Suelo = false;
		Col.isTrigger = true;
		Plataforma.isKinematic = false;
	}

	void Respawn(){
		Plataforma.isKinematic = true;
		Plataforma.velocity = Vector3.zero;
		Plataforma.transform.position = start;
		Col.isTrigger = false;
	}
}
