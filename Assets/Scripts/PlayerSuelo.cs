using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuelo : MonoBehaviour {

	private PlayerController Player;
	private Rigidbody2D RPlayer;

	// Use this for initialization
	void Start ()
	{
		Player = GetComponentInParent<PlayerController> ();
		RPlayer = GetComponentInParent<Rigidbody2D> ();
	}

	void OnCollisionEnter2D(Collision2D Colision){
		if (Colision.gameObject.tag.Equals ("Movil")) {
			Player.transform.parent = Colision.transform;
			RPlayer.velocity = Vector3.zero;
			Player.Suelo = true;
		}
		colision (Colision.gameObject.tag);
		Escalar ();
	}

	void OnCollisionStay2D(Collision2D Colision){
		colision (Colision.gameObject.tag);
		if (Colision.gameObject.tag.Equals ("Plataforma Cae") && !Colision.gameObject.GetComponentInParent<Rigidbody2D>().isKinematic) Player.Suelo = false;
	}

	void OnCollisionExit2D(Collision2D Colision){
		Player.transform.parent = null;
		Player.Suelo = false;
	}

	void OnTriggerStay2D (Collider2D Colision)
	{
		colision (Colision.tag);
		if (Colision.tag.Equals ("Plataforma Cae") && !Colision.gameObject.GetComponentInParent<Rigidbody2D>().isKinematic) Player.Suelo = false;
	}

	void OnTriggerExit2D(Collider2D Colision){
		Player.transform.parent = null;
		Player.Suelo = false;
	}

		
	private void colision(string Target){
		Player.Suelo = (Target.Equals ("Plataforma") || Target.Equals("Disparo") || Target.Equals ("Movil") || Target.Equals ("Plataforma Cae")) ? true : false;
	}

	private void Escalar(){
		if (transform.localScale.x > 0) transform.parent.localScale = new Vector3 (5f, 5f, 1f);
		else transform.parent.localScale = new Vector3 (-5f, 5f, 1f);	
	}
}
