using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MainObject
{
    [SerializeField]
    List<AudioClip> _audio = new List<AudioClip>();

    AudioSource _source;

    private List<Vector2> _asteroidPosition = new List<Vector2>();
    private List<Vector2> _ufoPosition = new List<Vector2>();

    private Vector2 _spawnPoint;

    private Transform _target;

    private Text _scoreText;
    private Text _xpText;

    private GameObject _menu;
    private GameObject _menuContinue;

    private int _score;
    private int _xp;
    private int _asteroidDestroyCount;    
    private int _asteroidOnScene;
    private int _asteroidToDestroy;

    private float _speed;
    
    private bool _isPause = true;    
    private bool _isStart = false;
    public bool _isButtonControl = true;

    void Start()
    {
        _xp = 3;
        _xpText = GameObject.FindGameObjectWithTag("Xp").gameObject.GetComponent<Text>();
        _xpText.text = "XP: " + _xp.ToString();
        _source = GetComponent<AudioSource>();
        _asteroidOnScene = 2;
        _asteroidToDestroy = 8;
        _objectPool = ObjectPool.SharedInstance;
        Time.timeScale = 0;
        _menu = GameObject.FindGameObjectWithTag("Menu");
        _menuContinue = _menu.transform.GetChild(0).gameObject;
        _menuContinue.GetComponent<Button>().interactable = false;
        _scoreText = GameObject.FindGameObjectWithTag("Score").gameObject.GetComponent<Text>();
        _scoreText.text = "SCORE: " + _score.ToString();
        RandomPosition();
    }
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (_asteroidToDestroy <= _asteroidDestroyCount)
        {
            _asteroidToDestroy += 4;
            _asteroidDestroyCount = 0;
            _asteroidOnScene += 1;
            StartCoroutine(SpawnAsteroid(_asteroidOnScene));
        }
    }
    public override void SpawnObjects(int _index, Transform _transform)
    {
        _spawnPoint = _asteroidPosition[Random.Range(0, _asteroidPosition.Count)];
        GameObject Asteroid = _objectPool.GetPooledObject(_index);
        Asteroid.transform.position = _spawnPoint;
        Vector3 dir = _transform.transform.position - Asteroid.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Asteroid.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Asteroid.GetComponent<AsteroidControl>().Speed(_speed);
        Asteroid.SetActive(true);
        _asteroidPosition.Clear();
        RandomPosition();
    }
    private void RandomPosition()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 top = new Vector2(max.x, Random.Range(min.y, max.y));
        Vector2 bottom = new Vector2(min.x, Random.Range(min.y, max.y));
        Vector2 left = new Vector2(Random.Range(min.x, max.x), min.y);
        Vector2 right = new Vector2(Random.Range(min.x, max.x), max.y);
        _asteroidPosition.Add(top);
        _asteroidPosition.Add(bottom);
        _asteroidPosition.Add(left);
        _asteroidPosition.Add(right);
    }
    public void ChangeScore(int count)
    {
        _score = _score + count;
        _scoreText.text = "SCORE: " + _score.ToString();

    }
    public void StartGame()
    {
        if (!_isStart)
        {
            SpawnPlayer();
            AsteroidSpeed();
            StartCoroutine(SpawnAsteroid(_asteroidOnScene));
            StartCoroutine("SpawnUfo");
            _isStart = true;
            _menuContinue.GetComponent<Button>().interactable = true;
            PauseGame();
        }
        else if (_isStart)
        {
            RestartGame();           
            PauseGame();
        }
    }
    public void PauseGame()
    {
        if (!_isPause)
        {
            _menu.gameObject.SetActive(true);
            _isPause = true;
            Time.timeScale = 0;
        }
        else
        {
            _menu.gameObject.SetActive(false);
            _isPause = false;
            Time.timeScale = 1;
            Control();
        }
    }
    private void RestartGame()
    {
        var _poolObjects = _objectPool.itemsToPool.Count;
        for (var i = 0; i < _poolObjects; i++)
        {
            var _poolObjects2 = _objectPool.GetAllPooledObjects(i);
            foreach (GameObject obj in _poolObjects2)
            {
                obj.SetActive(false);
            }
        }
        _score = 0;
        _scoreText.text = "SCORE: " + _score.ToString();
        SpawnPlayer();
        ShipControl.FindObjectOfType<ShipControl>().StartCoroutine("Blink");
        StartCoroutine(SpawnAsteroid(2));
        
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void SpawnPlayer()
    {
        GameObject Object = _objectPool.GetPooledObject(5);
        Object.transform.position = new Vector3(0, 0, 0);
        Object.SetActive(true);
    }
    public void ButtonControl()
    {
        _isButtonControl = true;
        
    }
    public void MouseControl()
    {
        _isButtonControl = false;
       
    }
    private void Control()
    {
        if (_isButtonControl)
        {
            ShipControl.FindObjectOfType<ShipControl>().control = ShipControl.Control.ButtonControl;
        }
        else
        {
            ShipControl.FindObjectOfType<ShipControl>().control = ShipControl.Control.MouseControl;
        }
    }
    public void Kill(int count)
    {
        _asteroidDestroyCount = _asteroidDestroyCount + count;
    }
    public void AsteroidSpeed()
    {
        _speed = Random.Range(1, 4);
    }
    IEnumerator SpawnUfo()
    {
        yield return new WaitForSeconds(20f);
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        GameObject Ufo = _objectPool.GetPooledObject(4);
        Vector2 left = new Vector2(min.x, Random.Range(min.y - min.y / 100f * 20f, max.y + min.y / 100f * 20f));
        Vector2 right = new Vector2(max.x, Random.Range(min.y - min.y / 100f * 20f, max.y + min.y / 100f * 20f));
        _ufoPosition.Add(left);
        _ufoPosition.Add(right);
        var _ufoPoint = _ufoPosition[Random.Range(0, _ufoPosition.Count)];
        Ufo.transform.position = _ufoPoint;        
        Ufo.SetActive(true);
        if (Ufo.gameObject.transform.position.x == left.x)
        {
            UfoControl.FindObjectOfType<UfoControl>().ChangeAxis(1);            
        }
        else if (Ufo.gameObject.transform.position.x == right.x)
        {
            UfoControl.FindObjectOfType<UfoControl>().ChangeAxis(-1);            
        }
        _ufoPosition.Clear();

    }
    IEnumerator SpawnAsteroid(int count)
    {
        yield return new WaitForSeconds(2f);
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        var i = 0;
        
        while (true)
        {
            i++;
            SpawnObjects(1, _target);
            yield return new WaitForSeconds(0.01f);
            if (i >= count)
            {
                i = 0;
                break;
            }
        }
    }
    public void StartDestroy()
    {
        Invoke("SpawnPlayer", 1.4f);
    }

    public void PlayEffect(int count)
    {
        _source.PlayOneShot(_audio[count]);
    }
    public void DestroyPlayer(Collision2D obj, int xp)
    {
        Kill(xp);
        PlayEffect(2);
        ChangeHealth(-1);
        obj.gameObject.SetActive(false);
        StartDestroy();
        
    }
    public void ChangeHealth(int count)
    {
        _xp = _xp + count;
        _xpText.text = "XP: " + _xp.ToString();
        if (_xp <= 0)
        {
            Invoke("GameOver", 1.5f);
        }

    }
    private void GameOver()
    {
        SceneManager.LoadScene(0);
    }


}
