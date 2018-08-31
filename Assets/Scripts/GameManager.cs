using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;   // 스크립트 인스턴스

    public GameObject character;

    public GameObject WinText;
    public GameObject LoseText;
    public GameObject TipText;

    void Awake()
    {
        GameManager.instance = this;     // 스크립트 인스턴스
    }

    public GameObject Spawn()
    {
        character.SetActive(true);
        return character;
    }

    public void GameClear()
    {
        WinText.SetActive(true);
    }

    public void GameOver()
    {
        LoseText.SetActive(true);
    }

    public void OnTip()
    {
        TipText.SetActive(true);
    }

    public void CloseTip()
    {
        TipText.SetActive(false);
    }
}
