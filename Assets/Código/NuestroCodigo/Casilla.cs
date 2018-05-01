using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour {

   public Vector2Int posMatriz;

    //0 = vacia, 1 = heroe, 2 = casa, 3 = aliado, 4 = enemigo
   public enum tipoCasilla { vacia, heroe, casa, aliado, enemigo};

    public tipoCasilla tCasilla;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		
	}

    //Movidas para la posMatriz
    public void setPosMatriz(int x, int y)
    {
        posMatriz.x = x;
        posMatriz.y = y;

        //Debug.Log(posMatriz);
        
    }

    public Vector2Int getPosMatriz()
    {
        //Debug.Log("Si salgo, preocupate");
        return posMatriz;
    }

    //public tipoCasilla getTipoCasilla() { return tCasilla; }

    public void setTipoCasilla(int t)
    {

        switch (t)
        {
            case 0: //Vacia
                tCasilla = tipoCasilla.vacia;
                break;

            case 1: //Heroe
                tCasilla = tipoCasilla.heroe;
                break;

            case 2: //Casa
                tCasilla = tipoCasilla.casa;
                break;

            case 3: //Aliado
                tCasilla = tipoCasilla.aliado;
                break;

            case 4: //Enemigo
                tCasilla = tipoCasilla.enemigo;
                break;



        }

    }

    void OnMouseUp () {
        GameManager.instance.onClick(this.gameObject);
 
    }

}
