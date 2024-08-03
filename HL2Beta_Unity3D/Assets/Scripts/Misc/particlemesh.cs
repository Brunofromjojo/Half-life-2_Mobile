using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particlemesh : MonoBehaviour
{
    public ParticleSystem particles;
    public SkinnedMeshRenderer renderr;
    private Mesh m;

    void Start()
    {
        m = new Mesh();
    }

    void LateUpdate()
    {
        renderr.BakeMesh(m);

        var sh = particles.shape;
        sh.mesh = m;
    }
}
