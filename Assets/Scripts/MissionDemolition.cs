using UnityEngine;
using UnityEngine.UI;

public class MissionDemolition : MonoBehaviour
{
    private static MissionDemolition singletoneMissionDemolition; // скрытый объект-одиночка

    [Header("Для редактирования")]
    [SerializeField] private Text _uiCurrentLevelText; // Ссылка на объект UIText_Level
    [SerializeField] private Text _uiShotsText;
    [SerializeField] private Text _uiButtonText;
    [SerializeField] private Vector3 _castlePosition;
    [SerializeField] private GameObject[] _castles;
    
    private int _currentLevel; // Текущий уровень
    private int _levelMaxCount;
    private int _shotsTaken;
    private GameObject _currentCastle; // Текущий замок
    private GameMode _gameMode = GameMode.idle;
    private string _showingMode = "Show Slingshot"; // Режим FollowCam

    private void Start()
    {
        singletoneMissionDemolition = this;

        _currentLevel = 0;
        _levelMaxCount = _castles.Length;
        StartLevel();
    }

    private void StartLevel()
    {
        // Уничтожить прежний замок, если он существует
        if (_currentCastle != null)
        {
            Destroy(_currentCastle);
        }

        // Уничтожить прежние снаряды, если они существуют
        var gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        // Создать новый замок
        _currentCastle = Instantiate<GameObject>(_castles[_currentLevel]);
        _currentCastle.transform.position = _castlePosition;
        _shotsTaken = 0;

        // Переустановить камеру в начальную позицию
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        // Сбросить цель
        Goal.goalMet = false;

        UpdateGUI();

        _gameMode = GameMode.playing;
    }

    private void UpdateGUI()
    {
        // Показать данные в элементах ПИ
        _uiCurrentLevelText.text = $"Level: {_currentLevel+1} of {_levelMaxCount}";
        _uiShotsText.text = $"Shots Taken: {_shotsTaken}";
    }

    private void Update()
    {
        UpdateGUI();

        // Проверить завершение уровня
        if ((_gameMode == GameMode.playing) && Goal.goalMet)
        {
            // Изменить режим, чтобы прекратить проверку завершения уровня
            _gameMode = GameMode.levelEnd;
            // Уменьшить масштаб
            SwitchView("Show Both");
            // Начать новый уровень через 2 секунды
            Invoke("NextLevel", 2f);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void NextLevel()
    {
        _currentLevel++;
        if (_currentLevel== _levelMaxCount)
        {
            _currentLevel = 0;
        }
        StartLevel();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = _uiButtonText.text;
        }
        _showingMode = eView;
        switch(_showingMode)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                _uiButtonText.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = singletoneMissionDemolition._currentCastle;
                _uiButtonText.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                _uiButtonText.text = "Show Slingshot";
                break;
        }
    }

    // Статический метод, позволяющий из любого кода увеличить shotsTaken
    public static void ShotFired()
    {
        singletoneMissionDemolition._shotsTaken++;
    }
}
