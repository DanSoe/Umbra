﻿using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour
{
    private knightController enemyKnight;
    private GhoulController enemyGhoul;

    // Use this for initialization
    void Start()
    {
        
      //  enemyKnight = GameObject.FindGameObjectWithTag("Knight").GetComponent<knightController>();


    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider hittarget)
    {
        //Debug.Log(hittarget.gameObject.name);
        if (gameObject.tag == "lightA")
        {
            if (hittarget.gameObject.tag == "ghoul")
            {
                enemyGhoul = hittarget.GetComponent<GhoulController>();
                Debug.Log("One Damage");
                enemyGhoul.takeDamage(1);

            }
            if (hittarget.gameObject.tag == "Knight")
            {
                enemyKnight = hittarget.GetComponent<knightController>();
                if (enemyKnight.chase == true)
                {
                    enemyKnight.takeDamage(1);
                    Debug.Log("1 dmg");
                }
                else
                {
                    enemyKnight.takeDamage(2);
                    
                }
            }
        }
        else
        {
            if (hittarget.gameObject.tag == "ghoul")
            {
                enemyGhoul = hittarget.GetComponent<GhoulController>();
                Debug.Log("Two Damage");
                enemyGhoul.takeDamage(1);
            }
            if (hittarget.gameObject.tag == "Knight")
            {
                enemyKnight = hittarget.GetComponent<knightController>();

                if (enemyKnight.chase == true)
                {
                    enemyKnight.takeDamage(2);
                }
                else
                {
                    enemyKnight.takeDamage(4);
                }
                

            }

        }
    }
}
