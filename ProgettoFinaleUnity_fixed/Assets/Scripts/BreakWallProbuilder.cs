using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class BreakWallProbuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Test()
    {
        Mesh m = this.GetComponent<MeshFilter>().mesh;
        ProBuilderMesh m2 = ProBuilderMesh.Create();

    }
}
