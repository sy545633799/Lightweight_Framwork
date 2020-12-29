// ========================================================
// des：渲染血条，可能包括图片和数字
// author: shenyi
// time：2020-12-24 13:37:53
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class HUDBloodRender : HUDBaseRender
	{
		HUDMesh bloodMesh;
		HUDMesh shieldMesh;

		public void PushBlood()
		{
			Sprite bg = HUDManager.GetSpritesByName("slider_bg");
			PushSprite(bg, bg.rect.width, bg.rect.height, 0, 0);

			
			//	bloodMesh = PushSprite(HUDManager.GetSpritesByName("slider_yellow"), 156 * 0.5f - 10, 12 * 0.5f - 2, 0, 0);
			//	shieldMesh = PushSprite(HUDManager.GetSpritesByName("slider_white"), 156 * 0.5f - 10, 12 * 0.5f - 2, 0, 0);
		}

		//HUDMesh[] _angerMesh = new HUDMesh[3];
		//public void PushAnger()
		//{
		//	PushSprite(HUDManager.GetSpritesByName("anger_bg"), 44 * 0.5f, 64 * 0.5f, 0, 0);
		//	_angerMesh = new HUDMesh[3]
		//	{
		//		PushSprite(HUDManager.GetSpritesByName($"anger0"), 18* 0.5f, 20 * 0.5f, 0, 0),
		//		PushSprite(HUDManager.GetSpritesByName($"anger0"), 18* 0.5f, 20 * 0.5f, 0, 0),
		//		PushSprite(HUDManager.GetSpritesByName($"anger0"), 18* 0.5f, 20 * 0.5f, 0, 0),
		//	};
		//}

		public void SetNormalBlood(float pos, bool fillLiftToRight = true)
		{
			pos = Mathf.Clamp01(pos);
			if (fillLiftToRight)
			{
				bloodMesh.ResetFillLeftToRight(pos);
			}
			else
			{
				bloodMesh.ResetFillRightToLeft(1 - pos);
			}
		}

		public void SetNormalShield(float pos, bool fillLiftToRight = false)
		{
			pos = Mathf.Clamp01(pos);
			if (fillLiftToRight)
			{
				shieldMesh.ResetFillLeftToRight(pos);
			}
			else
			{
				shieldMesh.ResetFillRightToLeft(1 - pos);
			}
		}

		//public void SetAnger(int anger)
		//{
		//	anger = Mathf.Abs(anger);
		//	List<int> _anger = new List<int>();
		//	int _numberOfIndex = 0;
		//	do
		//	{
		//		_numberOfIndex = anger % 10;
		//		anger /= 10;
		//		_anger.Add(_numberOfIndex);
		//	} while (anger > 0);
		//	_anger.Reverse();
		//	float interval_X = -4;
		//	float _sum = _anger.Count * 18 + (_anger.Count - 1) * interval_X;
		//	for (int i = 0; i < _angerMesh.Length; i++)
		//	{
		//		if (i + 1 <= _anger.Count)
		//		{
		//			float _offset = (_anger.Count - (i + 1)) * (18 + interval_X) + 18 * 0.5f;
		//			_offset = _offset - _sum * 0.5f;
		//			_angerMesh[i].m_offsetX = -35;
		//			_angerMesh[i].m_offsetY = -5;
		//			_angerMesh[i].ReSet(_angerMesh[i].m_transform, HUDManager.Instance.GetSpritesByName($"anger{_anger[i]}"), 18 * 0.5f, 20 * 0.5f);
		//			_angerMesh[i].MoveOffsetX(-_offset);
		//		}
		//		else
		//		{
		//			_angerMesh[i].m_mesh.Clear();
		//		}
		//	}
		//}


		//#region Buff Icon 相关
		//private class BuffRenderModel
		//{
		//	public int m_addMax;
		//	public int m_addsCount;
		//	public HUDMesh m_mesh;
		//	public HUDMesh m_countMesh;
		//	public HUDMesh m_countBGMesh;
		//	public bool isadd { get => m_addMax > 1; }

		//	public void ShowOrHide(bool @switch)
		//	{
		//		m_mesh.m_hideOrShow = @switch;
		//		if (isadd)
		//		{
		//			m_countMesh.m_hideOrShow = @switch;
		//			m_countBGMesh.m_hideOrShow = @switch;
		//		}
		//	}
		//}

		///// 增益buff
		//private Dictionary<int, BuffRenderModel> m_buffIcons = new Dictionary<int, BuffRenderModel>();
		///// 减益buff
		//private Dictionary<int, BuffRenderModel> m_debuffIcons = new Dictionary<int, BuffRenderModel>();

		//public void PushBuffIcon(int buffID, int type, int addMax, string spriteName)
		//{
		//	if (type == 1)
		//	{
		//		int _count = m_buffIcons.Count;
		//		PushBuffHander(m_buffIcons, buffID, addMax, spriteName);
		//		RedrawIcons(m_buffIcons, -18, 0, 4);
		//		if (_count == 0 && m_buffIcons.Count > 0 && m_debuffIcons.Count > 0)
		//			RedrawIcons(m_debuffIcons, -18, 11, 4);
		//	}
		//	else
		//	{
		//		PushBuffHander(m_debuffIcons, buffID, addMax, spriteName);
		//		var _offsetY = m_buffIcons.Count > 0 ? 11 : 0;
		//		RedrawIcons(m_debuffIcons, -18, _offsetY, 4);
		//	}
		//}

		//public void RemoveBuffIcon(int buffID, int type)
		//{
		//	if (type == 1)
		//	{
		//		RemoveBuffHander(m_buffIcons, buffID);
		//		RedrawIcons(m_buffIcons, -18, 0, 4);
		//		if (m_buffIcons.Count <= 0 && m_debuffIcons.Count > 0)
		//			RedrawIcons(m_debuffIcons, -18, 0, 4);
		//	}
		//	else
		//	{
		//		RemoveBuffHander(m_debuffIcons, buffID);
		//		var _offsetY = m_buffIcons.Count > 0 ? 11 : 0;
		//		RedrawIcons(m_debuffIcons, -18, _offsetY, 4);
		//	}
		//}

		//private void RemoveBuffHander(Dictionary<int, BuffRenderModel> buffList, int buffID)
		//{
		//	if (buffList.ContainsKey(buffID))
		//	{
		//		if (buffList[buffID].isadd)
		//		{
		//			buffList[buffID].m_addsCount--;
		//			if (buffList[buffID].m_addsCount <= 0)
		//			{
		//				buffList[buffID].ShowOrHide(false);
		//				buffList.Remove(buffID);
		//			}
		//		}
		//		else
		//		{
		//			buffList[buffID].ShowOrHide(false);
		//			buffList.Remove(buffID);
		//		}
		//	}
		//}

		//private void PushBuffHander(Dictionary<int, BuffRenderModel> buffList, int buffID, int addMax, string spriteName)
		//{
		//	if (addMax <= 1 && buffList.ContainsKey(buffID))
		//	{
		//		return;
		//	}
		//	if (!buffList.ContainsKey(buffID))
		//	{
		//		buffList[buffID] = new BuffRenderModel();
		//		buffList[buffID].m_addsCount = 0;
		//		buffList[buffID].m_addMax = addMax;
		//		buffList[buffID].m_mesh = PushSprite(HUDManager.GetSpritesByName(spriteName), 10, 10, 0, 0);
		//		if (buffList[buffID].isadd)
		//		{
		//			buffList[buffID].m_countBGMesh = PushSprite(HUDManager.GetSpritesByName("buff_icon_bg"), 10, 10, 0, 0);
		//			buffList[buffID].m_countMesh = PushSprite(HUDManager.GetSpritesByName("anger0"), 4.5f, 5, 0, 0);
		//		}
		//	}
		//	if (buffList[buffID].isadd && buffList[buffID].m_addsCount < buffList[buffID].m_addMax)
		//	{
		//		buffList[buffID].m_addsCount++;
		//	}
		//}

		//private void RedrawIcons(Dictionary<int, BuffRenderModel> buffList, float offset_X, float offset_Y, int drawMaxCount)
		//{
		//	int _index = 0;
		//	int _offsetIndex = 0;
		//	foreach (var buffIconRederModel in buffList)
		//	{
		//		buffIconRederModel.Value.ShowOrHide(_index < drawMaxCount);
		//		buffIconRederModel.Value.m_mesh.SetOffsetX(offset_X + _offsetIndex);
		//		buffIconRederModel.Value.m_mesh.SetOffsetY(offset_Y);
		//		if (buffIconRederModel.Value.isadd)
		//		{
		//			buffIconRederModel.Value.m_countBGMesh.SetOffsetX(offset_X + _offsetIndex);
		//			buffIconRederModel.Value.m_countBGMesh.SetOffsetY(offset_Y);
		//			buffIconRederModel.Value.m_countMesh.SetOffsetX(offset_X + _offsetIndex);
		//			buffIconRederModel.Value.m_countMesh.SetOffsetY(offset_Y);
		//			buffIconRederModel.Value.m_countMesh.ResetSprite(HUDManager.GetSpritesByName($"anger{buffIconRederModel.Value.m_addsCount}"));
		//		}
		//		_offsetIndex += 11;
		//		_index++;
		//	}
		//}

		//#endregion

	}
}
