using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace finished2
{
    public class MouseController : MonoBehaviour
    {
        public GameObject cursor;

        [Header("Character Info")]
        public GameObject characterPrefab;
        private CharacterInfo character;

        [Header("Movement")]
        public bool isMoving = false;
        public float speed;

        public PathFinder pathFinder;
        public List<OverlayTile> path;

        private void Start()
        {
            pathFinder = new PathFinder();
            path = new List<OverlayTile>();
        }

        void LateUpdate()
        {
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                cursor.transform.position = tile.transform.position;
                cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;
                if (Input.GetMouseButtonDown(0) && isMoving == false)
                {
                    tile.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

                    if (character == null)
                    {
                        character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                        PositionCharacterOnLine(tile);
                        character.activeTile = tile;
                    }
                    else
                    {
                        path = pathFinder.FindPath(character.activeTile, tile);

                        tile.gameObject.GetComponent<OverlayTile>().HideTile();

                        isMoving = true;
                    }
                }
            }
            if (path.Count > 0)
            {
                MoveAlongPath();
            } else
            {
                isMoving = false;
            }
            
        }

        //Moves the character along the created path
        private void MoveAlongPath()
        {
            var step = speed * Time.deltaTime;

            float zIndex = path[0].transform.position.z;
            Vector2 offset = new Vector2(path[0].transform.position.x, path[0].transform.position.y + 0.18f);
            character.transform.position = Vector2.MoveTowards(character.transform.position, offset, step);
            character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

            if (Vector2.Distance(character.transform.position, offset) < 0.00001f)
            {
                PositionCharacterOnLine(path[0]);
                path.RemoveAt(0);
            }

        }

        //Positions the character directly center of the tile
        private void PositionCharacterOnLine(OverlayTile tile)
        {
            character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.18f, tile.transform.position.z - 0.1f);
            character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
            character.activeTile = tile;
        }

        //Allows the mouse to recognize tiles
        private static RaycastHit2D? GetFocusedOnTile()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            if (hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }
    }
}