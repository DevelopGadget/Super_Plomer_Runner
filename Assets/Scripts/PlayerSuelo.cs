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
		if(Colision.gameObject.tag.Equals("Movil")) Player.transform.parent = Colision.transform; RPlayer.velocity = Vector3.zero;
		colision (Colision.gameObject.tag);
	}

	void OnCollisionStay2D(Collision2D Colision){
		if(Colision.gameObject.tag.Equals("Movil")) Player.transform.parent = Colision.transform;
		colision (Colision.gameObject.tag);
	}

	void OnCollisionExit2D(Collision2D Colision){
		Player.transform.parent = null;
		Player.Suelo = false;
	}

	private void colision(string Target){
		if (Target.Equals ("Plataforma") || Target.Equals("Disparo")) {
			Player.Suelo = true;
		} else if (Target.Equals ("Movil") || Target.Equals("Disparo")) {
			Player.Suelo = true;
		} else {
			Player.Suelo = false;
		}
	}
}
