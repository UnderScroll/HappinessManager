using System;
using Unity.Mathematics;
using UnityEngine;

namespace Builder
{
    public partial class Builder : MonoBehaviour
    {
        public void LoadLevel(Structure structure)
        {
            Structure = structure;
            InitPreviewer();
        }
    }
}
