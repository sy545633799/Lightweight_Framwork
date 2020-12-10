// ========================================================
// xuzheng
// time：2020-08-05 17:28:25
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum HeroBodyPartsType
    {
        /* 躯干 */
        SPINE = 0,

        /* 全身 */
        MESH,

        /* 左手 */
        LEFT_HAND,

        /* 血条和名字 */
        HP_NAME,

        /* 近战打击点 */
        NEAR_HIT,

        /* 近战打击点(boss) */
        NEAR_HIT_BOSS,

        /* 武器网格对象 */
        WEAPON_MESH,

        /* 头部特效挂点 */
        TOPEFLINK = 100,

        /* 中间特效挂点 */
        CENTEREFLINK = 101,

        /* 底部特效挂点 */
        BOTTOMEFLINK = 102,

        /* 模型网格对象 */
        // BODY_MESH
    }

    public static class ModelUtils
	{
        public static Dictionary<HeroBodyPartsType, string> BodyParts = new Dictionary<HeroBodyPartsType, string>()
        {
            /* 躯干 */
            { HeroBodyPartsType.SPINE, "Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1"},

            /* 全身 */
            { HeroBodyPartsType.MESH, "mesh"},

            /* 左手 */
            { HeroBodyPartsType.LEFT_HAND, "Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand"},

            /* 血条和名字 */
            { HeroBodyPartsType.HP_NAME, "HP_name"},

            /* 近战打击点 */
            { HeroBodyPartsType.NEAR_HIT, "melee_hit"},

            /* 近战打击点(boss) */
            { HeroBodyPartsType.NEAR_HIT_BOSS, "melee_hit_b"},

            /* 武器网格对象 */
            { HeroBodyPartsType.WEAPON_MESH, "weapon"},

            /* 武器网格对象 */
            { HeroBodyPartsType.TOPEFLINK, "buff01"},

            /* 武器网格对象 */
            { HeroBodyPartsType.CENTEREFLINK, "buff02"},

            /* 武器网格对象 */
            { HeroBodyPartsType.BOTTOMEFLINK, "buff03"},

            /* 模型网格对象 */
            // { HeroBodyPartsType.BODY_MESH, "body"},
        };

        public static T FindBodyParts<T>(HeroBodyPartsType bodyPart, Transform heroModel) where T : Object
        {
            Transform _transPart = heroModel.Find(BodyParts[bodyPart]);
            if (_transPart == null)
            {
                Debug.LogError($"挂点没找到 : [{bodyPart}]");
                return null;
            }
            if (typeof(T) == typeof(GameObject))
                return _transPart.gameObject as T;
            else if (typeof(T) == typeof(Transform))
                return _transPart as T;
            else
            {
                T _component = _transPart.GetComponent<T>();
                if (_component == null)
                    Debug.LogError($"挂点没找到 : [{bodyPart}]");
                return _component;
            }
        }

        public static T FindBodyParts<T>(HeroBodyPartsType bodyPart, GameObject heroModel) where T : Object
            => FindBodyParts<T>(bodyPart, heroModel.transform);

        public static Transform FindBodyParts(int bodyPart, Transform heroModel)
            => FindBodyParts<Transform>((HeroBodyPartsType)bodyPart, heroModel);

        public static Transform FindBodyParts(int bodyPart, GameObject heroModel)
            => FindBodyParts<Transform>((HeroBodyPartsType)bodyPart, heroModel);

        public static void SetLayerWithChildren(GameObject go, string layerName)
            => SetLayerWithChildren(go, LayerMask.NameToLayer(layerName));

        public static void SetLayerWithChildren(GameObject go, int layer)
        {
            go.gameObject.layer = layer;
            if (go.transform.childCount > 0)
            {
                for (int i = 0; i < go.transform.childCount; i++)
                    SetLayerWithChildren(go.transform.GetChild(i).gameObject, layer);
            }
        }
    }
}
