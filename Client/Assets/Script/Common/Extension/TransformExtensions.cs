using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// Transform的扩展方法
    /// </summary>
    public static class TransformExtension
    {
        public static void SetPositionX(this Transform t, float newX)
        {
            t.position = new Vector3(newX, t.position.y, t.position.z);
        }

        public static void SetPositionY(this Transform t, float newY)
        {
            t.position = new Vector3(t.position.x, newY, t.position.z);
        }
        public static void SetPositionZ(this Transform t, float newZ)
        {
            t.position = new Vector3(t.position.x, t.position.y, newZ);
        }
        public static void SetLocalPosX(this Transform pTran, float pScale)
        {
            pTran.localPosition = new Vector3(pScale, pTran.localPosition.y, pTran.localPosition.z);
        }

        public static void SetLocalPosY(this Transform pTran, float pScale)
        {
            pTran.localPosition = new Vector3(pTran.localPosition.x, pScale, pTran.localPosition.z);
        }

        public static void SetLocalPosZ(this Transform pTran, float pScale)
        {
            pTran.localPosition = new Vector3(pTran.localPosition.x, pTran.localPosition.y, pScale);
        }

        public static void SetLocalPos(this Transform pTran, float x, float y, float z)
        {
            pTran.localPosition = new Vector3(x, y, z);
        }

        public static void SetLocalPositionYAdd(this Transform t, float newY)
        {
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y + newY, t.localPosition.z);
        }
        public static void SetLocalRotX(this Transform pTran, float pScale)
        {
            pTran.localRotation =
                Quaternion.Euler(new Vector3(pScale, pTran.localRotation.eulerAngles.y,
                    pTran.localRotation.eulerAngles.z));
        }

        public static void SetLocalRotY(this Transform pTran, float pScale)
        {
            pTran.localRotation =
                Quaternion.Euler(new Vector3(pTran.localRotation.eulerAngles.x, pScale,
                    pTran.localRotation.eulerAngles.z));
        }

        public static void SetLocalRotZ(this Transform pTran, float pScale)
        {
            pTran.localRotation =
                Quaternion.Euler(new Vector3(pTran.localRotation.eulerAngles.x, pTran.localRotation.eulerAngles.y,
                    pScale));
        }

        public static void SetLocalRot(this Transform pTran, float x, float y, float z)
        {
            pTran.localRotation = Quaternion.Euler(new Vector3(x, y, z));
        }

        public static void SetLocalScaleX(this Transform pTran, float pScale)
        {
            pTran.localScale = new Vector3(pScale, pTran.localScale.y, pTran.localScale.z);
        }

        public static void SetLocalScaleY(this Transform pTran, float pScale)
        {
            pTran.localScale = new Vector3(pTran.localScale.x, pScale, pTran.localScale.z);
        }

        public static void SetLocalScaleZ(this Transform pTran, float pScale)
        {
            pTran.localScale = new Vector3(pTran.localScale.x, pTran.localScale.y, pScale);
        }
        public static void SetLocalScaleZero(this Transform t)
        {
            t.localScale = Vector3.zero;
        }
        public static void ZomeLocalScaleX(this Transform pTran, float pScale)
        {
            pTran.SetLocalScaleX(pTran.localScale.x * pScale);
        }

        public static void ZomeLocalScaleY(this Transform pTran, float pScale)
        {
            pTran.SetLocalScaleY(pTran.localScale.y * pScale);
        }

        public static void ZomeLocalScaleZ(this Transform pTran, float pScale)
        {
            pTran.SetLocalScaleZ(pTran.localScale.z * pScale);
        }

        public static void SetLocalScale(this Transform pTran, float x, float y, float z)
        {
            pTran.localScale = new Vector3(x, y, z);
        }

        public static void ZomeLocalScale(this Transform pTran, float x, float y, float z)
        {
            pTran.localScale = new Vector3(pTran.localScale.x * x, pTran.localScale.y * y, pTran.localScale.z * z);
            ;
        }

        public static void ZomeLocalScale(this Transform pTran, float pScale)
        {
            pTran.localScale *= pScale;
        }

        public static void SetParentAndPos(this Transform pTran, Vector3 pLocalPos, Transform pParent = null)
        {
            SetParentAndTrans(pTran, pParent);
            pTran.transform.localPosition = pLocalPos;

        }

        public static void SetParentAndRot(this Transform pTran, Vector3 pLocalRot, Transform pParent = null)
        {
            SetParentAndTrans(pTran, pParent);
            pTran.transform.localRotation = Quaternion.Euler(pLocalRot);
        }

        public static void SetParentAndScale(this Transform pTran, Vector3 pLocalScale, Transform pParent = null)
        {
            SetParentAndTrans(pTran, pParent);
            pTran.transform.localScale = pLocalScale;
        }

        public static void SetParentAndTrans(this Transform pTran, Transform pParent = null)
        {
            if (pParent != null)
            {
                pTran.transform.parent = pParent;
            }
        }

        public static void SetParentAndTrans(this Transform pTran, Vector3 pLocalPos, Vector3 pLocalRot,
            Vector3 pLocalScale,
            Transform pParent = null)
        {
            pTran.transform.parent = pParent;
            pTran.transform.localPosition = pLocalPos;
            pTran.transform.localRotation = Quaternion.Euler(pLocalRot);
            pTran.transform.localScale = pLocalScale;
        }

		public static void SetParentAndReset(this Transform parent, Transform child)
		{
			child.SetParent(parent.transform);
			child.localPosition = Vector3.zero;
			child.localRotation = Quaternion.identity;
			child.localScale = Vector3.one;
		}
		public static void SetActive(this Transform pTran, bool pFlag)
        {
            pTran.gameObject.SetActive(pFlag);
        }

        public static T AddComponent<T>(this Transform pTran) where T : MonoBehaviour
        {
            return pTran.gameObject.AddComponent<T>();
        }

        public static T AddCompIfNull<T>(this Transform pObj) where T : MonoBehaviour
        {
            T _com = pObj.GetComponent<T>();
            if (_com != null)
            {
                return _com;
            }
            else
            {
                return pObj.AddComponent<T>();
            }
        }


        /// <summary>
        /// 广度优先搜索遍历
        /// </summary>
        /// <param name="root"></param>
        /// <typeparam name="TP">遍历时调用的函数的参数的类型</typeparam>
        /// <typeparam name="TR">遍历时调用的函数的返回值的类型</typeparam>
        /// <param name="visitFunc">遍历时调用的函数
        /// <para>TR Function(Transform t, T para)</para>
        /// </param>
        /// <param name="para">遍历时调用的函数的第二个参数</param>
        /// <param name="failReturnValue">遍历时查找失败的返回值</param>
        /// <returns>遍历时调用的函数的返回值</returns>
        public static TR BFSVisit<TP, TR>(this Transform root, System.Func<Transform, TP, TR> visitFunc, TP para, TR failReturnValue = default(TR))
        {
            TR ret = visitFunc(root, para);
            if (ret != null && !ret.Equals(failReturnValue))
                return ret;
            Queue<Transform> parents = new Queue<Transform>();
            parents.Enqueue(root);
            while (parents.Count > 0)
            {
                Transform parent = parents.Dequeue();
                foreach (Transform child in parent)
                {
                    ret = visitFunc(child, para);
                    if (ret != null && !ret.Equals(failReturnValue))
                        return ret;
                    parents.Enqueue(child);
                }
            }
            return failReturnValue;
        }

        /// <summary>
        /// 深度优先搜索遍历
        /// </summary>
        /// <param name="root"></param>
        /// <typeparam name="TP">遍历时调用的函数的参数的类型</typeparam>
        /// <typeparam name="TR">遍历时调用的函数的返回值的类型</typeparam>
        /// <param name="visitFunc">遍历时调用的函数
        /// <para>TR Function(Transform t, T para)</para>
        /// </param>
        /// <param name="para">遍历时调用的函数的第二个参数</param>
        /// <param name="failReturnValue">遍历时查找失败的返回值</param>
        /// <returns>遍历时调用的函数的返回值</returns>
        public static TR DFSVisit<TP, TR>(this Transform root, System.Func<Transform, TP, TR> visitFunc, TP para, TR failReturnValue = default(TR))
        {
            Stack<Transform> parents = new Stack<Transform>();
            parents.Push(root);
            while (parents.Count > 0)
            {
                Transform parent = parents.Pop();
                TR ret = visitFunc(parent, para);
                if (ret != null && !ret.Equals(failReturnValue))
                    return ret;
                for (int i = parent.childCount - 1; i >= 0; i--)
                {
                    parents.Push(parent.GetChild(i));
                }
            }
            return failReturnValue;
        }

        /// <summary>
        /// 根据名字查找并返回子孙，广度优先搜索
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="childName">要查找的子孙的名字</param>
        /// <returns>要查找的子孙的Transform</returns>
        public static T FindComponent_BFS<T>(this Transform trans, string childName) where T : Component
        {
            var target = BFSVisit<string, Transform>(trans,
                (t, str) => { if (t.name.Equals(str)) return t; return null; },
                childName
            );

            if (target == null)
            {
                Debug.LogError(string.Format("cann't find child transform {0} in {1}", childName, trans.gameObject.name));
                return null;
            }

            T component = target.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError("Component is null, type = " + typeof(T).Name);
                return null;
            }
            return component;
        }

        /// <summary>
        /// 根据名字查找并返回子孙，广度优先搜索
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="tagName">要查找的子孙的名字</param>
        /// <returns>要查找的子孙的Transform</returns>
        public static Transform FindChild_ByTag(this Transform trans, string tagName)
        {
            var target = BFSVisit<string, Transform>(trans,
                (t, str) => { if (t.tag.Equals(str)) return t; return null; },
                tagName
            );

            if (target == null)
            {
                //Debug.LogError(string.Format("cann't find child transform {0} in {1}", tagName, trans.gameObject.name));
                return null;
            }

            return target;
        }

        /// <summary>
        /// 根据名字查找并返回子孙，深度优先搜索
        /// </summary>
        /// /// <param name="trans"></param>
        /// <param name="childName">要查找的子孙的名字</param>
        /// <returns>要查找的子孙的Transform</returns>
        public static T FindComponent_DFS<T>(this Transform trans, string childName) where T : Component
        {
            var target = DFSVisit<string, Transform>(trans,
                (t, str) => { if (t.name.Equals(str)) return t; return null; },
                childName
            );

            if (target == null)
            {
                Debug.LogError(string.Format("cann't find child transform {0} in {1}", childName, trans.gameObject.name));
                return null;
            }

            T component = target.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError("Component is null, type = " + typeof(T).Name);
                return null;
            }
            return component;
        }

        /// <summary>
        /// 根据名字在子对象中查找组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trans"></param>
        /// <param name="name"></param>
        /// /// <param name="reportError"></param>
        /// <returns></returns>
        public static T FindComponent<T>(this Transform trans, string name = null, bool reportError = true) where T : Component
        {
            Transform target;
            if (string.IsNullOrEmpty(name))
                target = trans;
            else
                target = trans.Find(name);

            if (target == null)
            {
                if (reportError)
                {
                    Debug.LogError("Transform is null, name = " + name);
                }

                return null;
            }

            T component = target.GetComponent<T>();
            if (component == null)
            {
                if (reportError)
                {
                    Debug.LogError("Component is null, type = " + typeof(T).Name);
                }

                return null;
            }

            return component;
        }

        /// <summary>
        /// 初始化物体的相对位置、旋转、缩放
        /// </summary>
        /// <param name="trans"></param>
        public static void InitTransformLocal(this Transform trans)
        {
            trans.localPosition = Vector3.zero;
            trans.localScale = Vector3.one;
            trans.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// 可以递归地查找所有子节点的某个T类型的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <param name="recursive"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static T[] FindComponentsInChildren<T>(this Transform transform, bool recursive = true, bool includeInactive = true) where T : Component
        {

            if (recursive)
            {
                var list = new List<T>();
                GetChildren(transform, includeInactive, ref list);
                return list.ToArray();
            }
            else
            {
                return transform.GetComponentsInChildren<T>(includeInactive);
            }
        }

        public static T GetComponentsInParent<T>(this Transform transform) where T : Component
        {
            if (transform == null)
            {
                return null;
            }
            if (transform.GetComponent<T>() != null)
            {
                return transform.GetComponent<T>();
            }
            if (transform.parent != null)
            {
                return transform.parent.GetComponentInParent<T>();
            }
            return null;
        }

        public static Transform GetChildByName(this Transform transform, string name, bool recursive = true, bool includeInactive = true)
        {
            Transform target;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child != null && (includeInactive || transform.gameObject.activeSelf))
                {
                    if (child.name == name)
                    {
                        return child;
                    }
                    else
                    {
                        target = child.GetChildByName(name);
                        if (target != null) return target;
                    }
                }
            }

            return null;
        }

        private static void GetChildren<T>(Transform t, bool includeInactive, ref System.Collections.Generic.List<T> list)
        {
            if (includeInactive || t.gameObject.activeSelf)
            {
                for (int i = 0; i < t.childCount; i++)
                {
                    if (t.GetChild(i) != null)
                    {
                        GetChildren(t.GetChild(i), includeInactive, ref list);
                    }
                }

                var comp = t.GetComponent<T>();
                if (comp != null)
                {
                    list.Add(comp);
                }
            }

        }

        public static void GetAllChildren(this Transform parent, ref List<Transform> children)
        {
            if (parent != null)
            {
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    Transform child = parent.transform.GetChild(i);
                    children.Add(child);
                    GetAllChildren(child, ref children);
                }
            }
        }

        public static void GetAllChildrenPath(this Transform parent, ref List<string> children, string parentPath = "")
        {
            if (parent != null)
            {
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    Transform child = parent.transform.GetChild(i);
                    string childPath = parentPath + child.name;
                    children.Add(childPath);
                    childPath += "/";
                    GetAllChildrenPath(child, ref children, childPath);
                }
            }
        }

        public static void GetAllChildrenWithPath(this Transform parent, ref Dictionary<Transform, string> children, string parentPath = "")
        {
            if (parent != null)
            {
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    Transform child = parent.transform.GetChild(i);
                    string childPath = parentPath + child.name;
                    children.Add(child, childPath);
                    childPath += "/";
                    GetAllChildrenWithPath(child, ref children, childPath);
                }
            }
        }

    }

}