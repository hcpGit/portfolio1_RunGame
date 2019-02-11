﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class BossMapST
    {
        public GameObject chunk;
        public List<GameObject> objs = new List<GameObject>();

        public void MoveAll(float moveSpeed)
        {
            chunk.transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);

            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
            }
        }
        public bool IsShouldBeRemoved(float chunkMargin)
        {
            if (chunk.transform.position.z <= -1 * ((Constants.wantToShowNumOfChunksInBehind + 1) * chunkMargin))
                return true;
            else return false;
        }
        public void TurnInObjs()
        {
            for (int i = 0; i < objs.Count; i++)
            {
                MapAndObjPool.GetInstance().TurnInPoolObj(objs[i]);
            }
            objs.Clear();
        }
        public bool IsDisabled()
        {
            if (chunk == null)
                return true;
            else return false;
        }
        public void MoveChunk(float pos)
        {
            chunk.transform.position = Vector3.forward * pos;
        }
        public void ObjIn(int spawnPointNum, GameObject obj)
        {
            if (obj == null || IsDisabled()) ErrorManager.SpurtError("업음");

            Transform spawnPoint = chunk.transform.GetChild(3) //3은 굉장히 청크의 자식순서에 종속적인 값
                .GetChild(spawnPointNum);

            obj.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            obj.SetActive(true);
            objs.Add(obj);
        }
    }



    public static class MyExtendMethodForListBossMapST
    {
        public static void MoveAllInList(this List<BossMapST> list, float moveSpeed)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsDisabled() == false)
                    list[i].MoveAll(moveSpeed);
            }
        }
        public static BossMapST FindChunkShouldBeRemoved(this List<BossMapST> list, float chunkMargin)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsDisabled() == false && list[i].IsShouldBeRemoved(chunkMargin))
                    return list[i];
            }
            return null;
        }
        public static float GetNewCreatePoint(this List<BossMapST> list, float chunkMargin)
        {
            float last = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsDisabled()) continue;

                if (list[i].chunk.transform.position.z > last)
                    last = list[i].chunk.transform.position.z;
            }
            return last + chunkMargin;
        }
    }

}