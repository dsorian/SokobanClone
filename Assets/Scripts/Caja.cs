using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Caja : MonoBehaviour
{
    public Sprite spriteNormal,spriteColocada;
    public GameManager elGameManager;

    public bool estoyColocada=false;  //True=la caja está colocada en casilla de destino False=no lo está

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if( other.tag == "PosCaja"){
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteColocada;
            estoyColocada=true;
            elGameManager.CajaColocada();
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if( other.tag == "PosCaja"){
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteNormal;
            estoyColocada=false;
        }
    }
}
