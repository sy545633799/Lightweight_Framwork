using System.Collections.Generic;
using System;
using UnityEngine;
using XLua;
//using SuperScrollView;
using UnityEngine.Events;
using Game;

// namespace Game.Editor
// {
    public static class GenConfig
    {
        //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>() {
		// unity
		typeof(System.Object),
        typeof(UnityEngine.Object),
        typeof(Ray2D),
        typeof(GameObject),
        typeof(Component),
        typeof(Behaviour),
        typeof(Transform),
        typeof(Resources),
        typeof(TextAsset),
        typeof(Keyframe),
        typeof(AnimationCurve),
        typeof(AnimationClip),
        typeof(MonoBehaviour),
        typeof(ParticleSystem),
        typeof(SkinnedMeshRenderer),
        typeof(Renderer),
        typeof(WWW),
        typeof(List<int>),
        typeof(Debug),
        typeof(Delegate),
        typeof(UnityEvent),

        // unity结合lua，这部分导出很多功能在lua侧重新实现，没有实现的功能才会跑到cs侧
        typeof(Bounds),
        typeof(Color),
        typeof(LayerMask),
        typeof(Mathf),
        typeof(Plane),
        typeof(Quaternion),
        typeof(Ray),
        typeof(RaycastHit),
        typeof(Time),
        typeof(Touch),
        typeof(TouchPhase),
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        
        // 渲染
        typeof(RenderMode),
        
        // UGUI  
        typeof(UnityEngine.Canvas),
        typeof(UnityEngine.Rect),
        typeof(UnityEngine.RectTransform),
        typeof(UnityEngine.RectOffset),
        typeof(UnityEngine.Sprite),
        typeof(UnityEngine.UI.CanvasScaler),
        typeof(UnityEngine.UI.CanvasScaler.ScaleMode),
        typeof(UnityEngine.UI.CanvasScaler.ScreenMatchMode),
        typeof(UnityEngine.UI.GraphicRaycaster),
        typeof(UnityEngine.UI.Image),
        typeof(UnityEngine.UI.ScrollRect),
        typeof(UnityEngine.UI.Scrollbar),
        typeof(UnityEngine.UI.ToggleGroup),
        typeof(UnityEngine.UI.Button.ButtonClickedEvent),
        typeof(UnityEngine.UI.ScrollRect.ScrollRectEvent),
        typeof(UnityEngine.UI.GridLayoutGroup),
        typeof(UnityEngine.UI.ContentSizeFitter),
        typeof(UnityEngine.UI.Slider),

        // 场景、资源加载
        typeof(UnityEngine.Resources),
        typeof(UnityEngine.ResourceRequest),
        typeof(UnityEngine.SceneManagement.SceneManager),
        
        // 其它
        typeof(Application),
        typeof(RuntimePlatform),
        typeof(PlayerPrefs),
        typeof(GC),
        
		//ui
		//typeof(LoopListView),
  //      typeof(LoopListViewItem),
		//typeof(UIListView),
		//typeof(UIListViewItem),
		typeof(UIText),
        typeof(UIButton),
        typeof(UIToggle),
        typeof(UIInput),
        typeof(UIDropDown),
        typeof(UIImage),
        typeof(UIJoyStick),

		//manager
		typeof(InputManager),
        typeof(LoadManager),
        typeof(ResourceManager),
		typeof(AtlasManager),
		typeof(TcpManager),
        typeof(MapManager),
		typeof(EntityBehaviorManager),
		//adapt
		typeof(UIUtil),
        typeof(UIListener),
        typeof(CommonUtil),
		//typeof(GameUtil),

        typeof(GameSettings),


        //widget
        typeof(CameraDrag),

        //extensions
         typeof(EventExtension),
		 typeof(UIExtensions),
		 typeof(TransformExtension),
	};

        //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>() {
        typeof(Action),
        typeof(Action<int>),
        typeof(Action<bool>),
        typeof(Action<float>),
        typeof(Action<float, float>),
        typeof(Action<byte[]>),

        typeof(UnityEngine.Event),
        typeof(System.Collections.IEnumerator),
        //typeof(System.Func<LoopListView, int, LoopListViewItem>),

        typeof(UnityAction),
        typeof(UnityAction<bool>),
        typeof(UnityAction<int>),
        typeof(UnityAction<float>),
        typeof(UnityAction<float, float>),
        typeof(UnityAction<KeyCode>),
        typeof(UnityAction<Vector2>),
        typeof(UnityAction<Vector2, GameObject>),

        typeof(UIButton.UIButtonEvent),
        typeof(UIButton.DoubleClickEvent),
    };
    }
// }