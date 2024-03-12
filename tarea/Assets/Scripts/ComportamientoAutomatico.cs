using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoAutomatico : MonoBehaviour {


    //Enum para los estados
    public enum State {
        MAPEO,
        DFS
    }

    private State currentState;
    private Sensores sensor;
	private Actuadores actuador;
	private Mapa mapa;
    public Vertice verticeActual, VerticeDestino;
    public bool fp = false, look;


    void Start(){
        SetState(State.MAPEO);
        sensor = GetComponent<Sensores>();
		actuador = GetComponent<Actuadores>();
        mapa = GetComponent<Mapa>();
        mapa.ColocarNodo(0);
        mapa.popStack(out verticeActual);
        //mapa.setPreV(anterior);

    }


    void FixedUpdate() {
        switch (currentState) {
            case State.MAPEO:
            UpdateMAPEO();
            break;
        }
    }

    // Funciones de actualizacion especificas para cada estado
    void UpdateMAPEO() {
        if(fp){
            mapa.popStack(out verticeActual);
            mapa.setPreV(verticeActual); //asignar a mapa el vertice nuevo al que nos vamos a mover, para crear las adyacencias necesarias.
            fp = false;
        }

        if(Vector3.Distance(sensor.Ubicacion(), verticeActual.posicion) >= 0.04f){
            if(!look){
                transform.LookAt(verticeActual.posicion);
                look = true;
            }

            actuador.Adelante();
            if(!sensor.FrenteLibre()){
                actuador.GirarIzquierda();
                SetState(State.DFS);
                look = false;
                fp = true;
            }

        } else {
            look = false;
            fp = true;
            SetState(State.DFS);
        }
    }

    void UpdateDFS(){
        if(!sensor.FrenteLibre()){
            actuador.Detener();
        }
        if(sensor.IzquierdaLibre()){
            mapa.ColocarNodo(1);
        }
        if(sensor.DerechaLibre()){
            mapa.ColocarNodo(3);
        }
        if(sensor.FrenteLibre()){
            mapa.ColocarNodo(2);
        }
        SetState(State.MAPEO);
    }

    // Función para cambiar de estado
    void SetState(State newState) {
        currentState = newState;
    }

}
