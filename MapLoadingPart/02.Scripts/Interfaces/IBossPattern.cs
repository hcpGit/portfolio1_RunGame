﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using hcp;
public interface IBossPattern
{
    void BossPatternObjGen(E_BOSSPATTERN pattern, float disFromPlayer, E_SPAWNLINE line);
    void BossPatternFireBallShoot(Vector3 pos);
}