using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
namespace GPUAnim {
	public class InstancingBatch : IDisposable {
		GPUAnimGroup group;
		GPUAnim.MaterialProperty property;
		public static int MAXBATCHCOUNT = 1023; //固定缓存，避免每帧GC
		public Matrix4x4[] matrices = new Matrix4x4[MAXBATCHCOUNT];
		public ShadowCastingMode shadowCastingMode = ShadowCastingMode.Off;
		public bool receiveShadows = false;
		public int layer = 0;
		public int visibleCount = 0; //可见物体数量
		public bool isfull = false;
		public bool isValid = true;
		List<GPUAnimObj> objs = new List<GPUAnimObj> ();
		public InstancingBatch (GPUAnimGroup group) {
			this.group = group;
			property = new GPUAnim.MaterialProperty (MAXBATCHCOUNT);
		}
		public void AddObject (GPUAnimObj o) {
			if (o == null) return;
			objs.Add (o);
			o.manager = this;
			o.property = property;
			o._index = objs.Count - 1;
			if (o._isVisible) ShowObject (o);
			if (objs.Count >= MAXBATCHCOUNT) isfull = true;
		}
		public void HideObject (GPUAnimObj o) {
			int idx = o._index;
			SwapObj (o._index, visibleCount - 1);
			objs[idx]._needUpdateProperty = true;
			objs[idx]._needUpdateTransform = true;
			o._isVisible = false;
			visibleCount--;
			if (visibleCount == 0) {
				isValid = false;
				return; //不可见则不参与合批
			}
			if (!group.combineList.Contains (this) && visibleCount < MAXBATCHCOUNT / 2) group.combineList.Add (this);
		}
		public void ShowObject (GPUAnimObj o) {
			SwapObj (o._index, visibleCount);
			o._needUpdateProperty = true;
			o._needUpdateTransform = true;
			o._isVisible = true;
			visibleCount++;
			if (visibleCount >= MAXBATCHCOUNT / 2) group.combineList.Remove (this);
			isValid = true;
		}
		public void RemoveObject (GPUAnimObj o) {
			if (o == null || !objs.Contains (o)) return;
			if (o._isVisible) HideObject (o);
			SwapObj (o._index, objs.Count - 1);
			objs.RemoveAt (objs.Count - 1);
			isfull = false;
		}
		void SwapObj (int currentIdx, int dstIdx) { //数据本身不动，仅动指针
			if (currentIdx == dstIdx) return;
			var current = objs[currentIdx];
			var dst = objs[dstIdx];
			objs[currentIdx] = dst;
			objs[dstIdx] = current;
			current._index = dstIdx;
			dst._index = currentIdx;
		}
		public void DrawBatch () {
			property.UpdateProperty (); //need reset each frame
			for (int i = 0; i < visibleCount; i++) {
				if (objs[i]._needUpdateProperty) objs[i].UpdateProperty ();
				if (objs[i]._needUpdateTransform) objs[i].UpdateTransform ();
			}
			Graphics.DrawMeshInstanced (group.mesh, 0, group.material, matrices, visibleCount, property.block, shadowCastingMode, receiveShadows, layer);
		}
		public void CombineBatch (InstancingBatch batch) {
			if (MAXBATCHCOUNT - visibleCount < batch.visibleCount) return;
			GPUAnimObj visible = null;
			GPUAnimObj invisible = null;
			int n = batch.visibleCount;
			while (n > 0) {
				visible = batch.objs[0];
				if (visibleCount < objs.Count) invisible = objs[visibleCount];
				else invisible = null;
				batch.RemoveObject (visible);
				this.RemoveObject (invisible);
				visible._isVisible = true;
				AddObject (visible);
				batch.AddObject (invisible);
				n--;
			}
			batch.SetValid (false);
			group.combineList.Remove (this);
			group.combineList.Remove (batch);
		}
		public void SetValid (bool b) {
			isValid = b;
		}
		public void Dispose () {
			matrices = null;
			property = null;
			objs.Clear ();
		}
	}
}