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
    public Vertice anterior, destino;
    public bool fp = false;


    void Start(){
        SetState(State.MAPEO);
        sensor = GetComponent<Sensores>();
		actuador = GetComponent<Actuadores>();
        mapa = GetComponent<Mapa>();
        mapa.ColocarNodo(0);
        mapa.popStack(out anterior);
        mapa.setPreV(anterior);

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
        actuador.Adelante();
        if(!sensor.FrenteLibre()){
            actuador.GirarIzquierda();
        }
    }

    void UpdateDFS(){
        if(sensor.FrenteLibre()){
            mapa.ColocarNodo(2);
        }
        if(sensor.IzquierdaLibre()){
            mapa.ColocarNodo(1);
        }
        if(sensor.DerechaLibre()){
            mapa.ColocarNodo(3);
        }
        SetState(State.MAPEO);
    }

    // Función para cambiar de estado
    void SetState(State newState) {
        currentState = newState;
    }

}
