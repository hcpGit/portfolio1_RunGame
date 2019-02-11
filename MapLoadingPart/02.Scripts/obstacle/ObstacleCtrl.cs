﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    [System.Serializable]
    public abstract class ObstacleCtrl : MonoBehaviour,IChildModel
    {
        protected Transform playerTr;
        protected ObstacleST obsST;
        public Material[] materials;
        protected GameObject childModel;
        protected IObjToCharactor objToCharactor;
        protected Renderer childModelRdr;
        protected Collider childModelCollider;

        public virtual void FromChildOnCollisionEnter(GameObject child, Collision coll)
        {
            Debug.Log("콜리전 이벤트. 원래 이건 일어나서는 안됨.");
        }

        public virtual void FromChildOnTriggerEnter(GameObject child, Collider other)
        {
        }

        protected virtual void Awake()
        {
            obsST = new ObstacleST();
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
            childModel = transform.Find("childModel").gameObject; //구형 장애물 모델링 자식 추출  (transform.find로 자식 중 내에서 검색)
            obsST.beenHit = false;
            objToCharactor = playerTr.gameObject.GetComponent<IObjToCharactor>();
            childModelRdr = childModel.GetComponent<Renderer>();
            childModelCollider = childModel.GetComponent<Collider>();
        }
        protected virtual void OnEnable()
        {
            childModelRdr.material = materials[Random.Range(0, materials.Length)];
            obsST.beenHit = false;
        }

        protected virtual void OnDisable()
        { 
            obsST.beenHit = false;
            if (childModelCollider.enabled == false)
            {
                childModelCollider.enabled = true;
            }
        }
    }
}