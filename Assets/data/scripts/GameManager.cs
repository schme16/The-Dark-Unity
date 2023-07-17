using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerScript player;
    public LanternScript lantern;

    void Start()
    {
        player.gm = this;
        lantern.gm = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
