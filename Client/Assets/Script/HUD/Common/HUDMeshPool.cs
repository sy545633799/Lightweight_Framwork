// ========================================================
// des：
// author: 
// time：2020-12-24 16:48:02
// version：1.0
// ========================================================

// xuzheng 2020/11/6
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class HUDMeshPool
    {
        private static Stack<Mesh> m_meshs = new Stack<Mesh>();

        public static Mesh CreateMesh()
        {
            if (m_meshs.Count > 0)
            {
                return m_meshs.Pop();
            }
            return new Mesh();
        }

        public static void RecoveryMesh(Mesh mesh)
        {
            mesh.Clear();
            m_meshs.Push(mesh);
        }

        public static void Dispose()
        {
            while (m_meshs.Count > 0)
            {
                GameObject.DestroyImmediate(m_meshs.Pop());
            }
            m_meshs.Clear();
        }
    }
}
