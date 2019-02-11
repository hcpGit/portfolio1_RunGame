using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class BossMapMgr : SingletonTemplate<BossMapMgr>
    {
        public GameObject chunk;
        public float moveSpeed = 20.0f;
        List<BossMapST> mapSTList = new List<BossMapST>();
        float chunkMargin;

        bool stop = false;

        protected override void Awake()
        {
            base.Awake();

            chunkMargin = chunk.GetComponentInChildren<Renderer>().bounds.size.z;

        }
        private void Start()
        {
            MapAndObjPool.GetInstance().ChunkPoolInit(Constants.wantToShowNumOfChunks);
            MapAndObjPool.GetInstance().bossObsBreathPoolInit();
            MapAndObjPool.GetInstance().bossObsFireBallPoolInit();
            MapAndObjPool.GetInstance().bossObsMeteorPoolInit();
            InitMapST();
        }

        private void Update()
        {
            if (!stop)
            {
                mapSTList.MoveAllInList(moveSpeed);
                BossChunkLoading();
            }
        }

        public void StopMapMove()
        {
            stop = true;
        }

        void BossChunkLoading()
        {
            BossMapST bst = mapSTList.FindChunkShouldBeRemoved(chunkMargin);
            if (bst == null) return;

            bst.TurnInObjs();
            bst.MoveChunk(mapSTList.GetNewCreatePoint(chunkMargin));

        }
        


        void InitMapST()
        {
            BossMapST bst;
            float startPoint = -1*Constants.wantToShowNumOfChunksInBehind;
            for (int i = 0; i < Constants.wantToShowNumOfChunks; i++)
            {
                bst = new BossMapST();
                
                GameObject chunk = 
                MapAndObjPool.GetInstance().GetChunkInPool();

                if (chunk == null) ErrorManager.SpurtError("청크가 널임");
                if (startPoint == 0)
                    chunk.transform.position = Vector3.zero;
                else
                    chunk.transform.position = Vector3.forward*( startPoint * chunkMargin);

                bst.chunk = chunk;
                bst.chunk.SetActive(true);
                startPoint++;

                mapSTList.Add(bst);
            }
        }
    }

    
}