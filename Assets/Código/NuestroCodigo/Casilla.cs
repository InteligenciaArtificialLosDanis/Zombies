using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour {

   public Vector2Int posMatriz;
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

        Debug.Log(posMatriz);
        
    }

    public Vector2Int getPosMatriz()
    {
        Debug.Log("Si salgo, preocupate");
        return posMatriz;
    }

    void OnMouseDown () {
        GameManager.instance.onClick(this.gameObject);
 
    }

}
