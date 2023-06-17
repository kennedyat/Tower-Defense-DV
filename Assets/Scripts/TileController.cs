using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    public static TileController current;

    public GridLayout gridLayout;

    private Grid _grid;
    [SerializeField] private Tilemap _mainTilemap;
    [SerializeField] private TileBase _whiteTile;

    public GameObject prefab1;
    public GameObject prefab2;

    private PlaceableObject _placeableObject;

    #region Unity methods

    private void Awake()
    {
        current = this;
        _grid = gridLayout.gameObject.GetComponent<Grid>();
    }
    
    private void Update()
    {

        /*Drag from inventory from the bottom/side of screen
         * Mobile: tap multiple time to switch throught the types of tiles
         * PC/Mac hac use middle mouse to scroll through (not all mice have middle bar though
         * Have arrows indicating that the tiles can rotate foer the player
         */
        if (Input.GetKeyDown(KeyCode.A))
        {
            InitializeWithPath(prefab1);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            InitializeWithPath(prefab2);
        }

        if (!_placeableObject)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanBePlaced(_placeableObject))
            {
                _placeableObject.Place();
                Vector3Int start = gridLayout.WorldToCell(_placeableObject.GetStartPosition());
                TakeArea(start, _placeableObject.Size);
            }
            else
            {
                Destroy(_placeableObject.gameObject);
            }
        } else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(_placeableObject.gameObject);
        }

    }

    #endregion

    #region Utils

    //Gets Mouse position by using a raycast that originates from the camera to mouse pos, hitting a part of the map
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    //Takes in a world position and converts to cell pos. Returns position of the closest center of a cell matching that position on the grid
    public Vector3 SnapCoordinateToGrid(Vector3 pos)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(pos);
        pos = _grid.GetCellCenterWorld(cellPos);
        return pos;
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] blocks = new TileBase[area.size.x * area.size.y*area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x,v.y, 0);
            blocks[counter++] = tilemap.GetTile(pos);
            counter++;
        }
        return blocks;
    }

    #endregion

    #region Tile Placement

    //Initializing a object, creating said object and then assigning it the ability to drag
    public void InitializeWithPath(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject tilePath = Instantiate(prefab, position, Quaternion.identity);
        _placeableObject = tilePath.GetComponent<PlaceableObject>();
        tilePath.AddComponent<ObjectDrag>();
    }

    private bool CanBePlaced(PlaceableObject placeable)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(_placeableObject.GetStartPosition());
        area.size = placeable.Size;

        TileBase[] baseArray = GetTilesBlock(area, _mainTilemap);

        foreach(var b in baseArray)
        {
            if (b == _whiteTile)
            {
                return false;
            }
        }
        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        _mainTilemap.BoxFill(start, _whiteTile, start.x, start.y, 
                             start.x + size.x, start.y + size.y);
    }

    #endregion





    // Code for 2d tilemaps
    /*[SerializeField]
    private Tilemap _tilemap;

    [SerializeField]
    private TileBase _tile;*/


    // Update is called once per frame
   

   /* private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;
            var cellPos = _tilemap.WorldToCell(mousePos);
            _tilemap.SetTile(cellPos, _tile);
        }
    }*/
}
