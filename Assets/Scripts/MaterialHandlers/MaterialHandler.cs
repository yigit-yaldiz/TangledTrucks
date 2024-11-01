using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("Material handlers base class")]
public class MaterialHandler : MonoBehaviour
{
    protected Renderer _renderer;

    public virtual void ChangeMaterial(Material material)
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = material;
    }
}
