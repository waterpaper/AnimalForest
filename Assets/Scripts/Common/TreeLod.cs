using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLod : MonoBehaviour
{
    LODGroup _lodGroup;
    MtreeComponent _component;
    int _beforeLevel;

    // Start is called before the first frame update
    void Start()
    {
        _lodGroup = GetComponent<LODGroup>();
        _component = GetComponent<MtreeComponent>();
        _beforeLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        LevelofDetailTree(1);
    }

    void LevelofDetailTree(int level)
    {
        if(_beforeLevel != level)
        {
            _component.Lod = level;
            _component.tree.Simplify(_component.simplifyAngleThreshold[level], _component.simplifyRadiusThreshold[level]);
            _beforeLevel =level;
        }
    }
}
