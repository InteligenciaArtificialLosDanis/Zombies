using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    //Instancia
    public static GameManager instance;
    
    //GameObjects que vamos a usar.//
    //Entidades
    public GameObject heroe;
    public GameObject aliado;
    public GameObject enemigo;
    //suelo
    public GameObject suelo;
    //Entorno
    public GameObject casa;
    public GameObject sol;
    public GameObject luna;

    //Dimensiones del tablero:
    const int AltoTablero = 6;
    const int AnchoTablero = 12;

    //Booleanos de Control™
    bool heroeSituado = false;
    bool dia = true;
    bool noche = false;
    
    //Textos y botones
    public Text textoHeroe;
    public Button botonTurno;
    public ColorBlock colorBoton;

    //Posiciones necesarias para cosas™
    int posCasaX;
    int posCasaY;

    int posHeroeX;
    int posHeroeY;

    enum tipoCasilla { suelo, heroe, aliado, enemigo, casa };

    GameObject[,] tablero;
	// Use this for initialization
	void Start () {
        instance = this;

		tablero = new GameObject[AnchoTablero, AltoTablero];

        for (int i = 0; i < AnchoTablero; i++){
            for (int j = 0; j < AltoTablero; j++)
            {
                tablero[i, j] = suelo;
            }
        }

        creaTableroBase();
	}

    //Primero, ponemos la casa y el agente.
    //Después, puedes clickar en casillas para cambiar entre aliado zombie o nada.
    //(Teniendo el cuenta los limites)
    void creaTableroBase()
    {
        //Metemos la casa.
        int posCasaX = Random.Range(0, AnchoTablero - 1);
        int posCasaY = Random.Range(0, AltoTablero - 1);

        tablero[posCasaX, posCasaY] = casa;

        for (int i = 0; i < AnchoTablero; i++)
        {
            for (int j = 0; j < AltoTablero; j++)
            {
                if (tablero[i, j] == casa) 
                    Instantiate(casa, new Vector3(i, j, 0), Quaternion.identity);
                else { 
                    Instantiate(suelo, new Vector3(i, j, 0), Quaternion.identity);
                    tablero[i, j].GetComponent<Casilla>().setPosMatriz(i, j);
                }
            }
        }

    }
	
	// Update is called once per frame
	void Update () {

		
	}

    public void onClick(GameObject objetoCasilla)
    {

        Vector2Int posCasilla = objetoCasilla.GetComponent<Casilla>().posMatriz;
        Debug.Log(posCasilla);

        if (!heroeSituado)
        {
            tablero[posCasilla.x, posCasilla.y] = heroe;
            posHeroeX = posCasilla.x;
            posHeroeY = posCasilla.y;
            Vector3 ph = objetoCasilla.transform.position;
            Instantiate(heroe, ph, Quaternion.identity);
            //Heroe a true, quitamos el texto y habilitamos el botón
            textoHeroe.enabled = false;

            botonTurno.GetComponent<Image>().color = Color.white;
            botonTurno.colors = colorBoton;
  


            heroeSituado = true;
        }

    }


    public void callbackDiaNoche()
    {
        //Si es de día, cambiamos a noche
        if (dia)
        {
            sol.SetActive(false);
            luna.SetActive(true);

            dia = false;
            noche = true;
        }
         //Si es de noche, cambiamos a dia
        else
        {
            sol.SetActive(true);
            luna.SetActive(false);

            dia = true;
            noche = false;
        }
    }

    public void callbackTurno()
    {
        if (heroeSituado)
        {
            Debug.Log("JAJA SI");
        }
    }

    public void callbackReinicio()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
