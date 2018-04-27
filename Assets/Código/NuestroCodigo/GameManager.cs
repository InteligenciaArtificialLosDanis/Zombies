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

    //Puntuación
    int puntuacion = 0;
    public Text t;      //Texto que aparece por pantalla con la puntuación

    //enum tipoCasilla { suelo, heroe, aliado, enemigo, casa };

    GameObject[,] tablero;
	// Use this for initialization
	void Start () {
        instance = this;

		tablero = new GameObject[AnchoTablero, AltoTablero];

        //Estamos guardando prefabs :/
        /*for (int i = 0; i < AnchoTablero; i++){
            for (int j = 0; j < AltoTablero; j++)
            {
                tablero[i, j] = suelo;
            }
        }*/

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
                {
                    Instantiate(suelo, new Vector3(i, j, 0), Quaternion.identity);
                    Instantiate(casa, new Vector3(i, j, 0), Quaternion.identity);
                }
                else
                {
                    tablero[i, j] = Instantiate(suelo, new Vector3(i, j, 0), Quaternion.identity);
                    tablero[i, j].GetComponent<Casilla>().setPosMatriz(i, j);
                }
            }
        }

    }
	
	// Update is called once per frame
	void Update () {

        //De momento la puntuación se actualiza en el update
        t.text = "Puntuacion: " + puntuacion;
	}

    //Modifica los puntos según los que le vengan de la IA
    public void setPuntuacion (int newPuntos){

        puntuacion += newPuntos;

    }

    public void onClick(GameObject objetoCasilla)
    {

        Vector2Int posCasilla = objetoCasilla.GetComponent<Casilla>().posMatriz;
        //Debug.Log(posCasilla);

        Vector3 ph = objetoCasilla.transform.position;

        if (!heroeSituado)
        {
            tablero[posCasilla.x, posCasilla.y] = heroe;
            posHeroeX = posCasilla.x;
            posHeroeY = posCasilla.y;

            //Destroy(objetoCasilla);

            tablero[posHeroeX, posHeroeY] = Instantiate(heroe, ph, Quaternion.identity);
            //Llamar al setPosHeroe :3

            //Heroe a true, quitamos el texto y habilitamos el botón
            textoHeroe.enabled = false;

            botonTurno.GetComponent<Image>().color = Color.white;
            botonTurno.colors = colorBoton;



            heroeSituado = true;
        }

        //Cuando el héroe ya está colocado y se pulsa en una casilla que no sea héroe ni casa, esta rotará entre suelo, aliado y zombie
        else if (objetoCasilla.GetComponent<Casilla>().posMatriz.x != posHeroeX && objetoCasilla.GetComponent<Casilla>().posMatriz.y != posHeroeY
            && objetoCasilla.GetComponent<Casilla>().posMatriz.x != posCasaX && objetoCasilla.GetComponent<Casilla>().posMatriz.y != posCasaY)
        {
            //Debug.Log(objetoCasilla.tag);
            //Debug.Log(numAliados);
            //Debug.Log(numEnemigos);

            int posX = objetoCasilla.GetComponent<Casilla>().posMatriz.x;
            int posY = objetoCasilla.GetComponent<Casilla>().posMatriz.y;

            switch ((int)objetoCasilla.GetComponent<Casilla>().tCasilla)
            {
                //Si se pulsa suelo cambiará a aliado o enemigo según capacidad
                case 0:
            
                    //Si caben aliados se cambiará por uno
                    if (numAliados < 5)
                    {
                        //Destroy(objetoCasilla);

                        //Instancia
                        tablero[posX, posY] = Instantiate(aliado, ph, Quaternion.identity);
                        tablero[posX, posY].GetComponent<IAAliada>().setAliado(numAliados, posX, posY); // Al Id le pasamos del 0 a 4. Hay que tener en cuenta eso.
                        objetoCasilla.GetComponent<Casilla>().setTipoCasilla(3);
                        
                        numAliados++;
                    }

                    //Si no caben aliados se pondran enemigos hasta llenar el cupo
                    else if (numEnemigos < 20)
                    {
                        //Destroy(objetoCasilla);

                        //Instancia
                        tablero[posX, posY] = Instantiate(enemigo, ph, Quaternion.identity);
                        objetoCasilla.GetComponent<Casilla>().setTipoCasilla(4);
                        numEnemigos++;
                    }

                    break;

                //Si se pulsa aliado cambiará a enemigo o suelo según capacidad
                case 3:
            

                    
                    //Si caben enemigos se añadiran hasta llenar el cupo
                    if (numEnemigos < 20)
                    {
                        Destroy(tablero[posX, posY]);
                        if (numAliados > 0) numAliados--;

                        //Instancia
                        tablero[posX, posY] = Instantiate(enemigo, ph, Quaternion.identity);
                        objetoCasilla.GetComponent<Casilla>().setTipoCasilla(4);
                        numEnemigos++;
                    }

                    //Si no caben más enemigos se instancia suelo
                    else
                    {
                        Destroy(tablero[posX, posY]);
                        if (numAliados > 0) numAliados--;
                        objetoCasilla.GetComponent<Casilla>().setTipoCasilla(0);
                        tablero[posX, posY] = objetoCasilla;

                    }

                    break;

                //Si se pulsa sobre un enemigo se pondrá siempre suelo
                case 4:

                    Destroy(tablero[posX, posY]);
                    if (numEnemigos > 0) numEnemigos--;
                    objetoCasilla.GetComponent<Casilla>().setTipoCasilla(0);
                    tablero[posX, posY] = objetoCasilla;
                    break;


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
           //Hacemos un barrido de todas las posiciones de los actores de juego
            int contBuenos = 0;
            int contMalos = 0;
            Vector2[] Buenos = new Vector2[6];
            Vector2[] Malos = new Vector2[20];
            
            for (int i = 0; i < AltoTablero; i++)
            {
                for (int j = 0; j < AnchoTablero; j++)
                {
                    //Guardo las posiciones, sin importar que sea aliado o heroe
                    if (tablero[i, j].tag == "Heroe" || tablero[i, j].tag == "Aliados" )
                    {
                        Buenos[contBuenos] = new Vector2(i, j); 
                        contBuenos++;
                    }

                    else if (tablero[i, j].tag == "Enemigos")
                    {
                        Malos[contMalos] = new Vector2(i, j);
                        contMalos++;
                    }
                }
            }//for

            //Ahora cedemos el control a la IA, pasandole el tablero y los vectores de posiciones
        }   
    }

    public void callbackReinicio()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
