using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapView : MonoBehaviour
{
    public GameObject[] water;
    public GameObject[] enemyFOV;

    public void ShowWater()
    {
        foreach (GameObject obj in water)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in enemyFOV)
        {
            obj.SetActive(false);
        }
    }

    public void ShowEnemyFOV()
    {
        foreach (GameObject obj in enemyFOV)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in water)
        {
            obj.SetActive(false);
        }
    }
}