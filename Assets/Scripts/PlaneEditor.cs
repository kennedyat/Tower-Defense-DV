using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlaneEditor : MonoBehaviour
{
    
    public int terrainNumber;
    public GridLayout gridLayout;
    [SerializeField] private Tilemap _terrainTilemap;
    [SerializeField] Vector3Int Area;
    [SerializeField] GameObject[] Terrains;
    [SerializeField] GameObject[] Corners;
    
    private GameObject _terrain;


    private void Awake()
    {
        SetTerrain();
        SetArea();
    }
  
    private void Start()
    {
        Debug.Log("Start");
        InstantiateTerrain();
    }
    

    #region Set Plane
    private void SetTerrain()
    {
        _terrain = Terrains[terrainNumber];
    }
    private void SetArea()
    {
        transform.localScale = Area;
    }

    #endregion

    #region Utils
    private List<Vector3Int> FindCellPositions()
    {
        //Find bounds of tile????
        Debug.Log("Here " );
        List<Vector3Int> positions = new List<Vector3Int>();
        Vector3 startPos = Corners[0].transform.position;
        int startPosX = Mathf.FloorToInt(startPos.x);
        int startPosZ = Mathf.FloorToInt(startPos.z);
        Debug.Log("PosX: " +startPosX + "PosZ: " + startPosZ + "Overall: "+ startPos);
        for (float i = startPosX; i>=startPosX - Area.x * 10; i = i - .5f) //Why multiply by 5 instead of 10
        {
            for (float j = startPosZ; j>=startPosZ-Area.z*10; j = j-.5f )
            {
                Debug.Log("X: " + i + " Z: " + j);
                positions.Add(gridLayout.WorldToCell(transform.TransformPoint(
                        new Vector3(i, 0f, j))));
              
            }
        }
        return positions;
    }
    #endregion

    #region Create Terrain
    private void InstantiateTerrain()
    {
        List<Vector3Int> cellPos = FindCellPositions();
        foreach (var cell in cellPos)
        {
            Debug.Log("Tile: " + _terrain + "CellPos: " + cell);
            Instantiate(_terrain, _terrainTilemap.GetCellCenterWorld(cell/2), Quaternion.identity);
            
        }
    }

    #endregion




}
