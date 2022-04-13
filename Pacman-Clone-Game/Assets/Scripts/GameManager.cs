using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;

    [Header("Pacman Info")]
    public Pacman pacman;
    public Transform pellets;
    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int lives { get; private set; }
    public AudioClip pacmanEatenSFX;
    public AudioClip ghostEatenSFX;
    AudioSource audioSource;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Audios")]
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI gameOverMessageText;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (this.lives <= 0 && Input.GetKeyDown(KeyCode.R))
        {
            NewGame();
        }
        if (!HasRemainingPellets() && Input.GetKeyDown(KeyCode.R))
        {
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
        this.gameObject.SetActive(true);
        foreach (Transform pellet in this.pellets) { pellet.gameObject.SetActive(true); }

        audioSource.Stop();
        gameOverText.enabled = false;
        gameOverMessageText.enabled = false;

        ResetState();
    }

    private void ResetState()
    {
        for (int i = 0; i < this.ghosts.Length; i++) { this.ghosts[i].ResetState(); }
        SetGhosts(true);

        this.pacman.ResetState();
    }
    private void GameOver()
    {
        this.gameObject.SetActive(true);
        gameOverText.enabled = true;
        gameOverMessageText.enabled = true;

        for (int i = 0; i < this.ghosts.Length; i++) { this.ghosts[i].gameObject.SetActive(false); }

        audioSource.Play();

        this.pacman.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString("0000");
    }
    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }
    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
        AudioSource.PlayClipAtPoint(ghostEatenSFX, this.transform.position);

    }
    public void PacmanEaten()
    {
        pacman.DeathSequence();
        SetGhosts(false);

        SetLives(this.lives - 1);

        AudioSource.PlayClipAtPoint(pacmanEatenSFX, this.transform.position);

        if (this.lives > 0) { Invoke(nameof(ResetState), 2.0f); }
        else { GameOver(); }

    }
    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(this.score + pellet.points);

        if (!HasRemainingPellets())
        {
            Invoke(nameof(GameOver), 2.0f);
            this.gameObject.SetActive(false);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }
    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    public void SetGhosts(bool isActive)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(isActive);
        }
    }
    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
