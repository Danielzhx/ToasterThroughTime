using UnityEngine;
using TarodevController;

public class OutletCharge : MonoBehaviour
{
    private bool used;
    private SpriteRenderer outletSprite;
    [SerializeField] private Sprite brokenOutleltSprite;
    private Animator _anim;
    private Transform _zapTransform;
    private SpriteRenderer _zapSprite;

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
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E) && !used)
            {
                if (_zapSprite.flipX)
                {
                    _zapTransform.position = RightChargePoint.position;
                }
                else
                {
                    _zapTransform.position = LeftChargePoint.position;
                }
                _anim.SetTrigger("Charge");
                increaseCharges();
            }
        }
    }

    void increaseCharges()
    {
        if (charges.currentCharges < charges.totalCharges)
        {
            charges.currentCharges += gainedCharges;
            used = true;
            outletSprite.sprite = brokenOutleltSprite;
        }
    }
}
