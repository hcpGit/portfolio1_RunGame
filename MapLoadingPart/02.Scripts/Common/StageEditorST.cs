using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    [System.Serializable]
    public class StageEditorST
    {
        public int pos;    
        public E_WhichTurn whichTurn;
        public StageObjArr soa = new StageObjArr();   

        [System.NonSerialized]
        public GameObject floorChunk;
        [System.NonSerialized] 
        public bool isNowShowed=false;
        [System.NonSerialized] 
        public List<GameObject> objs = new List<GameObject>();
        [System.NonSerialized] 
        Vector3 bornPos = new Vector3(0, 0, 0);
        [System.NonSerialized] 
        Transform spg;

        public int GetRealTurningPoint()
        {
            return pos + 2; //턴청크에 종속적인 값 2
        }

        public void showThisToEditor(int standardPos, float chunkMargin)
        {
            bornPos.z = (pos - standardPos) * chunkMargin;  //기준점 대응 현재 pos 값 얻음.
            

            switch (whichTurn)
            {
                
                case E_WhichTurn.LEFT:
                    //왼쪽 으로 청크 풀링 해오기.
                    floorChunk =
                        MapAndObjPool.GetInstance().GetTurnLeftChunksInPool();
                    break;
                case E_WhichTurn.RIGHT:
                    //오른쪽으로 청크 풀링
                    floorChunk =
                        MapAndObjPool.GetInstance().GetTurnRightChunksInPool();
                    break;
                case E_WhichTurn.NOT_TURN:
                    floorChunk
                        = MapAndObjPool.GetInstance().GetChunkInPool();
                    break;
                default: ErrorManager.SpurtError("위치턴이 아무것도 아닌 심각한 오류.");
                    break;
            }
            floorChunk.transform.position = bornPos;
            floorChunk.SetActive(true);

            if (whichTurn == E_WhichTurn.NOT_TURN)
            {
                spg = floorChunk.transform.GetChild(3);
                FixedObjGenerator.FixedObjGen(spg, soa.spawnObjType, ref objs);
            }
            isNowShowed = true;
        }

        public void DisappearFromEditor()
        {
            if (floorChunk == null)
                ErrorManager.SpurtError("생성도 안됐는데 삭제를 불러와짐");
           
                MapAndObjPool.GetInstance().TurnInPoolObj(floorChunk);

                floorChunk = null;
                spg = null;

            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i] != null)
                    MapAndObjPool.GetInstance().TurnInPoolObj(objs[i]);
            }
            objs.Clear();

            isNowShowed = false;
        }
        bool UpperHuddleCheck(int spawnPointNum, E_SPAWN_OBJ_TYPE changingType, StageObjArr soa)
        {
            //검증단계
            if (spawnPointNum == 1)
            {
                if (soa.spawnObjType[0] == E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_2 || soa.spawnObjType[0] == E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_3)
                {
                    return false;
                }

                if (changingType == E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_3)
                    return false;
            }
            if (spawnPointNum == 2)
            {
                if (soa.spawnObjType[1] == E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_2 || soa.spawnObjType[1] == E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_3)
                {
                    return false;
                }

                if (changingType == E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_3 || changingType==E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_2)
                    return false;
            }



            if (spawnPointNum == 0) //보정 단계
            {
                if (changingType == E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_2)
                {
                    soa.spawnObjType[1] = E_SPAWN_OBJ_TYPE.NOTHING;
                }
                if (changingType == E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_3)
                {
                    soa.spawnObjType[1] = E_SPAWN_OBJ_TYPE.NOTHING;
                    soa.spawnObjType[2] = E_SPAWN_OBJ_TYPE.NOTHING;
                }
            }
            else if (spawnPointNum == 1)
            {
                
                if (changingType == E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_2)
                {
                    soa.spawnObjType[2] = E_SPAWN_OBJ_TYPE.NOTHING;
                }
                
            }
            return true;
        }

        public void ChangeObj(int objNum, E_SPAWN_OBJ_TYPE changingType)
        {
            if (floorChunk == null) ErrorManager.SpurtError("생성도 안됐는데 오브젝트를 바꾸려함.");

            if (false == UpperHuddleCheck(objNum, changingType, soa))
            {
                Debug.Log("어퍼허들 체크에서 오류.");
                return;
            }

            if (whichTurn != E_WhichTurn.NOT_TURN)
            {
                Debug.Log("회전 청크인데 오브젝트를 체인지 하려고함.");
                return;
            }

            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i] != null)
                    MapAndObjPool.GetInstance().TurnInPoolObj(objs[i]);
            }
            objs.Clear();//참조만 날라감. 괜찮음.

            soa.spawnObjType[objNum] = changingType;

            spg = floorChunk.transform.GetChild(3);

            FixedObjGenerator.FixedObjGen(spg, soa.spawnObjType, ref objs);
        }

        public void ChangeFloor(E_WhichTurn whichTurn)   
        {
            if (floorChunk == null) ErrorManager.SpurtError("생성도 안됐는데 청크를 바꾸려함,");
            if (this.whichTurn == whichTurn)
            {
                Debug.Log("똑같은 턴 방향임");
                return;
            }

            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i] != null)
                    MapAndObjPool.GetInstance().TurnInPoolObj(objs[i]);
            }
            objs.Clear();//참조만 날라감. 괜찮음.

            soa.spawnObjType[0] = E_SPAWN_OBJ_TYPE.NOTHING;
            soa.spawnObjType[1] = E_SPAWN_OBJ_TYPE.NOTHING;
            soa.spawnObjType[2] = E_SPAWN_OBJ_TYPE.NOTHING;

            this.whichTurn = whichTurn;
            //위치턴 따라서 플로어 청크 분기해오기.
            bornPos.z = floorChunk.transform.position.z;
            MapAndObjPool.GetInstance().TurnInPoolObj(floorChunk);

            switch (whichTurn)
            {
                case E_WhichTurn.LEFT:
                    //왼쪽 으로 청크 풀링 해오기.
                    floorChunk =
                       MapAndObjPool.GetInstance().GetTurnLeftChunksInPool();
                    break;
                case E_WhichTurn.RIGHT:
                    //오른쪽으로 청크 풀링
                    floorChunk =
                     MapAndObjPool.GetInstance().GetTurnRightChunksInPool();
                    break;
                case E_WhichTurn.NOT_TURN:
                    floorChunk
                        = MapAndObjPool.GetInstance().GetChunkInPool();
                    break;
                default:
                    ErrorManager.SpurtError("위치턴이 아무것도 아닌 오류.");
                    break;
            }
            floorChunk.transform.position = bornPos;
            floorChunk.SetActive(true);
        }

        public StageEditorST(int pos, E_WhichTurn whichTurn)
        {
            this.pos = pos;
            this.whichTurn = whichTurn;
        }
        public StageEditorST(int pos, E_WhichTurn whichTurn , StageObjArr soa)
        {
            this.pos = pos;
            this.whichTurn = whichTurn;
            this.soa = soa;
        }

        public bool IsTurnChunks()
        {
            if (whichTurn != E_WhichTurn.NOT_TURN)
                return true;
            else return false;
        }

        public void ReadyForGrave()
        {
            if (floorChunk != null)
            {
                MapAndObjPool.GetInstance().TurnInPoolObj(floorChunk);
            }
            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i] != null)
                    MapAndObjPool.GetInstance().TurnInPoolObj(objs[i]);
            }
            objs.Clear();//참조만 날라감. 괜찮음.

        }
    }
}