using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Caja : MonoBehaviour
{
    public Sprite spriteNormal,spriteColocada;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        Debug.Log("Soy: "+this.name+" me ha colisionado: "+other.name);
        if( other.tag == "PosCaja"){
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteColocada;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        Debug.Log("Soy: "+this.name+" he salido de: "+other.name);
        if( other.tag == "PosCaja"){
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteNormal;
        }
    }
}
