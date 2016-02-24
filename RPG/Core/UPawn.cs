﻿using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 当前PlayerController控制的对象，拥有实体
/// </summary>
public class UPawn : UActor
{
    public UController Controller;
    UPlayerState PlayerState;
    private bool bInputEnabled;
    public string PawnName;

    public UPawn()
    {
        CreatePlayerInputComponent();
    }
    public void SetPlayerState(UPlayerState PlayerState)
    {
        this.PlayerState = PlayerState;
    }
    public override void BeginPlay()
    {
        base.BeginPlay();
        SetupPlayerInputComponent(InputComponent);
    }
    public override void Tick(float DeltaSeconds)
    {
        base.Tick(DeltaSeconds);
        InputComponent.bBlockInput = !bInputEnabled;
    }
    public virtual void Reset() { }
    public virtual void SetupPlayerInputComponent(UInputComponent InInputComponent) { }
    public override void SetName(string s)
    {
        PawnName = s;
    }
    public override string GetHumanReadableName()
    {
        return PawnName;
    }
    public virtual void CreatePlayerInputComponent()
    {
        InputComponent = new UInputComponent(this, "PawnInputComponent0");
    }
    public virtual void DestroyPlayerInputComponent()
    {
        if (InputComponent != null)
        {
            InputComponent.ClearBindingValues();
            InputComponent = null;
        }
    }
    public void BindAction(string KeyName, EInputActionType ActionType, UnityAction ActionDelegate)
    {
        InputComponent.BindAction(KeyName, ActionType, ActionDelegate);
    }
    public void BindAxisBindAxis(string KeyName, UnityAction<float> ActionDelegate)
    {
        InputComponent.BindAxis(KeyName, ActionDelegate);
    }

    public void PossessedBy(UController NewController)
    {
        Controller = NewController;
        if (Controller.PlayerState != null)
        {
            PlayerState = Controller.PlayerState;
        }
    }
    public void UnPossessed()
    {
        UController OldController = Controller;

        PlayerState = null;
        transform.SetParent(null);
        Controller = null;

        // Unregister input component if we created one
        DestroyPlayerInputComponent();
    }
    public bool IsControlled()
    {
        UPlayerController PC = (UPlayerController)(Controller);
        return (PC != null);
    }
    /// <summary>
    /// 获取Controller
    /// </summary>
    /// <returns></returns>
    public UController GetController()
    {
        return Controller;
    }
    public override void EnableInput(UPlayerController PlayerController)
    {
        if (PlayerController == Controller || PlayerController == null)
        {
            InputComponent.bBlockInput = false;
        }
        else
        {
            Debug.LogError("EnableInput can only be specified on a Pawn for its Controller");
        }
    }

    public override void DisableInput(UPlayerController PlayerController)
    {
        if (PlayerController == Controller || PlayerController == null)
        {
            bInputEnabled = false;
        }
        else
        {
            Debug.LogError("DisableInput can only be specified on a Pawn for its Controller");
        }
    }
}
