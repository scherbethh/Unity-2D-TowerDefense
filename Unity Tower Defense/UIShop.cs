using UnityEngine;
using TMPro;

public class UIShop : MonoBehaviour
{
    [SerializeField] private Animator toggleanim;
    [SerializeField] private Animator pauseanim;
    [SerializeField] private TMP_Text healthText;

    public bool MenuOpen = true;
    public bool Pause = false;

    private EnemyMoveAI enemyMoveAI;
    private AudioManager audioManager;

    private void Start()
    {
        enemyMoveAI = GetComponent<EnemyMoveAI>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void FixedUpdate()
    {
        if (healthText != null)
        {
            int playerHealth = EnemyMoveAI.currentHealth;
            healthText.text = playerHealth.ToString();

        }
    }

    public void ToggleMenu()
    {
        MenuOpen = !MenuOpen;
        toggleanim.SetBool("MenuOpen", MenuOpen);
    }

    public void CloseMenu()
    {
        if (MenuOpen)
        {
            ToggleMenu();
        }
    }
    public void OpenMenu()
    {
        if (!MenuOpen)
        {
            ToggleMenu();
        }
    }

    public void PauseResume()
    {
        audioManager.PlaySFX(audioManager.click);

        Pause = !Pause;
        pauseanim.SetBool("p", Pause);
        Time.timeScale = Pause ? 0f : 1f;
    }
}
