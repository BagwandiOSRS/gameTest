using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int shotgunMaxAmmo = 4;
    [SerializeField] private float shotgunFireTime = 0.15f;
    [SerializeField] private float shotgunReloadTime = 0.8f;

    [Header("Shotgun Sounds")]
    [SerializeField] private AudioClip shotgunReadySound;
    [SerializeField] private AudioClip shotgunFireSound;
    [SerializeField] private AudioClip shotgunLastFireSound;
    [SerializeField] private AudioClip shotgunReloadSound;

    [Header("Camera Offset")]
    [SerializeField] private Vector3 shotgunReloadPosition;
    [SerializeField] private Vector3 shotgunReloadEuler;

    protected override int MaxAmmo => shotgunMaxAmmo;
    protected override float FireTime => shotgunFireTime;
    protected override float ReloadTime => shotgunReloadTime;
    protected override AudioClip ReadyUpSound => shotgunReadySound;
    protected override AudioClip FireSound => shotgunFireSound;
    protected override AudioClip LastFireSound => shotgunLastFireSound;
    protected override AudioClip ReloadSound => shotgunReloadSound;
    public override Vector3 ReloadPosition => shotgunReloadPosition;
    public override Vector3 ReloadEuler => shotgunReloadEuler;
}