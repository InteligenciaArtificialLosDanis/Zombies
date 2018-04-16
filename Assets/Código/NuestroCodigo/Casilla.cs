using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour {

    Vector2Int posMatriz = new Vector2Int(0, 0);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		
	}

    //Movidas para la posMatriz
    void setPosMatriz(int x, int y)
    {
        posMatriz.x = x;
        posMatriz.y = y;
        
    }

    public Vector2Int getPosMatriz()
    {
        return posMatriz;
    }

    void OnMouseDown () {
        GameManager.instance.onClick(this.gameObject);
    }

}
