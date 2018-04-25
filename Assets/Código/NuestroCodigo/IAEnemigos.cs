using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAEnemigos : MonoBehaviour {

    //Debemos hacer un A* que localice al enemigo o heroe más cercano y debe desplazarse una casilla hacia él cada turno
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Busca en el tablero cual es el aliado o héroe más cercano
    //Habría que pasarle el tablero en cada turno, ya que puede cambiar en el transcurso de la partida (muertes...)
    void buscaCercano(GameObject[,] tablero, int alto, int ancho)
    {

        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {

                if (tablero[i, j].tag == "Heroe" || tablero[i, j].tag == "Aliados")
                {
                    //Se compara si está más cerca que la distancia que esté guardada.
                    //Esta se optiene restando la posición del aliado a la del robot y sacando el valor absoluto
                }

            }
        }

    }
}
