using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected abstract int MaxAmmo { get; }
    protected abstract float FireTime { get; }
    protected abstract float ReloadTime { get; }
    protected abstract AudioClip ReadyUpSound { get; }
    protected abstract AudioClip FireSound { get; }
    protected abstract AudioClip LastFireSound { get; }
    protected abstract AudioClip ReloadSound { get; }
    public abstract Vector3 ReloadPosition { get; }
    public abstract Vector3 ReloadEuler { get; }

    protected int currentAmmo;
    protected AudioSource audioSource;
    protected Transform itemSocket;
    protected Transform toolGarage;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentAmmo = MaxAmmo;
    }

    public void Toss()
    {
        if (currentAmmo == 0) return;

        audioSource.clip = currentAmmo > 1 ? FireSound : LastFireSound;
        audioSource.Play();
        currentAmmo--;
    }

    public void Reload()
    {
        currentAmmo = MaxAmmo;
    }

    public void Ready(Transform player)
    {
        if (itemSocket == null)
        {
            itemSocket = player.GetChild(0);
        }

        transform.SetParent(itemSocket, false);
        transform.SetLocalPositionAndRotation(
            new Vector3(0.0f, -0.28f, 0.0f),
            Quaternion.identity
        );
    }

    public void Despawn()
    {
        transform.SetParent(null);
        transform.position = new Vector3(100.0f, 100.0f, 100.0f);
    }
}