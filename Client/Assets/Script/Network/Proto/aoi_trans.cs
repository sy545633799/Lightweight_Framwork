// ========================================================
// des：
// author: 
// time：2020-12-20 17:09:00
// version：1.0
// ========================================================

using Sproto;

namespace Game {
	public class aoi_trans : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _aoiId;
		public long aoiId
		{
			get { return _aoiId; }
			set { base.has_field.set_field(0, true); _aoiId = value; }
		}

		private sync_trans _tarns;
		public sync_trans trans
		{
			get { return _tarns; }
			set { base.has_field.set_field(0, true); _tarns = value; }
		}


		public aoi_trans() : base(max_field_count) { }

		public aoi_trans(byte[] buffer) : base(max_field_count, buffer)
		{
			this.decode();
		}

		public override int encode(SprotoStream stream)
		{
			base.serialize.open(stream);
			if (base.has_field.has_field(0))
			{
				base.serialize.write_integer(this.aoiId, 0);
			}

			if (base.has_field.has_field(1))
			{
				base.serialize.write_obj(this.trans, 1);
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
						this.aoiId = base.deserialize.read_integer();
						break;
					case 1:
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
