using System;
using UnityEngine;

namespace Game
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static T mInstance = null;
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = Activator.CreateInstance<T>();
                    if (mInstance != null)
                    {
                        (mInstance as Singleton<T>).Init();
                    }
                }

                return mInstance;
            }
        }

        public static void Release()
        {
            if (mInstance != null)
            {
                mInstance = (T)((object)null);
            }
        }

        public virtual void Init()
        {

        }

        public virtual void Dispose()
        {

        }
    }
}