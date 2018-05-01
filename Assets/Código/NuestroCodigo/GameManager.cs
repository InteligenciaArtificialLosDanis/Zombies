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
    GameObject[,] casillas;
	// Use this for initialization
	void Start () {
        instance = this;

		tablero = new GameObject[AnchoTablero, AltoTablero];

        //Estamos guardando prefabs :/
        /*for (int i = 0; i < AnchoTablero; i++){
            for (int j = 0; j < AltoTablero; j++)
            {
                casillas[i, j] = suelo;
                casillas[i, j].GetComponent<Casilla>().setTipoCasilla(0);
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

                    casillas[i, j].GetComponent<Casilla>().setTipoCasilla(2);
                }
                else
                {
                    tablero[i, j] = Instantiate(suelo, new Vector3(i, j, 0), Quaternion.identity);

                    casillas[i, j] = Instantiate(suelo, new Vector3(i, j, 0), Quaternion.identity);
                    casillas[i, j].GetComponent<Casilla>().setTipoCasilla(0);
                    casillas[i, j].GetComponent<Casilla>().setPosMatriz(i, j);
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

            casillas[posCasilla.x, posCasilla.y].GetComponent<Casilla>().setTipoCasilla(1);

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
                        casillas[posX, posY].GetComponent<Casilla>().setTipoCasilla(3);
                        
                        numAliados++;
                    }

                    //Si no caben aliados se pondran enemigos hasta llenar el cupo
                    else if (numEnemigos < 20)
                    {
                        //Destroy(objetoCasilla);

                        //Instancia
                        tablero[posX, posY] = Instantiate(enemigo, ph, Quaternion.identity);
                        casillas[posX, posY].GetComponent<Casilla>().setTipoCasilla(4);
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
                        casillas[posX, posY].GetComponent<Casilla>().setTipoCasilla(4);
                        numEnemigos++;
                    }

                    //Si no caben más enemigos se instancia suelo
                    else
                    {
                        Destroy(tablero[posX, posY]);
                        if (numAliados > 0) numAliados--;
                        casillas[posX, posY].GetComponent<Casilla>().setTipoCasilla(0);
                        tablero[posX, posY] = objetoCasilla;

                    }

                    break;

                //Si se pulsa sobre un enemigo se pondrá siempre suelo
                case 4:

                    Destroy(tablero[posX, posY]);
                    if (numEnemigos > 0) numEnemigos--;
                    casillas[posX, posY].GetComponent<Casilla>().setTipoCasilla(0);
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
            Vector2Int[] Buenos = new Vector2Int[5];
            GameObject[] listaMalos = new GameObject[20];
            int contMalos = 0;

            for (int i = 0; i < AltoTablero; i++)
            {
                for (int j = 0; j < AnchoTablero; j++)
                {
                    //Guardo las posiciones, sin importar que sea aliado o heroe
                    if (tablero[i, j].tag == "Aliados" )
                    {
                        Buenos[contBuenos] = new Vector2Int(i, j); 
                        contBuenos++;
                    }

                    else if (tablero[i, j].tag == "Enemigos")
                    {
                        listaMalos[contMalos] = tablero[i, j];
                        tablero[i, j].GetComponent<IAEnemigos>().setPosicionEnemigo(i, j);
                        
                    }

                    else if (tablero[i, j].tag == "Heroe")
                    {
                        posHeroeX = i;
                        posHeroeY = j;
                    }
                }
            }//for

            foreach (GameObject enemigo in listaMalos)
            {
                Vector2Int posEnemigo = enemigo.GetComponent<IAEnemigos>().getPos();
                Vector2Int posObjetivo = enemigo.GetComponent<IAEnemigos>().buscaCercano(Buenos, new Vector2Int (posHeroeX, posHeroeY));

                if (posEnemigo.y != posObjetivo.y)
                {
                    if (posEnemigo.y < posObjetivo.y)
                    {
                        enemigo.gameObject.transform.position = new Vector3(posEnemigo.x, posEnemigo.y - 1, 0);

                        tablero[posEnemigo.x, posEnemigo.y] = Instantiate(suelo, new Vector3(posEnemigo.x, posEnemigo.y, 0), Quaternion.identity);
                        tablero[posEnemigo.x, posEnemigo.y].GetComponent<Casilla>().setTipoCasilla(0);

                        enemigo.GetComponent<IAEnemigos>().setPosicionEnemigo(posEnemigo.x, posEnemigo.y - 1);
                    }

                    else
                    {
                        enemigo.gameObject.transform.position = new Vector3(posEnemigo.x, posEnemigo.y + 1, 0);

                        tablero[posEnemigo.x, posEnemigo.y] = Instantiate(suelo, new Vector3(posEnemigo.x, posEnemigo.y, 0), Quaternion.identity);
                        tablero[posEnemigo.x, posEnemigo.y].GetComponent<Casilla>().setTipoCasilla(0);

                        enemigo.GetComponent<IAEnemigos>().setPosicionEnemigo(posEnemigo.x, posEnemigo.y + 1);
                    }

                }

                else
                {
                    if (posEnemigo.x < posObjetivo.x)
                    {
                        enemigo.gameObject.transform.position = new Vector3(posEnemigo.x + 1, posEnemigo.y, 0);

                        tablero[posEnemigo.x, posEnemigo.y] = Instantiate(suelo, new Vector3(posEnemigo.x, posEnemigo.y, 0), Quaternion.identity);
                        tablero[posEnemigo.x, posEnemigo.y].GetComponent<Casilla>().setTipoCasilla(0);

                        enemigo.GetComponent<IAEnemigos>().setPosicionEnemigo(posEnemigo.x + 1, posEnemigo.y);
                    }

                    else
                    {
                        enemigo.gameObject.transform.position = new Vector3(posEnemigo.x - 1, posEnemigo.y, 0);

                        tablero[posEnemigo.x, posEnemigo.y] = Instantiate(suelo, new Vector3(posEnemigo.x, posEnemigo.y, 0), Quaternion.identity);
                        tablero[posEnemigo.x, posEnemigo.y].GetComponent<Casilla>().setTipoCasilla(0);

                        enemigo.GetComponent<IAEnemigos>().setPosicionEnemigo(posEnemigo.x - 1, posEnemigo.y);
                    }
                }

                //Después de resolver todos los ifs la posición del enemigo apuntará a la casilla que te quieres mover.
                //Habrá que revisar qué hay en esa casilla. Si hay un bueno resolver el conflicto y guardar en el tablero lo que quede,
                //y si no se topa con nadie se actualiza el tablero como tipo zombie

                Vector2Int movimiento = enemigo.GetComponent<IAEnemigos>().getPos();

                //Si están en la misma casilla pelean
                if (tablero[movimiento.x, movimiento.y].tag == "Aliados" || tablero[movimiento.x, movimiento.y].tag == "Heroe")
                {
                    int porcentajeGanador = 0;

                    if (contBuenos + 1 >= 3) porcentajeGanador = 90;
                    else if (contBuenos + 1 == 2) porcentajeGanador = 50;
                    else if (contBuenos + 1 == 1) porcentajeGanador = 20;

                    if (noche) porcentajeGanador -= 10;

                    int combate = Random.Range(1, 100);

                    //Si el número aleatorio está dentro del porcentajeGanador el aliado gana el combate
                    if (combate <= porcentajeGanador)
                    {
                        //Se destruye el enemigo y se suman los puntos correspondientes
                        Destroy(enemigo);

                        if (tablero[movimiento.x, movimiento.y].tag == "Heroe") puntuacion += 5;
                        else puntuacion++;
                    }

                    else
                    {

                        if (tablero[movimiento.x, movimiento.y].tag == "Aliados")
                        {
                            Destroy(tablero[movimiento.x, movimiento.y]);

                            casillas[movimiento.x, movimiento.y].GetComponent<Casilla>().setTipoCasilla(4);

                            tablero[movimiento.x, movimiento.y] = enemigo;

                        }

                        //Hay que mostrar algo por pantalla si muere el héroe (sin necesidad de modificar los parámetros)

                      
                    }
                }

            }
           

        }   
    }

    public void callbackReinicio()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
