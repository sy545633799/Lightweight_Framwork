// ========================================================
// des: 每一段Shot的属性
// author: shenyi'
// time：2020-12-30 10:45:41
// version：1.0
// ========================================================

namespace Game
{
	/// <summary>
	/// 技能作用目标
	/// </summary>
	public enum SKillShotTargetType
	{
		Enemy = 1 << 0,
		Friend = 1 << 1,
		Self = 1 << 2,
	}

	/// <summary>
	/// 选择目标
	/// </summary>
	public enum SkillShotSelectType
	{
		/// <summary>
		/// 单个目标
		/// </summary>
		Single = 1,
		/// <summary>
		/// 多个目标(带SkillShape)
		/// </summary>
		Multi = 2,
		/// <summary>
		/// 选择目标(带带SkillShape, SkillSelector)
		/// </summary>
		Selector = 3,
	}

	/// <summary>
	/// 伤害类型
	/// </summary>
	public enum SkillShotCastType
	{
		/// <summary>
		/// 瞬发
		/// </summary>
		Once,
		/// <summary>
		/// 持续(包括Bullet, 带SkillPath)
		/// </summary>
		Continuous,
	}


}
