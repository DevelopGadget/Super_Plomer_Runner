using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Text Estrella, Score, Veces_Muerto, Enemigos_Muertos;
	private int Point, ScoreP, Vidas, Enemigos;
	public GameObject Imagen;
	private bool Game_Over = false;
	public static GameController Game;
	public GameObject Vidas_Object;

	void Awake(){
		if (Game == null) {
			Game = this;
			if(Imagen != null) DontDestroyOnLoad (gameObject);
		} else if (Game != this && Game.Game_Over) {
			Estrella.text = " Estrellas Conseguidas: " + Game.Point;
			Score.text = " Score: " + Game.ScoreP;
			Veces_Muerto.text = " Veces Muerto: " + Game.Vidas;
			Enemigos_Muertos.text = " Enemigos Muertos: " + Game.Enemigos;
			Destroy (Game.gameObject);
		}
	}

	public void EscenaManager(string Escena){
		if (Escena.Equals ("Game Over") || Escena.Equals ("Passes")) Game_Over = true;
		SceneManager.LoadScene (Escena);
	}

	public void Estrella_Puntos(GameObject col){
		Destroy (col.gameObject);
		ScoreP += 1000;
		Estrella.text = "x" + ((++Point).ToString ()); 
		Score.text = "Score: " + ScoreP;
	}

	public void Llave(GameObject col){
		Imagen.SetActive(true);
		Destroy (col.gameObject);
	}

	public void Enemigo_Puntos(int Point){
		Enemigos++;
		ScoreP += Point;
		Score.text = "Score: " + ScoreP;
	}

	public void Vidas_Puntos(){
		Vidas++;
	}

	public void Enemigos_muertos(){
		Enemigos++;
	}

	public void Vidas_Inc(){
		Vidas_Object.gameObject.transform.GetChild (Vidas).gameObject.SetActive (false);
		Vidas++;
	}

	public int GetVida(){
		return Vidas;
	}
}
