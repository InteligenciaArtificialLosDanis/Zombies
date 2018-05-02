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
    GameObject heroeEscena;
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

    bool final = false;
    
    //Textos y botones
    public Text textoHeroe;
    public Button botonTurno;
    public ColorBlock colorBoton;

    public Button botonNuevaPartida;

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

    public Text gameOver; //Texto que aparece si pierdes

    //enum tipoCasilla { suelo, heroe, aliado, enemigo, casa };

    GameObject[,] tablero;
    GameObject[,] casillas;
	// Use this for initialization
	void Start () {
        instance = this;

        //botonNuevaPartida.gameObject.SetActive(false);
        gameOver.enabled = false;

		tablero = new GameObject[AnchoTablero, AltoTablero];

        casillas = new GameObject[AnchoTablero, AltoTablero];

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
                    casillas[i, j] = Instantiate(suelo, new Vector3(i, j, 0), Quaternion.identity);
                    Instantiate(casa, new Vector3(i, j, 0), Quaternion.identity);

                    casillas[i, j].GetComponent<Casilla>().setTipoCasilla(2);
                }
                else
                {
                    tablero[i, j] = Instantiate(suelo, new Vector3(i, j, 0), Quaternion.identity);

                    casillas[i, j] = tablero[i, j];
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
            heroeEscena = tablero[posHeroeX, posHeroeY];
            //Llamar al setPosHeroe :3

            //Heroe a true, quitamos el texto y habilitamos el botón
            textoHeroe.enabled = false;

            botonTurno.GetComponent<Image>().color = Color.white;
            botonTurno.colors = colorBoton;

            heroeSituado = true;
        }

        //Cuando el héroe ya está colocado y se pulsa en una casilla que no sea héroe ni casa, esta rotará entre suelo, aliado y zombie
        else if (objetoCasilla.GetComponent<Casilla>().posMatriz.x != posHeroeX && objetoCasilla.GetComponent<Casilla>().posMatriz.y != posHeroeY
            && objetoCasilla.GetComponent<Casilla>().posMatriz.x != posCasaX && objetoCasilla.GetComponent<Casilla>().posMatriz.y != posCasaY && !final)
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
        if (heroeSituado && !final)
        {
           //Hacemos un barrido de todas las posiciones de los actores de juego
            int contBuenos = 0;
            List <Vector2Int> Buenos = new List<Vector2Int>();
            List<Vector2Int> posMalos= new List<Vector2Int>();

            List<GameObject> listaMalos = new List<GameObject>();
            int contMalos = 0;
            //PRIMER PASO: ENCONTRAMOS TODA LA INFORMACIÓN
            for (int i = 0; i < AnchoTablero; i++)
            {
                for (int j = 0; j < AltoTablero; j++)
                {
                    //Guardo las posiciones, sin importar que sea aliado o heroe
                    if (tablero[i, j].tag == "Aliados" )
                    {
                        Buenos.Add (new Vector2Int(i, j)); 
                        contBuenos++;
                    }

                    else if (tablero[i, j].tag == "Enemigos")
                    {

                        posMalos.Add(new Vector2Int(i, j));
                        listaMalos.Add (tablero[i, j]);
                        contMalos++;
                        tablero[i, j].GetComponent<IAEnemigos>().setPosicionEnemigo(i, j);
                        
                    }

                    else if (tablero[i, j].tag == "Heroe")
                    {
                        posHeroeX = i;
                        posHeroeY = j;
                    }
                }
            }//for

         
            //TURNO DE LOS ENEMIGOS

            foreach (GameObject enemigo in listaMalos)
            {
                if (enemigo == null) continue;

                Debug.Log(listaMalos.Count);

                Vector2Int posEnemigo = enemigo.GetComponent<IAEnemigos>().getPos();
                Vector2Int posObjetivo = enemigo.GetComponent<IAEnemigos>().buscaCercano(Buenos, new Vector2Int (posHeroeX, posHeroeY));

                if (posEnemigo.y != posObjetivo.y)
                {
                    if (posEnemigo.y < posObjetivo.y)
                    {
                        enemigo.gameObject.transform.position = new Vector3(posEnemigo.x, posEnemigo.y + 1, 0);

                        tablero[posEnemigo.x, posEnemigo.y] = Instantiate(suelo, new Vector3(posEnemigo.x, posEnemigo.y, 0), Quaternion.identity);
                        tablero[posEnemigo.x, posEnemigo.y].GetComponent<Casilla>().setTipoCasilla(0);

                        enemigo.GetComponent<IAEnemigos>().setPosicionEnemigo(posEnemigo.x, posEnemigo.y + 1);
                    }

                    else
                    {
                        enemigo.gameObject.transform.position = new Vector3(posEnemigo.x, posEnemigo.y - 1, 0);

                        tablero[posEnemigo.x, posEnemigo.y] = Instantiate(suelo, new Vector3(posEnemigo.x, posEnemigo.y, 0), Quaternion.identity);
                        tablero[posEnemigo.x, posEnemigo.y].GetComponent<Casilla>().setTipoCasilla(0);

                        enemigo.GetComponent<IAEnemigos>().setPosicionEnemigo(posEnemigo.x, posEnemigo.y - 1);
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

                //PELEA
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
                        //listaMalos.Remove(enemigo);
                        Destroy(enemigo);

                        if (tablero[movimiento.x, movimiento.y].tag == "Heroe") puntuacion += 5;
                        else puntuacion++;
                    }

                    else
                    {

                        if (tablero[movimiento.x, movimiento.y].tag == "Aliados")
                        {
                            int i = 0;
                            while (Buenos[i].x != movimiento.x && Buenos[i].y != movimiento.y && i < Buenos.Count)
                            {

                                i++;
                            }

                            Buenos[i] = new Vector2Int(100, 100);
                            Destroy(tablero[movimiento.x, movimiento.y]);

                            casillas[movimiento.x, movimiento.y].GetComponent<Casilla>().setTipoCasilla(4);

                            tablero[movimiento.x, movimiento.y] = enemigo;

                            puntuacion -= 10;

                        }

                        //Hay que mostrar algo por pantalla si muere el héroe (sin necesidad de modificar los parámetros)
                        else if (tablero[movimiento.x, movimiento.y].tag == "Heroe")
                        {
                            puntuacion -= 50;

                            Destroy(heroeEscena);

                            gameOver.enabled = true;
                            botonNuevaPartida.gameObject.SetActive(true);
                            botonNuevaPartida.enabled = true;

                            final = true;

                        }

                      
                    }
                }

                else
                {
                    tablero[movimiento.x, movimiento.y] = enemigo;
                    casillas[movimiento.x, movimiento.y].GetComponent<Casilla>().setTipoCasilla(4);
                }

            }

            //TURNO DEL HEROE
            //primero analizamos la situacion: numero de enemigos, numero de aliados y la luz
            //Y en base a eso toma un movimiento.

            int riesgo = 0;

            if (contMalos == 0) riesgo += 0;
            else if (contMalos >= 1 && contMalos < 5) riesgo += 1;
            else if (contMalos >= 5) riesgo += 2;

            if (contBuenos == 0) riesgo += 2;
            else if (contBuenos == 1) riesgo += 1;
            else if (contMalos > 1) riesgo += 0;

            if (dia) riesgo += 0;   //Día
            else riesgo += 1;       //Noche

            //AHORA INTERPRETAMOS RESULTADOS
            if (riesgo < 2)
            {
                Vector2Int objetivoHeroe = heroeEscena.GetComponent<IAHeroe>().ataca(posMalos);
                calculaMovimientoGameObject(objetivoHeroe); //Ahora, se habrá movido pero NO ha rsuelto conflictos
                
            }
            //Irse a casa
            else if (riesgo >= 4)
            {
                calculaMovimientoGameObject(new Vector2Int(posCasaX, posCasaY));
            }

            //PELEA DEL HEROE: LUCHA EN MODO ACTIVO O VOLVIENDO A CASA
            //Si están en la misma casilla pelean
            if (tablero[posHeroeX, posHeroeY].tag == "Enemigos")
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
                    Destroy(tablero[posHeroeX, posHeroeY]);
                    puntuacion += 5;

                    tablero[posHeroeX, posHeroeY] = heroeEscena;
                    casillas[posHeroeX, posHeroeY].GetComponent<Casilla>().setTipoCasilla(1);

                }

                else
                {
                    //Te mueres.
                    puntuacion -= 50;
                    Destroy(heroeEscena);

                    gameOver.enabled = true;
                    botonNuevaPartida.gameObject.SetActive(true);
                    botonNuevaPartida.enabled = true;

                    final = true;

                }
            }

            else
            {
                tablero[posHeroeX, posHeroeY] = heroeEscena;
                casillas[posHeroeX, posHeroeY].GetComponent<Casilla>().setTipoCasilla(1);
            }


            //FINALMENTE, BORRAS TODO PARA LIMPIAR
            Buenos.Clear();
            listaMalos.Clear();
        }   
    }

    public void callbackReinicio()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void calculaMovimientoGameObject(Vector2Int objetivoHeroe)
    {
       
        if (posHeroeY != objetivoHeroe.y)
        {
            if (posHeroeY < objetivoHeroe.y)
            {
                heroeEscena.gameObject.transform.position = new Vector3(posHeroeX, posHeroeY + 1, 0);
                posHeroeY += 1;

                tablero[posHeroeX, posHeroeY] = Instantiate(suelo, new Vector3(posHeroeX, posHeroeY, 0), Quaternion.identity);
                tablero[posHeroeX, posHeroeY].GetComponent<Casilla>().setTipoCasilla(0);


            }

            else
            {
                heroeEscena.gameObject.transform.position = new Vector3(posHeroeX, posHeroeY - 1, 0);
                posHeroeY -= 1;

                tablero[posHeroeX, posHeroeY] = Instantiate(suelo, new Vector3(posHeroeX, posHeroeY, 0), Quaternion.identity);
                tablero[posHeroeX, posHeroeY].GetComponent<Casilla>().setTipoCasilla(0);


            }

        }

        else
        {
            if (posHeroeX < objetivoHeroe.x)
            {
                heroeEscena.gameObject.transform.position = new Vector3(posHeroeX + 1, posHeroeY, 0);
                posHeroeX += 1;

                tablero[posHeroeX, posHeroeY] = Instantiate(suelo, new Vector3(posHeroeX, posHeroeY, 0), Quaternion.identity);
                tablero[posHeroeX, posHeroeY].GetComponent<Casilla>().setTipoCasilla(0);


            }

            else
            {
                heroeEscena.gameObject.transform.position = new Vector3(posHeroeX - 1, posHeroeY, 0);
                posHeroeX -= 1;

                tablero[posHeroeX, posHeroeY] = Instantiate(suelo, new Vector3(posHeroeX, posHeroeY, 0), Quaternion.identity);
                tablero[posHeroeX, posHeroeY].GetComponent<Casilla>().setTipoCasilla(0);


            }
        }

    }
}
