using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinder
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end)
    {
        List <OverlayTile> openlist = new List<OverlayTile>();
        List<OverlayTile> closedlist = new List<OverlayTile>();

        openlist.Add(start);
        while(openlist.Count > 0)
        {
            OverlayTile currentOverlayTile = openlist.OrderBy(x => x.f).First();

            openlist.Remove(currentOverlayTile);
            closedlist.Add(currentOverlayTile);

            if(currentOverlayTile == end)
            {
                //Finalizes Path
                return GetFinishedList(start, end);

            }
            var neighborTiles = GetNeighborTiles(currentOverlayTile);

            foreach (var neighbor in neighborTiles)
            {
                //1 in this case is the character's jump height
                if (neighbor.isBlocked || closedlist.Contains(neighbor) || Mathf.Abs(currentOverlayTile.gridLocation.z - neighbor.gridLocation.z) > 1 )
                {
                    continue;
                }

                neighbor.g = GetManhattenDistance(start, neighbor);
                neighbor.h = GetManhattenDistance(end, neighbor);
                neighbor.previous = currentOverlayTile;

                if (!openlist.Contains(neighbor))
                {
                    openlist.Add(neighbor);
                }
            }
        }
        return new List<OverlayTile>();
    }
    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();
        OverlayTile currentTile = end;
        
        while(currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }
        finishedList.Reverse();
        return finishedList;
    }

    private int GetManhattenDistance(OverlayTile start, OverlayTile neighbor)
    {
        return Mathf.Abs(start.gridLocation.x - neighbor.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbor.gridLocation.y);
    }

    //Gets the neighboring overlayTile locations
    private List<OverlayTile> GetNeighborTiles(OverlayTile currentOverlayTile)
    {
        var map = MapManager.Instance.map;

        List<OverlayTile> neighbors = new List<OverlayTile>();

        //Top
        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);
        if (map.ContainsKey(locationToCheck))
        {
            neighbors.Add(map[locationToCheck]);
        }        
        //Bottom
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);
        if (map.ContainsKey(locationToCheck))
        {
            neighbors.Add(map[locationToCheck]);
        }
        //Right
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1, currentOverlayTile.gridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            neighbors.Add(map[locationToCheck]);
        }
        //Top
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            neighbors.Add(map[locationToCheck]);
        }

        return neighbors;
    }

}
