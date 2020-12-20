// ========================================================
// des：同步移动结构, TODO：优化double为float
// author: shenyi
// time：2020-12-18 09:47:30
// version：1.0
// ========================================================

using Sproto;
using System.Collections.Generic;

namespace Game
{
	
	public class s2c_sync_trans : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<long, aoi_trans> _status;
		public Dictionary<long, aoi_trans> status
		{
			get { return _status; }
			set { base.has_field.set_field(0, true); _status = value; }
		}
		public bool HasStatus
		{
			get { return base.has_field.has_field(0); }
		}

		public s2c_sync_trans() : base(max_field_count) { }

		public s2c_sync_trans(byte[] buffer) : base(max_field_count, buffer)
		{
			this.decode();
		}

		public override int encode(SprotoStream stream)
		{
			base.serialize.open(stream);
			if (base.has_field.has_field(0))
			{
				base.serialize.write_obj(this.status, 0);
			}

			return base.serialize.close();
		}

		protected override void decode()
		{
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag()))
			{
				switch (tag)
				{
					case 0:
						this.status = base.deserialize.read_map<long, aoi_trans>(v => v.aoiId);
						break;
				}
			}
		}
	}
}
