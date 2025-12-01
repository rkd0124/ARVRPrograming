using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IEnemy.cs
public interface IEnemy
{
    void ApplySlow(float percent);  // 이동속도 감속
    void RemoveSlow();              // 원래 속도로 복귀
}

