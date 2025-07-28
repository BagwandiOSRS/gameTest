using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField] private int rifleMaxAmmo = 30;
    [SerializeField] private float rifleFireTime = 0.1f;
    [SerializeField] private float rifleReloadTime = 1.0f;

    [Header("Rifle Sounds")]
    [SerializeField] private AudioClip rifleReadySound;
    [SerializeField] private AudioClip rifleFireSound;
    [SerializeField] private AudioClip rifleLastFireSound;
    [SerializeField] private AudioClip rifleReloadSound;

    [Header("Camera Offset")]
    [SerializeField] private Vector3 rifleReloadPosition;
    [SerializeField] private Vector3 rifleReloadEuler;

    protected override int MaxAmmo => rifleMaxAmmo;
    protected override float FireTime => rifleFireTime;
    protected override float ReloadTime => rifleReloadTime;
    protected override AudioClip ReadyUpSound => rifleReadySound;
    protected override AudioClip FireSound => rifleFireSound;
    protected override AudioClip LastFireSound => rifleLastFireSound;
    protected override AudioClip ReloadSound => rifleReloadSound;
    public override Vector3 ReloadPosition => rifleReloadPosition;
    public override Vector3 ReloadEuler => rifleReloadEuler;
}