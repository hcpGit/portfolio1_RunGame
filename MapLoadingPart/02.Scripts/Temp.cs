﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hcp;
public class Temp : MonoBehaviour ,IObjToCharactor{
    public float moveSpeed=10f;
    public void BeenHitByObs(ObstacleST obstacleST)
    {
       // print("옵스타클 이벤트 받음.");
    }

    public void GetItem(ItemST itemST)
    {
      //  print("아이템 이벤트 받음.");
    }

    public bool GetMagnetState()
    {
        print("마그넷상태");
        return false;
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetKey("w"))
        {
            this.transform.Translate(Vector3.forward* moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey("d"))
        {
            this.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey("s"))
        {
            this.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
    }
}
