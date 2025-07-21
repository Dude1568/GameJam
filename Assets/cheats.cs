using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cheats : MonoBehaviour
{
    [SerializeField]GameObject cheatz;
    public void cheater()
    {
        cheatz.SetActive(true);
    }
}
