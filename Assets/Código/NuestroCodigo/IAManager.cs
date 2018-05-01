using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Realiza un paso completo de la IA:
    //*Mueve los zombies
    //*Mueve al heroe
    //*Resuelve colisiones (Si las hubiera)
    public void turnoIA(GameObject[,] tablero, Vector2Int[] posBuenos, Vector2Int[] posMalos)
    {
        //MOVIMIENTO DE LOS ZOMBIES
        for (int i = 0; i < posMalos.Length; i++)
        {
            int x = posMalos[i].x;       if (x == -1) continue;
            int y = posMalos[i].y;

            tablero[x, y].GetComponent<IAEnemigos>().buscaCercano(posBuenos, posMalos[i]);

            //Luego puedes acceder al tablero[x,y] para hacerle el transform en funcion de lo que te devuelva 
            //buscaCercano, que puede ser un float o un "oye muevete a tal sitio"

            //MOVIMIENTO DEL HEROE
            //Tengo que separar el heroe de los aliados en el vector 2
        }
    }

    //Resuelve las colisiones, si hay
    void resuelveColisiones(GameObject obj1, GameObject obj2)
    {

    }

}
