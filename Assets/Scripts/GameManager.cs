using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Dog[] dog;
    [SerializeField] private Rabbit rabbit;
    [SerializeField] private Transform carrots;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    public int dogMultiplier { get; private set; } = 1;
    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;


    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }
    
    private void Start()
    {
        if (rabbit == null)
        {
        Debug.LogError("Rabbit reference not set in GameManager!");
        }

        NewGame();
    }


    private void Update()
    {
        if(this.lives <= 0 && Input.anyKeyDown){
            NewGame();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        gameOverText.enabled = false;

        foreach(Transform carrot in carrots)
        {
            carrot.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {

        for(int i = 0; i<this.dog.Length; i++){
            this.dog[i].ResetState();
        }

        rabbit.ResetState(); 
    }

    private void GameOver()
    {
        gameOverText.enabled = true;

        for(int i = 0; i<dog.Length; i++){
            dog[i].gameObject.SetActive(false);
        }

        rabbit.gameObject.SetActive(false); 
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }


    public void DogEaten(Dog dog)
    {
        int points = dog.points * dogMultiplier;
        SetScore(score + points);

        dogMultiplier++;
        //new code - test 1
        dog.gameObject.SetActive(false);
        dog.Invoke(nameof(dog.ResetState), 4f);
    }

    public void RabbitEaten()
    {
        //this.rabbit.gameObject.SetActive(false);
        rabbit.DeathSequence();

        SetLives(lives - 1);

        if(lives > 0){
            Invoke(nameof(ResetState), 3f);
        }
        else {
            GameOver();
        }
    }

    public void CarrotEaten(Carrot carrot)
    {
        carrot.gameObject.SetActive(false);

        SetScore(score + carrot.points);

        if(!HasRemainingCarrots())
        {
            rabbit.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }

    }

    public void PowerCarrotEaten(PowerCarrot carrot)
    {
       for(int i =0; i < dog.Length; i++) 
       {
            dog[i].frightened.Enable(carrot.duration);
       } 

        CarrotEaten(carrot);
        CancelInvoke(nameof(ResetDogMultiplier));
        Invoke(nameof(ResetDogMultiplier), carrot.duration);
        
    }

    private bool HasRemainingCarrots()
    {
        foreach(Transform carrot in carrots)
        {
            if(carrot.gameObject.activeSelf){
                return true;
            }
        }
        return false; 
    }

    private void ResetDogMultiplier()
    {
        dogMultiplier = 1;
    }
}