﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
   public void OnCompleteAnimation()
    {
        Destroy(gameObject);
    }
}
