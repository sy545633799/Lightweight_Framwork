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
		private static int max_field_count = 4;

		private double _pos_x;
		public double pos_x
		{
			get { return _pos_x; }
			set { base.has_field.set_field(0, true); _pos_x = value; }
		}
		public bool HasPos_x
		{
			get { return base.has_field.has_field(0); }
		}

		private double _pos_y;
		public double pos_y
		{
			get { return _pos_y; }
			set { base.has_field.set_field(1, true); _pos_y = value; }
		}
		public bool HasPos_y
		{
			get { return base.has_field.has_field(1); }
		}

		private double _pos_z;
		public double pos_z
		{
			get { return _pos_z; }
			set { base.has_field.set_field(2, true); _pos_z = value; }
		}
		public bool HasPos_z
		{
			get { return base.has_field.has_field(2); }
		}

		private double _forward;
		public double forward
		{
			get { return _forward; }
			set { base.has_field.set_field(3, true); _forward = value; }
		}
		public bool HasForward_z
		{
			get { return base.has_field.has_field(3); }
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
				base.serialize.write_double(this.pos_x, 0);
			}

			if (base.has_field.has_field(1))
			{
				base.serialize.write_double(this.pos_y, 1);
			}

			if (base.has_field.has_field(2))
			{
				base.serialize.write_double(this.pos_z, 2);
			}

			if (base.has_field.has_field(3))
			{
				base.serialize.write_double(this.forward, 3);
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
						this.pos_x = base.deserialize.read_double();
						break;
					case 1:
						this.pos_y = base.deserialize.read_double();
						break;
					case 2:
						this.pos_z = base.deserialize.read_double();
						break;
					case 3:
						this.forward = base.deserialize.read_double();
						break;
					default:
						base.deserialize.read_unknow_data();
						break;
				}
			}
		}
	}
}
