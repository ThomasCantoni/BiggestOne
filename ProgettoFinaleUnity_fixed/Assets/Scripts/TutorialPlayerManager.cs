using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class TutorialPlayerManager : MonoBehaviour
{
    public FirstPersonController FPS;
    public GenericGun TutorialGun,Flamethrower;
    public int enemiesToKill=2;
    public int enemiesToKillWithFlamethrower = 4;

    public int deadEnemies=0;
    public UnityEvent OnEnemiesKilled,OnEnemiesKilledFlame;
    void Start()
    {
        FPS = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
    }

    public void EnableJump(FirstPersonController FPS)
    {
        FPS.CanJumpInTutorial = true;
    }
    public void EnableDash(FirstPersonController FPS)
    {
        FPS.GetComponent<Dash>().enabled = true;
        FPS.GetComponent<Dash>().DashEnabled = true;

    }
    public void EnableSlide(FirstPersonController FPS)
    {
        FPS.GetComponent<SlideCharacter>().enabled = true;
    }
    public void DisableSlide(FirstPersonController FPS)
    {
        FPS.GetComponent<SlideCharacter>().enabled = false;


    }
    public void DisableDash(FirstPersonController FPS)
    {
        FPS.GetComponent<Dash>().DashEnabled = false;
    }
    public void EnableGrenade(FirstPersonController FPS)
    {
        FPS.CanGrenadeInTutorial = true;
    }
    public void EnableWallRun(FirstPersonController FPS)
    {
        FPS.GetComponent<WallParkour>().enabled = true;

    }
    public void EnableGun(FirstPersonController FPS)
    {
        FPS.WS.AddGun(TutorialGun);
    }
    public void StartFlamethrower(FirstPersonController FPS)
    {
        FPS.WS.RemoveAllGuns();
        FPS.WS.AddGun(Flamethrower,true);
    }
    public void EnemyAnimation(EnemyMeleeAI EM)
    {
        
            EM.anim.SetBool("Tutorial", true);

        EM.OnEnemyDeath += () =>
        {
            EM.anim.SetBool("Tutorial", false);
        };
        
    }
    public void OnEnemyKilled(EnemyClass justDied)
    {
        deadEnemies++;
        if(deadEnemies == enemiesToKill)
        {
            OnEnemiesKilled?.Invoke();
            deadEnemies = 0;
        }
    }
    public void OnEnemyKilledFlamethrower()
    {
        deadEnemies++;
        if (deadEnemies == enemiesToKillWithFlamethrower)
        {
            OnEnemiesKilledFlame?.Invoke();
            deadEnemies = 0;
        }
    }
    public void TutorialFinished()
    {
        SceneManager.LoadScene("Scenes/Menu 3D");
    }
}
