using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObjectData : MonoBehaviour
{
    [SerializeField] public Vector2Int footprintSize;
    [SerializeField] public List<Material> materials;
}
