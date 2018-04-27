using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class IAEnemigos : MonoBehaviour {

    Vector2 posHeroe = new Vector2(-1,-1);
    Vector2 posAliado1 = new Vector2(-1, -1);
    Vector2 posAliado2 = new Vector2(-1, -1);
    Vector2 posAliado3 = new Vector2(-1, -1);
    Vector2 posAliado4 = new Vector2(-1, -1);
    Vector2 posAliado5 = new Vector2(-1, -1);

	// Use this for initialization
	void Start () {
		


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Guarda cada posición del héroe
    public void setPosHeroe(int posX, int posY) {

        posHeroe.x = posX;
        posHeroe.y = posY;

    }

    //Guarda la posición de cada uno de los aliados
    public void setPosAliado(int posX, int posY, int numAliado)
    {
        switch (numAliado)
        {
            case 1:
                posAliado1.x = posX;
                posAliado1.y = posY;
                break;

            case 2:
                posAliado2.x = posX;
                posAliado2.y = posY;
                break;

            case 3:
                posAliado3.x = posX;
                posAliado3.y = posY;
                break;

            case 4:
                posAliado4.x = posX;
                posAliado4.y = posY;
                break;

            case 5:
                posAliado5.x = posX;
                posAliado5.y = posY;
                break;
        }
    }
    float distanciaEuclidea(Vector2 posPropia, Vector2 posDestino)
    {
        return Mathf.Abs(posDestino.x - posPropia.x) + Mathf.Abs(posDestino.y - posPropia.y);
    }

    //Busca en el tablero cual es el aliado o héroe más cercano
    //Habría que pasarle el tablero en cada turno, ya que puede cambiar en el transcurso de la partida (muertes...)
    void buscaCercano(GameObject[,] tablero, int alto, int ancho)
    {

       

    }
}
