using System;
using UnityEngine;
using static PlayerStateType;
// static in class semantically enforces that there should be no instances of this in the code
public static class PlayerStateFactory
{
    public static PlayerState[] CreateStates(
        CameraManager cameraManager,
        PlayerData playerData,
        PlayerMovement playerMovement,
        CameraState fpv,
        CameraState reload,
        Func<PlayerStateType> getCurrentState,
        Func<PlayerStateType> getPreviousState
    )
    {
        return new PlayerState[]
        {
            new PlayerState(
                name: "Idling",
                duration: 0,
                allowedFrom: new PlayerStateType[] { Idling },
                interruptedBy: new PlayerStateType[] { Stunned },
                onEnter: () => {
                    // movementCamera.SetState(movementCamera.activeWayPoint.trackType);
                }
            ),
            new PlayerState(
                name: "Aiming",
                duration: 0,
                allowedFrom: new PlayerStateType[] { Moving, Idling, Aiming },
                interruptedBy: new PlayerStateType[] { Stunned },
                onEnter: () => {
                    if (getPreviousState() != Swapping)
                    {
                        playerData.SummonCurrentItem(false);
                        cameraManager.Inject(fpv);
                    }
                },
                onExit: () => {
                    // Debug.Log("HIT AIM EXIT");
                    if (getCurrentState() != Firing && getCurrentState() != Swapping)
                    {
                        Debug.Log("1");
                        playerData.DespawnCurrentItem(false);
                        cameraManager.ExitInject();
                    }
                }
            ),
            new PlayerState(
                name: "Reloading",
                duration: 2.0f,
                allowedFrom: new PlayerStateType[] { Idling, Moving, Aiming },
                interruptedBy: new PlayerStateType[] { Stunned },
                onEnter: () => {
                    playerData.ReloadCurrentItem();
                    cameraManager.Inject(reload);
                },
                onExit: () => {
                    cameraManager.ExitInject();
                }
            ),
            new PlayerState(
                name: "Moving",
                duration: 0,
                allowedFrom: new PlayerStateType[] { Idling },
                interruptedBy: new PlayerStateType[] { Stunned },
                onUpdate: () => {
                    playerMovement.Move();
                }
            ),
            new PlayerState(
                name: "Turning",
                duration: 0.5f,
                allowedFrom: new PlayerStateType[] { Idling },
                interruptedBy: new PlayerStateType[] { Stunned, Dashing },
                onEnter: () => {
                    playerMovement.Turn();
                }
            ),
            new PlayerState(
                name: "Firing",
                duration: 1.0f,
                allowedFrom: new PlayerStateType[] { Aiming },
                interruptedBy: new PlayerStateType[] { Stunned },
                onEnter: () => {
                    playerData.TossCurrentItem();
                },
                onExit: () => {
                    if (getCurrentState() != Aiming)
                    {
                        playerData.DespawnCurrentItem(false);
                        cameraManager.ExitInject();
                    }
                }
            ),
            new PlayerState(
                name: "Dashing",
                duration: 0.5f,
                allowedFrom: new PlayerStateType[] { Turning, Moving },
                interruptedBy: new PlayerStateType[] { },
                onEnter: () => {
                    playerMovement.Turn();
                },
                onUpdate: () => {
                    playerMovement.Move();
                }
            ),
            new PlayerState(
                name: "ForwardDashing",
                duration: 0.5f,
                allowedFrom: new PlayerStateType[] { Turning },
                interruptedBy: new PlayerStateType[] { },
                onUpdate: () => {
                    playerMovement.Move();
                }
            ),
            new PlayerState(
                name: "Stunned",
                duration: 1.0f,
                allowedFrom: new PlayerStateType[] { AllStates },
                interruptedBy: new PlayerStateType[] { }
            ),
            new PlayerState(
                name: "Jumping",
                duration: 0,
                allowedFrom: new PlayerStateType[] { },
                interruptedBy: new PlayerStateType[] { }
            ),
            new PlayerState(
                name: "Swapping",
                duration: 0.2f,
                allowedFrom: new PlayerStateType[] { Idling, Moving, Aiming},
                interruptedBy: new PlayerStateType[] { Stunned, Turning, /* Aiming ? */ },
                onEnter: () => {
                    if(getPreviousState() == Aiming)
                    {
                        //fpv swap - this needs to be animation controlled
                        playerData.DespawnCurrentItem(true);
                        playerData.SwapCurrentItem();
                        playerData.SummonCurrentItem(true);
                    }
                    else
                    {
                        playerData.SwapCurrentItem();
                    }
                },
                onExit: () => {
                    if(getCurrentState() != Aiming)
                    {
                        cameraManager.ExitInject();
                        playerData.DespawnCurrentItem(false);
                    }
                }
            ),
            new PlayerState(
                name: "AllStates",
                duration: 0,
                allowedFrom: new PlayerStateType[] { },
                interruptedBy: new PlayerStateType[] { }
            ),
        };
    }
}