using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Singleton UIService
public class UIService : MonoBehaviour
{
    public static UIService instance = null;
    public static bool isRestarted;

    [Header("The maximum possible number of enemies")]
    [SerializeField] private int _maxNumberEnemy = 5;
    [SerializeField] private Text _txtCountEnemy;
    [Header("Object with icons of Weapon")]
    [SerializeField] private Transform _currentWeapon;
    [Header("Interactive panel")]
    [SerializeField] private GameObject _panel;
    [Header("Text - time passed")]
    [SerializeField] private Text _timePassed;
    [Header("Game Audio")]
    [SerializeField] private GameObject _gameAudio;

    private PlayerController _playerController;
    private int _currentCountEnemy, _currentNumberEnemy;
    private float _mTime;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            _currentNumberEnemy = UnityEngine.Random.Range(1, _maxNumberEnemy + 1);
        }            
        else
            Destroy(gameObject);        
    }

    void Start()
    {
        if (Player.instance == null) throw new Exception("Please add the Player-object to the scene!");

        _playerController = Player.instance.GetComponent<PlayerController>();        
        _txtCountEnemy.text = _currentCountEnemy + " x " + _currentNumberEnemy;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Exit();

        if (_playerController == null && Player.instance != null)
            _playerController = Player.instance.GetComponent<PlayerController>();

        _mTime += Time.deltaTime;
    }

    // Returns mac count enemyes
    public int CountEnemy()
    {
        return _currentNumberEnemy;
    }

    // Called when the Enemy dies
    public void EnemyDeath()
    {
        _currentCountEnemy++;
        _txtCountEnemy.text = _currentCountEnemy + " x " + _currentNumberEnemy;

        if (_currentCountEnemy >= _currentNumberEnemy)
            OpenPanel(true);
    }

    // Called when the Player dies
    public void PlayerDeath()
    {
        OpenPanel(false);
    }

    // Shows the Player's current weapon
    public void PlayerWeapon(WeaponTypes type)
    {
        for (int i = 0; i < _currentWeapon.childCount; i++)
            _currentWeapon.GetChild(i).gameObject.SetActive(false);

        switch (type)
        {
            case WeaponTypes.Blaster:
                _currentWeapon.Find("BlasterIcon").gameObject.SetActive(true);
                break;
            case WeaponTypes.Pistol:
                _currentWeapon.Find("PistolIcon").gameObject.SetActive(true);
                break;
        }
    }

    // Restart Game
    public void Restart()
    {
        isRestarted = true;

        if (Player.instance != null)
            Destroy(Player.instance.gameObject);

        _currentNumberEnemy = UnityEngine.Random.Range(1, _maxNumberEnemy + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);        
        ClosePanel();
    }

    // Exit Game
    public void Exit()
    {
        Application.Quit();
    }

    private void OpenPanel(bool isWin)
    {
        _playerController.enabled = false;
        _panel.SetActive(true);

        string text = (isWin) ? "PanelTextWin" : "PanelTextLoose";
        _panel.transform.Find(text).gameObject.SetActive(true);

        _gameAudio.SetActive(false);

        int minut = (int)(_mTime / 60);
        int secund = (int)(_mTime - 60 * minut);
        _timePassed.text = "Elapsed time - " + minut + "." + secund;
        _timePassed.gameObject.SetActive(isWin);
    }

    private void ClosePanel()
    {
        _panel.transform.Find("PanelTextLoose").gameObject.SetActive(false);
        _panel.transform.Find("PanelTextWin").gameObject.SetActive(false);
        _txtCountEnemy.text = "0 x " + _currentNumberEnemy;
        _currentCountEnemy = 0;
        _panel.SetActive(false);
        _gameAudio.SetActive(true);
        _mTime = 0;
    }
}
