using UnityEngine;

namespace TTT.System
{
    public class Level : MonoBehaviour
    {
        public Animator levelEndAnimator;
        public TarodevController.PlayerController playerControlls;

        void OnTriggerEnter2D(Collider2D other){
            if(other.gameObject.CompareTag("Player")){
                levelEndAnimator.SetTrigger("LevelFinished");
                playerControlls.enabled = false;
            }
        }
    }
}

