using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour, ICharacter
{    
    public float Health { get { return health; } } // Default Health
    public float CurrentHealth { get; set; }       // Current Health

    // Character settings
    [Header("Character settings")]    
    [SerializeField] private float health = 100;            // health of the character    
    [SerializeField] private float speed = 200;             // running speed of the character 
    [SerializeField] private float jump = 400;              // jumping power of the character 
    [SerializeField] private Transform footPosition;        // character's foot position
    [Header("Character's weapon inventory")]
    [SerializeField] private WeaponInventory[] _weapons;    // character weapon-inventory

    // Character-object components
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private Animator _mAnimator;
    private DirectionTypes _mDirection;
    private LayerMask _groundLayer;
    private Vector3 _mScale
    {
        get { return transform.localScale; }
        set { transform.localScale = value; }
    }

    // Character controlling
    private bool isGround, isLeftRightCollision, isDeath;
    private bool toMove, toJump;
    private float _gravity;

    // Character Weapon-Inventory
    [Serializable] private struct WeaponInventory
    {
        public static Transform parent;
        public WeaponTypes type;
        public bool isActivated;
        private Weapon _weapon;

        public void Init()
        {
            Transform _weaponPosition = null;
            switch (type)
            {
                case WeaponTypes.Blaster:
                    _weapon = new Blaster();
                    _weaponPosition = parent.Find("BlasterPosition");
                    break;
                case WeaponTypes.Pistol:
                    _weapon = new Pistol();
                    _weaponPosition = parent.Find("PistolPosition");
                    break;
            }

            _weapon.SetPosition((_weaponPosition != null) ? _weaponPosition : parent);
            _weapon.SetActive(isActivated);
        }

        public Weapon GetWeapon()
        {
            return _weapon;
        }
    }


    protected void Start()
    {
        CurrentHealth = Health;
        _mDirection = DirectionTypes.Right;
        _groundLayer = LayerMask.GetMask("Ground");
        _mAnimator = gameObject.GetComponent<Animator>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _collider = gameObject.GetComponent<Collider2D>();

        // Checking for null
        if (_rb == null || _mAnimator == null || footPosition == null || _collider == null || _groundLayer == -1)
            throw new Exception("Object: " + gameObject.name + ". Check the correctness of all the object settings!");
        else
            _rb.freezeRotation = true;

        // Weapon-inventory initialization
        WeaponInventory.parent = transform;
        if (_weapons.Length > 0)
            for(int i = 0; i < _weapons.Length; i++)
                _weapons[i].Init();
    }

    protected void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(footPosition.position, .3f, _groundLayer);        

        _mAnimator.SetBool("isRun", toMove && isGround);
        if (toMove && !(isLeftRightCollision && !isGround))
            _rb.velocity = new Vector2((float)_mDirection * speed * Time.fixedDeltaTime, _rb.velocity.y);

        if (isGround)
        {
            if (toJump)
            {
                _mAnimator.SetTrigger("Jump");
                _rb.velocity = Vector2.up * jump * Time.fixedDeltaTime;
                _gravity = jump;
            }
            else
            {
                _mAnimator.ResetTrigger("Jump");
                _mAnimator.SetBool("isFall", false);
                _gravity = -1f;
            }
        }
        else
        {
            _gravity -= jump * Time.fixedDeltaTime;

            if (_gravity < jump/2)
                _mAnimator.SetBool("isFall", true);
        }

        isLeftRightCollision = false;
        toJump = false;
        toMove = false;
    }

    // We check the lunch for the presence of lead
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("GetBlaster"))
        {
            GetWeapon(WeaponTypes.Blaster);
            Destroy(collision.gameObject);
        }            
        else if (collision.tag.Equals("GetPistol"))
        {
            GetWeapon(WeaponTypes.Pistol);
            Destroy(collision.gameObject);
        }            

        if (_weapons.Length > 0)
            for (int i = 0; i < _weapons.Length; i++)
            {
                Weapon weapon = _weapons[i].GetWeapon();
                if (weapon.NameTag.Equals(collision.tag))
                    Damage(weapon.Damage);
            }
    }    

    // Ignore collisions with their own kind
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Player"))
            Physics2D.IgnoreCollision(collision.collider, _collider);
    }

    // So that there are no problems with moving during the jump
    protected void OnCollisionStay2D(Collision2D collision)
    {
        isLeftRightCollision = true;
    }

    // Selection of weapons
    private void GetWeapon(WeaponTypes type)
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            Weapon weapon = _weapons[i].GetWeapon();
            bool active = (weapon.Type == type);
            weapon.SetActive(active);
        }
    }

    // Set 'Direction' of the Characters and Weapons
    public void Direction(DirectionTypes type)
    {      
        _mScale = new Vector3((float)type * Mathf.Abs(_mScale.x), _mScale.y, _mScale.z);
        _mDirection = type;

        if (_weapons.Length > 0)
            for (int i = 0; i < _weapons.Length; i++)
                _weapons[i].GetWeapon().Direction(type);
    }

    // Ñharacter movement
    public void Move()
    {
        toMove = true;
    }

    // Ñharacter jump
    public void Jump()
    {
        toJump = true;
    }

    // Shooting from the selected Weapon
    public void Attack(WeaponTypes type)
    {
        if (_weapons.Length > 0)
            for (int i = 0; i < _weapons.Length; i++)
            {
                Weapon weapon = _weapons[i].GetWeapon();
                if (weapon.Type == type)
                {
                    if (weapon.Attack()) _mAnimator.SetTrigger(weapon.NameAttack);
                    break;
                }
            }                  
    }

    // Shooting with some active Weapons
    public void Attack()
    {
        Weapon weapon = GetActiveWeapon();
        if (weapon != null)
            if (weapon.Attack()) _mAnimator.SetTrigger(weapon.NameAttack);
    }

    // Returns the current active Weapon
    public Weapon GetActiveWeapon()
    {
        if (_weapons.Length > 0)
            for (int i = 0; i < _weapons.Length; i++)
            {
                Weapon weapon = _weapons[i].GetWeapon();
                if (weapon.IsActive())
                    return weapon;
            }
        return null;
    }    

    // Taking damage
    private void Damage(float damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0 && !isDeath)
        {
            isDeath = true;
            Destroy(gameObject);
        }            
    }
}
