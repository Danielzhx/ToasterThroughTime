using UnityEngine;

public class OutletCharge : MonoBehaviour
{
    private bool used;
    private SpriteRenderer outletSprite;
    [SerializeField] private Sprite brokenOutleltSprite;
    private Animator _anim;
    private Transform _zapTransform;
    private SpriteRenderer _zapSprite;
    private SpriteRenderer _elctricityIcon;

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
        _elctricityIcon = this.gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E) && !used)
            {
                increaseCharges();
            }
        }
    }

    void increaseCharges()
    {
        if (charges.currentCharges < charges.totalCharges)
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
            charges.currentCharges += gainedCharges;
            used = true;
            outletSprite.sprite = brokenOutleltSprite;
            _elctricityIcon.color = new Color(0.25f, 0.25f, 0.25f, 1);
        }
    }
}
