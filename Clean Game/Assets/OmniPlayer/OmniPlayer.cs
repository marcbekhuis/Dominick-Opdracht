using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class OmniPlayer : MonoBehaviour //Omni betekend allesomvattend :P
{
    //Variables
    public AudioSource _gameOver;
    public KeyCode _reset = KeyCode.R;
    public float _speed;
    public KeyCode d = KeyCode.S;
    public float _attackTime = 0.2f;
    public AudioSource _damS;
    public int _invSize;
    public string[] _inventory;
    public Rigidbody _rigidbody;
    public KeyCode _interactKey = KeyCode.I;//This is the key you need to press in order to try to interact with objects in the scene
    private float _timestamp = 0.0f;
    public AudioSource _jumping;
    public KeyCode _throwKey = KeyCode.T;//This is the key you need to press in order to throw a certain object, but only when you are holding an object
    public KeyCode _next = KeyCode.Return;
    public Text _ph;
    public int _health;//Keeps track of the remaining health of the player, when this reaches 0 the player will die
    public GameObject _chat1;
    public AudioSource _treasureChest;
    public KeyCode l = KeyCode.A;
    public GameObject _chat2;
    public Text _chat2T;
    private bool _showAttack = false;
    public GameObject _sphere;//A nice sphere
    public Text _inv;
    public KeyCode r = KeyCode.D;
    private Transform _crate = null;
    public float _jump;//jumpForce
    private int _counter = 0;//A variable to keep track of how many counts there have been
    public AudioSource _swoosh;//Awesome swoosh sound effect
    public KeyCode u = KeyCode.W;
    public AudioSource _pickUpS;
    public KeyCode _jumpButton = KeyCode.Space;
    public AudioSource _throw;
    public bool _talking = false;
    public AudioSource _resetS;
    public KeyCode _attack = KeyCode.LeftShift;
    public Text _chat1T;
    private bool _crateLift = false;
    
    //Awake is called once
    void Awake()
    {
        _rigidbody.useGravity = true;
        _invSize = _inventory.Length;
    }
    //Update is called multiple times
    void Update()
    {
        //if (!_talking)
        //{
        //    Vector3 movement = Vector3.zero;
        //    if (Input.GetKey(d))
        //    {
        //        movement = new Vector3(0, 0, -_speed);
        //    }
        //    else if (Input.GetKey(u))
        //    {
        //        movement = new Vector3(0, 0, _speed);
        //    }
        //    else if (Input.GetKey(l))
        //    {
        //        movement = new Vector3(-_speed, 0, 0);
        //    }
        //    else if (Input.GetKey(r))
        //    {
        //        movement = new Vector3(_speed, 0, 0);
        //    }
        //    transform.Translate(movement);
        //}
        if (!_crateLift && !_talking && Input.GetKeyDown((_interactKey)))
        {
            LayerMask mask = LayerMask.GetMask("Interactable");
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f, mask);
            if (colliders.Length > 0)
            {
                Collider colliderHit = colliders[0];
                if (colliderHit.tag == "NPC")//This block of code gets activated if the tag of the collider is equal to "NPC"
                {
                    _talking = true;
                    _counter = 0;
                    _chat1.SetActive(true);
                    _chat1T.text = "Hey what are you doing?";
                }
                //else if (colliderHit.tag == "TreasureChest")
                //{
                //    _treasureChest.Play();
                //    int numberOfItems = Random.Range(1, 4);
                //    string[] loot = new string[numberOfItems];
                //    for (int i = 0; i < numberOfItems; i++)
                //    {
                //        int randomLoot = Random.Range(0, 7);
                //        switch (randomLoot)
                //        {
                //            case 0:
                //                loot[i] = "Dagger";
                //                break;
                //            case 1:
                //                loot[i] = "Shield";
                //                break;
                //            case 2:
                //                loot[i] = "Helmet";
                //                break;
                //            case 3:
                //                loot[i] = "ChestPlate";
                //                break;
                //            case 4:
                //                loot[i] = "LegPlates";
                //                break;
                //            case 5:
                //                loot[i] = "Shoes";
                //                break;
                //            case 6:
                //                loot[i] = "2H sword";
                //                break;
                //        }
                //    }
                //    for (int i = 0; i < loot.Length; i++)
                //    {
                //        for (int j = 0; j < _invSize; j++)
                //        {
                //            if (_inventory[j] == "")
                //            {
                //                _inventory[j] = loot[i];
                //                break;
                //            }
                //        }
                //    }
                //    Destroy(colliderHit.gameObject);
                //}
                else if (colliderHit.tag == "Crate")
                {
                    _pickUpS.Play();
                    _crate = colliderHit.transform;
                    _crate.SetParent(transform);
                    _crate.localPosition = Vector3.up * 0.5f;
                    _crateLift = true;
                }
            }
        }
        string inventoryText = "Inventory: " + "\n";
        if (_crateLift && Input.GetKeyDown(_throwKey))
        {
            _throw.Play();
            _crate.SetParent(null);
            Rigidbody crateRB;
            crateRB = _crate.GetComponent<Rigidbody>();
            if (crateRB == null)
            {
                crateRB = _crate.gameObject.AddComponent<Rigidbody>();
            }
            crateRB.AddForce((transform.forward + Vector3.up) * 10, ForceMode.VelocityChange);
            _crateLift = false;
            _crate = null;
        }
        for (int i = 0; i < _inventory.Length; i++)
        {
            inventoryText += _inventory[i] + "\n";
        }
        Camera.main.transform.position = transform.position + new Vector3(0, 10, -10);

        if (_health <= 0)
        {
            _gameOver.Play();
            GetComponent<MeshRenderer>().enabled = false;
            enabled = false;
            Destroy(gameObject, 2.0f);
        }
        if (!_talking && _showAttack && _timestamp <= Time.time)
        {
            _showAttack = false;
            _sphere.SetActive(false);
        }
        //_inv.text = inventoryText;
        if (!_talking && !_showAttack && Input.GetKeyDown(_attack))
        {
            _showAttack = true;
            _sphere.SetActive(true);
            _swoosh.Play();
            _timestamp = Time.time + _attackTime;
            LayerMask mask = LayerMask.GetMask("Enemy");
            Collider[] colliders = Physics.OverlapSphere(transform.position, _sphere.transform.localScale.x, mask);
            for (int i = 0; i < colliders.Length; i++)
            {
                OmniEnemy enemy = colliders[i].GetComponent<OmniEnemy>();
                enemy.enabled = false;
                Destroy(enemy.gameObject, 5.0f);

                Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                rb.AddExplosionForce(1000, transform.position - Vector3.up, _sphere.transform.localScale.x);
            }
        }
        //if (!_talking && Input.GetKeyDown(_jumpButton))
        //{
        //    GetComponent<Rigidbody>().velocity += Vector3.up * _jump;
        //    _jumping.Play();
        //}
        if (Input.GetKeyDown(_reset))
        {
            _resetS.Play();
            Invoke("RestartScene", 0.5f);  //Little delay so the sound doesn't get cutoff';
        }
        if (_talking && Input.GetKeyDown(_next))
        {
            _counter++;
            if (_counter == 1)
            {
                _chat1.SetActive(false);
                _chat2.SetActive(true);
                _chat2T.text = "Uhmmmm... Nothing really, I'm pretty bored";
            }
            else if (_counter == 2)
            {
                _chat1.SetActive(true);
                _chat2.SetActive(false);
                _chat1T.text = "Oh, thats unfortunate. Would you like to join me in a quest?";
            }
            else if (_counter == 3)
            {
                _chat1.SetActive(false);
                _chat2.SetActive(true);
                _chat2T.text = "Huh? A quest? Do you think this is some kind of RPG game?";
            }
            else if (_counter == 4)
            {
                _chat1.SetActive(true);
                _chat2.SetActive(false);
                _chat1T.text = "Uhhhh.... yeah? And I'm the main hero!";
            }
            else if (_counter == 5)
            {
                _chat1.SetActive(false);
                _chat2.SetActive(true);
                _chat2T.text = "HAHAHAHAAA ROFL OMG YOU? ARE YOU THE MAIN HERO?";
            }
            else if (_counter == 6)
            {
                _chat1.SetActive(true);
                _chat2.SetActive(false);
                _chat1T.text = "Yes I am, come on just join me in my quest!";
            }
            else if (_counter == 7)
            {
                _chat1.SetActive(false);
                _chat2.SetActive(true);
                _chat2T.text = "Noooooo thank you, i'm rather bored than joining some random noob in a quest";
            }
            else
            {
                _chat1.SetActive(false);
                _chat2.SetActive(false);
                _chat1T.text = "";
                _chat2T.text = "";
                _talking = false;
            }
        }
        _ph.text = "Health: " + _health;
    }
    private void OnCollisionEnter(Collision pOther)
    {
        if (pOther.transform.tag == "Enemy")
        {
            _health--;
            _damS.Play();
        }
    }
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}