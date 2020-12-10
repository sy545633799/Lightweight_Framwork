/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;

namespace Game
{
    [System.Serializable]
    public class Injection
    {
        public string name;
        public GameObject value;
    }

    [LuaCallCSharp]
    public class LuaBehaviour : MonoBehaviour
    {
        //lua代码，在腾讯实例中是在Hierarchy面板赋值的
        public TextAsset luaScript;
        public Injection[] injections;

        internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;//1 second 

        private Action luaStart;
        private Action luaUpdate;
        private Action luaOnDestroy;

        private LuaTable scriptEnv;

        void Awake()
        {
            //获取LuaTable，要用luaEnv.NewTable()，这里会做一些排序，共用虚拟机等操作
            scriptEnv = luaEnv.NewTable();
            LuaTable meta = luaEnv.NewTable();
            //key,value的方式，虚拟机环境对应的key值一定要是这个“__index”，
            //在xlua的底层，获取LuaTable所属虚拟机的环境是get的时候，用的key是这个名字，所以不能改
            meta.Set("__index", luaEnv.Global);
            //将有虚拟机和全局环境的table绑定成他自己的metatable
            scriptEnv.SetMetaTable(meta);
            //值已经传递过去了，就释放他
            meta.Dispose();
            //这里的"self"和上面的"__index"是一个道理啦。将c#脚本绑定到LuaTable
            scriptEnv.Set("self", this);
            //在腾讯示例中，Hierarchy面板赋值的，在这里应该是约定名字和子物体的对应，绑定到luatable
            foreach (var injection in injections)
            {
                scriptEnv.Set(injection.name, injection.value);
            }
            //执行lua语句，三个参数的意思分别是lua代码，lua代码在c#里的代号，lua代码在lua虚拟机里的代号
            luaEnv.DoString(luaScript.text, "LuaBehaviour", scriptEnv);
            //xlua搞了这么久，也就是为了最后这几个锤子，c#调用lua里的方法。
            //总结起来一句话，通过luatable这个类来完成c#调用Lua
            //怎样完成这一步呢？就是获取luatable对象，配置lua虚拟机，配置虚拟机环境，绑定c#代码，最后执行lua语句
            Action luaAwake = scriptEnv.Get<Action>("awake");
            scriptEnv.Get("start", out luaStart);
            scriptEnv.Get("update", out luaUpdate);
            scriptEnv.Get("ondestroy", out luaOnDestroy);
            if (luaAwake != null)
            {
                luaAwake();
            }
        }

        // Use this for initialization
        void Start()
        {
            if (luaStart != null)
            {
                luaStart();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (luaUpdate != null)
            {
                luaUpdate();
            }
            if (Time.time - LuaBehaviour.lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                LuaBehaviour.lastGCTime = Time.time;
            }
        }

        void OnDestroy()
        {
            if (luaOnDestroy != null)
            {
                luaOnDestroy();
            }
            luaOnDestroy = null;
            luaUpdate = null;
            luaStart = null;
            scriptEnv.Dispose();
            injections = null;
        }
    }
}