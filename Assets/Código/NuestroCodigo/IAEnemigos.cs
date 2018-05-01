using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class IAEnemigos : MonoBehaviour {

    Vector2Int posicionEnemigo;


	// Use this for initialization
	void Start () {
		


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setPosicionEnemigo(int x, int y)
    {
        posicionEnemigo = new Vector2Int(x, y);
    }

    public Vector2Int getPos()
    {
        return posicionEnemigo;
    }


    float distanciaEuclidea(Vector2Int posPropia, Vector2Int posDestino)
    {
        return Mathf.Abs(posDestino.x - posPropia.x) + Mathf.Abs(posDestino.y - posPropia.y);
    }

    //Busca en el tablero cual es el aliado o héroe más cercano
    //Dado un vector de dónde están los buenos y una posicion, determina cual es el mejor
    public Vector2Int buscaCercano(Vector2Int [] listaBuenos, Vector2Int posHeroe)
    {
        float distanciaMenor = distanciaEuclidea(posicionEnemigo, posHeroe);
        Vector2Int objetivo = posHeroe;

        foreach (Vector2Int aliado in listaBuenos)
        {
            float aux = distanciaEuclidea(posicionEnemigo, aliado);

            if (aux < distanciaMenor) {
                distanciaMenor = aux;
                objetivo = aliado;
            }
        }

        return objetivo;

    }
}
