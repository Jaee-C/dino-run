using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ObstacleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathEffect;

    private MeshRenderer _renderer;

    private void Awake()
    {
        this._renderer = gameObject.GetComponent<MeshRenderer>();
    }

    public void Kill()
    {
        Debug.Log("Hello");
        var particles = Instantiate(this.deathEffect);
        particles.transform.position = transform.position;
    }
}