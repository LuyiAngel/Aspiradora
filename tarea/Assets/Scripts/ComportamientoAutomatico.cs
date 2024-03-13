using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoAutomatico : MonoBehaviour {

    // Enum para los estados
    public enum State {
        MAPEO,
        DFS
    }

    private State currentState;
    private Sensores sensor;
    private Actuadores actuador;
    private Mapa mapa;
    public Vertice verticeActual;
    private bool fp = false, look;

    void Start(){
        SetState(State.DFS);
        sensor = GetComponent<Sensores>();
        actuador = GetComponent<Actuadores>();
        mapa = GetComponent<Mapa>();
        // Iniciar el mapeo colocando un nodo
        mapa.ColocarNodo(0);
        mapa.popStack(out verticeActual);
    }

    void FixedUpdate() {
        switch (currentState) {
            case State.MAPEO:
                UpdateMAPEO();
                break;
            case State.DFS:
                UpdateDFS();
                break;
        }
    }

    // Función de actualización para el estado de mapeo
    void UpdateMAPEO() {
        if(fp){
            // Si ya se alcanzó el vértice actual, obtener el siguiente de la pila
            mapa.popStack(out verticeActual);
            mapa.setPreV(verticeActual); // Asignar a mapa el vertice nuevo al que nos vamos a mover
            fp = false;
        }

        // Avanzar hacia el vértice actual
        if(Vector3.Distance(sensor.Ubicacion(), verticeActual.posicion) >= 0.04f){
            if(!look){
                // Girar para enfrentar el vértice actual
                transform.LookAt(verticeActual.posicion);
                look = true;
            }
            // Avanzar hacia el vértice actual
            actuador.Adelante();
            // Si hay una pared frente a la aspiradora, cambiar al estado DFS
            if(!sensor.FrenteLibre()){
                actuador.GirarIzquierda();
                SetState(State.DFS);
                look = false;
                fp = true;
            }
        } else {
            // Si se alcanza el vértice actual, cambiar al estado DFS
            look = false;
            fp = true;
            SetState(State.DFS);
        }
    }

    // Función de actualización para el estado DFS
    void UpdateDFS(){
        // Detener la aspiradora si hay una pared frente a ella
        if(!sensor.FrenteLibre()){
            actuador.Detener();
        }
        // Colocar un nodo en la dirección izquierda si está libre
        if (sensor.IzquierdaLibre()) {
            mapa.ColocarNodo(1);
        }
        // Colocar un nodo en la dirección derecha si está libre
        if (sensor.DerechaLibre()) {
            mapa.ColocarNodo(3);
        }
        // Colocar un nodo en la dirección frontal si está libre
        if(sensor.FrenteLibre()){
            mapa.ColocarNodo(2);
        }
        // Cambiar al estado de mapeo
        SetState(State.MAPEO);
    }

    // Función para cambiar de estado
    void SetState(State newState) {
        currentState = newState;
    }
}
