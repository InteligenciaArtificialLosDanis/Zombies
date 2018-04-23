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

    //Contadores
    int numAliados = 0;     //max 5
    int numEnemigos = 0;    //max 20

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

        Vector3 ph = objetoCasilla.transform.position;

        if (!heroeSituado)
        {
            tablero[posCasilla.x, posCasilla.y] = heroe;
            posHeroeX = posCasilla.x;
            posHeroeY = posCasilla.y;

            Destroy(objetoCasilla);

            Instantiate(heroe, ph, Quaternion.identity);
            //Heroe a true, quitamos el texto y habilitamos el botón
            textoHeroe.enabled = false;

            botonTurno.GetComponent<Image>().color = Color.white;
            botonTurno.colors = colorBoton;
  


            heroeSituado = true;
        }

        //Cuando el héroe ya está colocado y se pulsa en una casilla que no sea héroe ni casa, esta rotará entre suelo, aliado y zombie
        else if (heroeSituado && objetoCasilla.tag != "Heroe")
        {
            //Si se pulsa suelo cambiará a aliado o enemigo según capacidad
            if (objetoCasilla.tag == "Suelo")
            {
                //Si caben aliados se cambiará por uno
                if (numAliados < 5)
                {
                   Destroy(objetoCasilla);

                   Instantiate(aliado, ph, Quaternion.identity);
                    numAliados++;
                }

                //Si no caben aliados se pondran enemigos hasta llenar el cupo
                else if ( numEnemigos < 20)
                {
                    Destroy(objetoCasilla);

                    Instantiate(enemigo, ph, Quaternion.identity);
                    numEnemigos++;
                }

            }

            //Si se pulsa aliado cambiará a enemigo o suelo según capacidad
            else if (objetoCasilla.tag == "Aliados")
            {
                //Si caben enemigos se añadiran hasta llenar el cupo
                if (numEnemigos < 20)
                {
                    Destroy(objetoCasilla);
                    if (numAliados > 0) numAliados--;

                    Instantiate(enemigo, ph, Quaternion.identity);
                    numEnemigos++;
                }

                //Si no caben más enemigos se instancia suelo
                else
                {
                    Destroy(objetoCasilla);
                    if (numAliados > 0) numAliados--;

                    Instantiate(suelo, ph, Quaternion.identity);

                }

            }

            //Si se pulsa sobre un enemigo se pondrá siempre suelo
            else if (objetoCasilla.tag == "Enemigos")
            {
                Destroy(objetoCasilla);
                if (numEnemigos > 0) numAliados--;

                Instantiate(suelo, ph, Quaternion.identity);

            }


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
