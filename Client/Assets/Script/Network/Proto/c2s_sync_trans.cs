// ========================================================
// des：同步移动结构, TODO：优化double为float
// author: shenyi
// time：2020-12-18 09:47:30
// version：1.0
// ========================================================


using Sproto;

namespace Game {
	public class c2s_sync_trans : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private sync_trans _trans;
		public sync_trans trans
		{
			get { return _trans; }
			set { base.has_field.set_field(0, true); _trans = value; }
		}

		public c2s_sync_trans() : base(max_field_count) { }

		public c2s_sync_trans(byte[] buffer) : base(max_field_count, buffer)
		{
			this.decode();
		}

		public override int encode(SprotoStream stream)
		{
			base.serialize.open(stream);

			if (base.has_field.has_field(0))
			{
				base.serialize.write_obj(this.trans, 0);
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
						this.trans = base.deserialize.read_obj<sync_trans>();
						break;
					default:
						base.deserialize.read_unknow_data();
						break;
				}
			}
		}
	}
}
