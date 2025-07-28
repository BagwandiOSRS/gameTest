using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int CurrentItem = 0;
    public List<Weapon> Weapons = new();
    public Weapon ActiveItem;
    [SerializeField]
    private Transform playerLookingTransform;
    public static PlayerData Instance;
    private void Awake()
    {
        Instance = this;
        if (playerLookingTransform == null)
        {
            // should be safe under current architecture 06/29/25
            playerLookingTransform = Camera.main.transform;
        }
        ActiveItem = Weapons[CurrentItem];
    }
    public void SummonCurrentItem(bool inFPV)
    {
        ActiveItem.Ready(playerLookingTransform);
    }
    public void DespawnCurrentItem(bool inFPV)
    {
        ActiveItem.Despawn();
    }
    public void TossCurrentItem()
    {
        Weapons[CurrentItem].Toss();
    }
    public void ReloadCurrentItem()
    {
        Weapons[CurrentItem].Reload();
    }
    public void SwapCurrentItem()
    {
        CurrentItem = CurrentItem == 0 ? 1 : 0;
        ActiveItem = Weapons[CurrentItem];
    }
}
