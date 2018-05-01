using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAAliada : MonoBehaviour {

    int ID;         //Identificacion del aliado

    int posX, posY; //Posicion en el tablero del aliado.

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setAliado(int id, int x, int y)
    {
        ID = id;
        posX = x;
        posY = y;

    }

    public int getId()
    {
        return ID;
    }

    public Vector2Int getPosition()
    {
        return new Vector2Int(posX, posY);
    }
}
