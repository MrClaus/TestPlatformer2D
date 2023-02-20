// Singleton Player
public class Player : Character
{
    public static Player instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }    

    private void Update()
    {
        Weapon weapon = GetActiveWeapon();
        if (weapon != null && UIService.instance != null)
            UIService.instance.PlayerWeapon(weapon.Type);
    }

    private void OnDestroy()
    {
        if (UIService.instance != null)
            if (!UIService.isRestarted)
                UIService.instance.PlayerDeath();
    }
}
