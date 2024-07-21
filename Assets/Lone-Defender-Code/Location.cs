using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Any type of location where pawns can stand: Forest and Clearing
 */
public class Location : MonoBehaviour
{
    [SerializeField] protected int id;
    [SerializeField] protected int max_buildings;
    [SerializeField] public List<Building> buildings; // Number of buildings is limited
    [SerializeField] public List<LD_Token> tokens; // Number of tokens is not limited
    [SerializeField] public List<Pawn> pawns; // Pawns are not limited (Probably)
    [SerializeField] public List<Location> adjacent_locations; // Clearings and Forests
    [SerializeField] public List<Road> adjacent_roads; // slightly different context between forest and clearing, but I think it'll be ok


}
