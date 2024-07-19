using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Any type of location where pawns can stand: Forest and Clearing
 */
public class Location : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] int max_buildings;
    [SerializeField] List<Building> buildings; // Number of buildings is limited
    [SerializeField] List<LD_Token> tokens; // Number of tokens is not limited
    [SerializeField] List<Pawn> pawns; // Pawns are not limited (Probably)
}
