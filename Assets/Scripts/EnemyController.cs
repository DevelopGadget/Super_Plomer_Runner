using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float maxSpeed = 1f;
	public float speed = 5f;
	private Animator Animacion;
	private Rigidbody2D rb2d;
	private Vector3 start, End;
	private bool anim, Control = true;
	public Transform Target;
	public GameObject Player;

	// Use this for initialization
	void Start () {
		transform.parent = null;
		rb2d = GetComponent<Rigidbody2D>();
		Animacion = GetComponent<Animator> ();
		if (gameObject.tag.Equals ("Enemigo Tipo 3")) {
			Animacion.SetBool ("Caer", true);
			if (Target != null) Target.parent = null; start = transform.position; End = Target.position;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!gameObject.tag.Equals ("Puas")) {
			if (Animacion.GetBool ("IsAlive")) {
				rb2d.AddForce (Vector2.right * speed);	
				rb2d.velocity = new Vector2 (Mathf.Clamp (rb2d.velocity.x, -maxSpeed, maxSpeed), rb2d.velocity.y);
				if (rb2d.velocity.x > -0.01f && rb2d.velocity.x < 0.01f) {
					speed = -speed;
					rb2d.velocity = new Vector2 (speed, rb2d.velocity.y);
				}
				if (speed < 0) {
					transform.localScale = new Vector3 (7f, 7f, 1f);
				} else if (speed > 0) {
					transform.localScale = new Vector3 (-7f, 7f, 1f);
				}
			}
			if (gameObject.tag.Equals ("Enemigo Tipo 3")) {
				EnemigoTipo3 ();
				if (Control) {
					if (Target != null)
						transform.position = Vector3.MoveTowards (transform.position, Target.position, 8f * Time.deltaTime);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (gameObject.tag == "Enemigo Tipo 1") {
			if (col.gameObject.tag == "Player") {
				if ((transform.position.y - (col.transform.position.y - transform.position.y) < col.transform.position.y - 0.64f)) {
					col.SendMessage ("EnemyJump");
					Player.SendMessage ("EnemigoPuntos", (int)Random.Range (250, 500));
					Anim ();
				} else {
					Player.SendMessage ("EnemyKnockBack", transform.position.x);
				}
			} else if (col.gameObject.tag == "Disparo") {
				if ((transform.position.y - (col.transform.position.y - transform.position.y) < col.transform.position.y - 0.64f)) {
					Player.SendMessage ("EnemigoPuntos", (int)Random.Range (150, 400));
					Anim ();
				}
			}

		} else if (gameObject.tag == "Enemigo Tipo 2") {
			if (col.gameObject.tag == "Disparo") {
				Anim ();
				Player.SendMessage ("EnemigoPuntos", (int)Random.Range (500, 750));
			} else if (col.gameObject.tag == "Player") {
				Player.SendMessage ("EnemyKnockBack");
			}
		} else if (gameObject.tag == "Enemigo Tipo 3") {
			if (col.gameObject.tag == "Player") Player.SendMessage ("EnemyKnockBack");
		} else if (gameObject.tag == "Puas") {
			if (col.gameObject.tag == "Player") Player.SendMessage ("EnemyKnockBack");
		}
	}

	private void die(){
		Destroy(gameObject);
	}
		
	private void Anim(){
		rb2d.isKinematic = true;
		rb2d.velocity = Vector2.zero;
		Destroy (transform.GetChild(0).gameObject);
		Animacion.SetBool ("IsAlive", false);
		Invoke ("die", 1f);
	}

	private void EnemigoTipo3(){
		if (transform.position == Target.position) {
			if (Target.position == start) {
				Valores (false, true);
				Target.position = End;
				Invoke ("ControlEnemigo", 2.5f);
			} else {
				Valores (false, false);
				Target.position = start;
				Invoke ("ControlEnemigo", 2.5f);
			}
		}
		//Target.position = (Target.position == start) ? End : start;
	}

	private void Valores(bool Control, bool anim){
		this.Control = Control;
		this.anim = anim;
	}

	private void ControlEnemigo(){
		Animacion.SetBool ("Caer", anim);
		Control = true;
	}

}
