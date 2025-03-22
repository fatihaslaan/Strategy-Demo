using Base.Input;
using StrategyDemo.Navigation_NS;
using StrategyDemo.Tile_NS;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameBoardCameraController : InputController
{
    public static event Action<TileCoordinate> TilePressed;
    public static event Action<TileCoordinate> MoveTilePressed;

    protected override void OnMoveAndAttackPointerUp(InputAction.CallbackContext obj)
    {

    }

    protected override void OnMoveAndAttackPointerDown(InputAction.CallbackContext obj)
    {

    }

    protected override void OnPointerDown()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(GetPointerPosition());
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit.collider != null)
        {
            Tile clickedObject = hit.collider.GetComponent<Tile>();
            TilePressed?.Invoke(clickedObject.TileCoordinate);
        }
    }

    protected override void OnPointerHeld()
    {

    }

    protected override void OnPointerUp()
    {

    }
}