using UnityEngine;

public class OutletCharge : MonoBehaviour
{
    public Charges charges;
    [Range(1,4)]
    public int gainedCharges;
    private bool used;

    void Start(){
        used = false;
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            if(Input.GetKeyDown(KeyCode.E) && !used){
                increaseCharges();
            }
        }
    }

    void increaseCharges(){
        if(charges.currentCharges < charges.totalCharges){
            charges.currentCharges += gainedCharges;
            used = true;
        }
    }
}
