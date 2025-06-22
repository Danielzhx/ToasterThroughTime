using UnityEngine;
using TTT.System;

namespace TTT
{
    public class OutletCharge : MonoBehaviour
    {
        [SerializeField] private Sprite brokenOutletSprite;
        private bool used;
        private SpriteRenderer outletSprite;
        private Animator _anim;
        private Transform _zapTransform;
        private SpriteRenderer _zapSprite;
        private SpriteRenderer _electricityIcon;

        public Charges charges;
        [Range(1, 4)]
        public int gainedCharges;
        public GameObject Zap;
        public Transform RightChargePoint;
        public Transform LeftChargePoint;

        void Start()
        {
            used = false;
            outletSprite = GetComponent<SpriteRenderer>();
            _zapTransform = Zap.GetComponent<Transform>();
            _zapSprite = Zap.GetComponent<SpriteRenderer>();
            _anim = Zap.GetComponent<Animator>();
            // DO NOT TOUCH OR EVEN LOOK AT THIS NEXT LINE!!!!!
            _electricityIcon = this.gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (Input.GetKey(KeyCode.E) && !used)
                {
                    AudioManager.instance.PlayCharacterChargingSound();
                    IncreaseCharges();
                }
            }
        }

        void IncreaseCharges()
        {
            if (charges.currentCharges < charges.totalCharges)
            {
                // Determine the position to place the Zap effect based on player's facing direction
                if (_zapSprite.flipX)
                {
                    _zapTransform.position = RightChargePoint.position;
                }
                else
                {
                    _zapTransform.position = LeftChargePoint.position;
                }

                _anim.SetTrigger("Charge");

                // Add charges using the Charges class method
                charges.AddCharges(gainedCharges);

                used = true;
                outletSprite.sprite = brokenOutletSprite;
                _electricityIcon.color = new Color(0.25f, 0.25f, 0.25f, 1);
            }
            else
            {
                Debug.Log("Player already has maximum charges.");
            }
        }
    }
}

