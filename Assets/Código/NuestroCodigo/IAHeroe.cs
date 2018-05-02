using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAHeroe : MonoBehaviour
{

    Vector2Int posHeroe;

    public void setPosHeroe(int x, int y)
    {
        posHeroe = new Vector2Int(x, y);
    }

    float distanciaEuclidea(Vector2Int posPropia, Vector2Int posDestino)
    {
        return Mathf.Abs(posDestino.x - posPropia.x) + Mathf.Abs(posDestino.y - posPropia.y);
    }

    public Vector2Int ataca(List<Vector2Int> listaMalos)
    {
        float distanciaMenor = distanciaEuclidea(posHeroe, listaMalos[0]);
        Vector2Int objetivo = listaMalos[0];

        foreach (Vector2Int malo in listaMalos)
        {
            float aux = distanciaEuclidea(posHeroe, malo);

            if (aux < distanciaMenor)
            {
                distanciaMenor = aux;
                objetivo = malo;
            }
        }

        return objetivo;

    }

    
}



   