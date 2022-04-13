using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int lives { get; private set; }


    void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if(this.lives <= 0 && Input.anyKeyDown)
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
        for (int i = 0; i < this.ghosts.Length; i++) { this.ghosts[i].gameObject.SetActive(false); }

        this.pacman.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
    }
    private void SetLives(int lives)
    {
        this.lives = lives;
    }
    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
    }
    public void PacmanEaten()
    {
        pacman.DeathSequence();
        SetGhosts(false);

        SetLives(this.lives -1);

        if(this.lives > 0){Invoke(nameof(ResetState), 2.0f);}
        else{GameOver();}
    }
    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(this.score + pellet.points);

        if(!HasRemainingPellets())
        {
            Invoke(nameof(NewRound), 3.0f);
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
        foreach(Transform pellet in pellets)
        {
            if(pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    public void SetGhosts(bool isActive)
    {
        for ( int i = 0; i < ghosts.Length; i++){
            ghosts[i].gameObject.SetActive(isActive);
        }
    }
    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
