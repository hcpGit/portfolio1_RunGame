using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace hcp
{
    public class ErrorManager {

        public static void SpurtError(string msg="")
        {
            Debug.Log(msg);
            Assert.IsTrue(true);
        }
    }
}