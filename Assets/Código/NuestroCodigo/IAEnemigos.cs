using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class IAEnemigos : MonoBehaviour {




	// Use this for initialization
	void Start () {
		


	}
	
	// Update is called once per frame
	void Update () {
		
	}


    float distanciaEuclidea(Vector2 posPropia, Vector2 posDestino)
    {
        return Mathf.Abs(posDestino.x - posPropia.x) + Mathf.Abs(posDestino.y - posPropia.y);
    }

    //Busca en el tablero cual es el aliado o héroe más cercano
    //Dado un vector de dónde están los buenos y una posicion, determina cual es el mejor
    public void buscaCercano(Vector2 [] listaBuenos, Vector2 PosEnemigo)
    {
        
       

    }
}
