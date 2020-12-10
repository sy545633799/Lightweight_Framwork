// ========================================================
// des：
// author: 
// time：2020-05-08 11:43:46
// version：1.0
// ========================================================

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Game {
//	public class InstallerTest : MonoInstaller
//	{
//		[HideInInspector]
//		[Inject]
//		public GameSettings GameSettings;

//		public override void InstallBindings()
//		{
//			//SignalBusInstaller.Install(Container);

//			//Container.DeclareSignal<LoadSceneSignal>();
//			//封装不够严密, 不用
//			//Container.BindSignal<LoadSceneSignal>()
//			//	.ToMethod<LaunchUI>(x => x.Close).FromResolve();

//			//Container.Bind<ITickable>().To<LaunchUI>().AsSingle().NonLazy();
//			//区别：BindInterfacesTo 只绑定当前类的接口到框架里面，类对外界隐藏的
//			//Container.BindInterfacesTo<LaunchUI>().AsSingle();
//			//Container.BindInterfacesAndSelfTo<LaunchUI>().AsSingle().NonLazy();


//			//Container.BindInstance(100).WhenInjectedInto<TestB>();
//			//Container.BindInstance(10);


//			//等同于Container.Bind<int>().FromInstance(10);
//			//Container.Bind<ITickable>().FromInstance(this);
//			//Container.BindInterfacesAndSelfTo<TestA>().AsSingle(); //必须加as
//			//Container.BindInterfacesAndSelfTo<TestB>().AsSingle();


//			Container.Bind<int>().FromInstance(10);
//			Container.Bind<ITickable>().To<TestA>().AsSingle();
//			Container.Bind<ITickable>().To<TestB>().AsSingle();
//			Container.BindInterfacesAndSelfTo<TestCollection>().AsSingle().NonLazy();
//		}

//		public async override void Start()
//		{

//			LoggerHelper.Instance.Startup();
//			//更新

//			//Xlua热修复
//			//XLuaManager.Instance.OnInit();
//			//XLuaManager.Instance.StartHotfix();
//			//XLuaManager.Instance.StartGame();


//			////每次都new一个新的实例
//			////container.Bind<DiTest>().AsTransient();
//			////每次绑定创建一个新的实例
//			//container.Bind<DiTest>().AsCached();
//			//container.Inject(this);
//			//diTest.Test();


//			//从factory中创建tick并没有执行
//			//Container.BindFactory<TestA, FactoryA>();
//			//var a = Container.Resolve<FactoryA>().Create();
//			//print(a.Num);

//			//Container.Bind<TestB>().FromFactory<FactoryB>();
//			//var b = Container.Resolve<TestB>();
//			////必须手动注入
//			//Container.Inject(b);
//			//print(b.Num);

//			//print(GameSettings.AssetBundleMode);


//		}

//		private void Update()
//		{
//			if (Input.GetMouseButtonDown(0))
//			{
//				SignalBus bus = Container.Resolve<SignalBus>();
//				bus.Fire(new LoadSceneStartSignal() { SceneName = "Test" });
//			}
//		}

//		public void Tick()
//		{
//			print("11111111");
//		}

//		public class TestA : ITickable
//		{
//			[Inject] public int Num;

//			public void Tick()
//			{
//				print("AAAAAAAAAAAA");
//			}
//		}

//		public class TestB : ITickable
//		{
//			[Inject] public int Num;

//			public void Tick()
//			{
//				print("bbbbbbbbbb");
//			}
//		}

//		public class FactoryA : PlaceholderFactory<TestA>
//		{
			
//		}

//		public class FactoryB : IFactory<TestB>
//		{
//			public TestB Create()
//			{
//				return new TestB();
//			}
//		}

//		public class TestCollection
//		{
//			public TestCollection(IEnumerable<ITickable> tickables)
//			{
//				print(new List<ITickable>(tickables).Count);
//			}
//		}

//	}
//}
