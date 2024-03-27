using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameEventManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private GameObject _failedPanel;
    [SerializeField] private GameObject _successPanel;
    [SerializeField] private float _canvasFadeTime = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioClip _caughtMusic;
    [SerializeField] private AudioClip _successMusic;
    

    private PlayerInput _playerInput;
    private FirstPersonController _fpController;
    private bool _isFadingIn = false;
    private float _fadeLevel = 0f;
    private bool _isGoalReached = false;
    
    // Start is called before the first frame update
    void Start()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy.onInvestigate.AddListener(EnemyInvestigating);
            enemy.onPlayerFound.AddListener(PlayerFound);
            enemy.onReturnToPatrol.AddListener(EnemyReturnToPatrol);
        }

        GameObject player = GameObject.FindWithTag("Player");
        if(player){
            _playerInput = player.GetComponent<PlayerInput>();
            _fpController = player.GetComponent<FirstPersonController>();
        }
        else
        {
            Debug.LogWarning("There is no player in the scene");
        }

        _canvasGroup.alpha = 0;
        _failedPanel.SetActive(false);
        _successPanel.SetActive(false);
    }

    private void EnemyReturnToPatrol()
    {
        
    }

    private void PlayerFound(Transform enemyThatFoundPlayer)
    {
        if (_isGoalReached) return;
        
        _isFadingIn = true;
        
        _failedPanel.SetActive(true);
        
        _fpController.CinemachineCameraTarget.transform.LookAt(enemyThatFoundPlayer);
        
        DeactivateInput();
        PlayBGM(_caughtMusic);
    }

    public void GoalReached()
    {
        _isFadingIn = true;
        _isGoalReached = true;
        
        _successPanel.SetActive(true);
        
        DeactivateInput();
        PlayBGM(_successMusic);
    }

    private void DeactivateInput()
    {
        _playerInput.DeactivateInput();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void PlayBGM(AudioClip newBgm)
    {
        if (_bgmSource.clip == newBgm) return;
        
        _bgmSource.clip = newBgm;
        _bgmSource.Play();
    }

    private void EnemyInvestigating()
    {
        
    }

    public void RestartScene()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFadingIn)
        {
            if (_fadeLevel < 1f)
            {
                _fadeLevel += Time.deltaTime / _canvasFadeTime;
            }
        }
        else
        {
            if (_fadeLevel > 0f)
            {
                _fadeLevel -= Time.deltaTime / _canvasFadeTime;
            }
        }
        _canvasGroup.alpha = _fadeLevel;
    }
}
