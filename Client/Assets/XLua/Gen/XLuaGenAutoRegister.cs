#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;
using System.Collections.Generic;
using System.Reflection;


namespace XLua.CSObjectWrap
{
    public class XLua_Gen_Initer_Register__
	{
        
        
        static void wrapInit0(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(XluaTest), XluaTestWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.IO.BinaryWriter), SystemIOBinaryWriterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(XLua.BinaryWriterExtentions), XLuaBinaryWriterExtentionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(XLua.UnityEngineObjectExtention), XLuaUnityEngineObjectExtentionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Object), UnityEngineObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(XLua.XLuaHelper), XLuaXLuaHelperWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Component), UnityEngineComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Array), SystemArrayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.IList), SystemCollectionsIListWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.IDictionary), SystemCollectionsIDictionaryWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Activator), SystemActivatorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Type), SystemTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Reflection.BindingFlags), SystemReflectionBindingFlagsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.LoggerHelper), GameLoggerHelperWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.GameUtility), GameGameUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.XLuaManager), GameXLuaManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.LuaBehaviour), GameLuaBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(object), SystemObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Ray2D), UnityEngineRay2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.GameObject), UnityEngineGameObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Behaviour), UnityEngineBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Transform), UnityEngineTransformWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Resources), UnityEngineResourcesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.TextAsset), UnityEngineTextAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Keyframe), UnityEngineKeyframeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimationCurve), UnityEngineAnimationCurveWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimationClip), UnityEngineAnimationClipWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.MonoBehaviour), UnityEngineMonoBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem), UnityEngineParticleSystemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SkinnedMeshRenderer), UnityEngineSkinnedMeshRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Renderer), UnityEngineRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<int>), SystemCollectionsGenericList_1_SystemInt32_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Debug), UnityEngineDebugWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Events.UnityEvent), UnityEngineEventsUnityEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Bounds), UnityEngineBoundsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Color), UnityEngineColorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LayerMask), UnityEngineLayerMaskWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Mathf), UnityEngineMathfWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Plane), UnityEnginePlaneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Quaternion), UnityEngineQuaternionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Ray), UnityEngineRayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RaycastHit), UnityEngineRaycastHitWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Time), UnityEngineTimeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Touch), UnityEngineTouchWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.TouchPhase), UnityEngineTouchPhaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector2), UnityEngineVector2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector3), UnityEngineVector3Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector4), UnityEngineVector4Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RenderMode), UnityEngineRenderModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Canvas), UnityEngineCanvasWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Rect), UnityEngineRectWrap.__Register);
        
        }
        
        static void wrapInit1(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(UnityEngine.RectTransform), UnityEngineRectTransformWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RectOffset), UnityEngineRectOffsetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Sprite), UnityEngineSpriteWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.CanvasScaler), UnityEngineUICanvasScalerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.CanvasScaler.ScaleMode), UnityEngineUICanvasScalerScaleModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.CanvasScaler.ScreenMatchMode), UnityEngineUICanvasScalerScreenMatchModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.GraphicRaycaster), UnityEngineUIGraphicRaycasterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Image), UnityEngineUIImageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.ScrollRect), UnityEngineUIScrollRectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Scrollbar), UnityEngineUIScrollbarWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.ToggleGroup), UnityEngineUIToggleGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Button.ButtonClickedEvent), UnityEngineUIButtonButtonClickedEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.ScrollRect.ScrollRectEvent), UnityEngineUIScrollRectScrollRectEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.GridLayoutGroup), UnityEngineUIGridLayoutGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.ContentSizeFitter), UnityEngineUIContentSizeFitterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Slider), UnityEngineUISliderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ResourceRequest), UnityEngineResourceRequestWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SceneManagement.SceneManager), UnityEngineSceneManagementSceneManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Application), UnityEngineApplicationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RuntimePlatform), UnityEngineRuntimePlatformWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PlayerPrefs), UnityEnginePlayerPrefsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.GC), SystemGCWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.UIText), GameUITextWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.UIButton), GameUIButtonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.UIToggle), GameUIToggleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.UIInput), GameUIInputWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.UIDropDown), GameUIDropDownWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.UIImage), GameUIImageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.UIJoyStick), GameUIJoyStickWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.NameComp), GameNameCompWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.HUDComp), GameHUDCompWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.AnimComp), GameAnimCompWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.MoveComp), GameMoveCompWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.RotateComp), GameRotateCompWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.NavComp), GameNavCompWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.SyncStatusComp), GameSyncStatusCompWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.SyncTransComp), GameSyncTransCompWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.MainCamera), GameMainCameraWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.InputManager), GameInputManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.LoadManager), GameLoadManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.ResourceManager), GameResourceManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.AtlasManager), GameAtlasManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.TcpManager), GameTcpManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.MapManager), GameMapManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.EntityBehaviorManager), GameEntityBehaviorManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.UIUtil), GameUIUtilWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.UIListener), GameUIListenerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.CommonUtil), GameCommonUtilWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.GameSettings), GameGameSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.CameraDrag), GameCameraDragWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.EventExtension), GameEventExtensionWrap.__Register);
        
        }
        
        static void wrapInit2(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(Game.UIExtensions), GameUIExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Game.TransformExtension), GameTransformExtensionWrap.__Register);
        
        
        
        }
        
        static void Init(LuaEnv luaenv, ObjectTranslator translator)
        {
            
            wrapInit0(luaenv, translator);
            
            wrapInit1(luaenv, translator);
            
            wrapInit2(luaenv, translator);
            
            
            translator.AddInterfaceBridgeCreator(typeof(System.Collections.IEnumerator), SystemCollectionsIEnumeratorBridge.__Create);
            
        }
        
	    static XLua_Gen_Initer_Register__()
        {
		    XLua.LuaEnv.AddIniter(Init);
		}
		
		
	}
	
}
namespace XLua
{
	public partial class ObjectTranslator
	{
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ s_gen_reg_dumb_obj = new XLua.CSObjectWrap.XLua_Gen_Initer_Register__();
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ gen_reg_dumb_obj {get{return s_gen_reg_dumb_obj;}}
	}
	
	internal partial class InternalGlobals
    {
	    
	    static InternalGlobals()
		{
		    extensionMethodMap = new Dictionary<Type, IEnumerable<MethodInfo>>()
			{
			    
			};
			
			genTryArrayGetPtr = StaticLuaCallbacks.__tryArrayGet;
            genTryArraySetPtr = StaticLuaCallbacks.__tryArraySet;
		}
	}
}
