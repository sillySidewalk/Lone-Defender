using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] List<Clearing> clearings; // Each road attaches to 2 clearings
    [SerializeField] List<Pawn> pawns; // Not sure if I'll have pawns stay on road, but just in case
}
