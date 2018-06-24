using mana.Foundation;

namespace xxd.sync.opration
{
    public class MoveRequest : DataObject
    {

		#region ---flags---
		public const byte __FLAG_UNITID = 0;
		public const byte __FLAG_TYPE = 1;
		public const byte __FLAG_X = 2;
		public const byte __FLAG_Y = 3;
		public const byte __FLAG_Z = 4;
		public const byte __FLAG_FACETO = 5;
		public const long __MASK_ALL_VALUE = 0x3f;

		#endregion
		#region ---constants---
		public const byte type_start = 1;
		public const byte type_moving = 2;
		public const byte type_end = 3;
		public const byte type_other = 4;
		#endregion

		#region ---mask---
		public readonly Mask mask = new Mask();
		#endregion

		#region ---unitId---
		private int _unitId = 0;
		public int unitId
		{
			get
			{
				return _unitId;
			}
			set
			{
				if(this._unitId != value)
				{
					this._unitId = value;
					this.mask.AddFlag(__FLAG_UNITID);
				}
			}
		}

		public bool HasUnitId()
		{
			return this.mask.CheckFlag(__FLAG_UNITID);
		}
		#endregion //unitId

		#region ---type---
		private byte _type = 0;
		public byte type
		{
			get
			{
				return _type;
			}
			set
			{
				if(this._type != value)
				{
					this._type = value;
					this.mask.AddFlag(__FLAG_TYPE);
				}
			}
		}

		public bool HasType()
		{
			return this.mask.CheckFlag(__FLAG_TYPE);
		}
		#endregion //type

		#region ---x---
		private float _x = 0.0f;
		public float x
		{
			get
			{
				return _x;
			}
			set
			{
				if(this._x != value)
				{
					this._x = value;
					this.mask.AddFlag(__FLAG_X);
				}
			}
		}

		public bool HasX()
		{
			return this.mask.CheckFlag(__FLAG_X);
		}
		#endregion //x

		#region ---y---
		private float _y = 0.0f;
		public float y
		{
			get
			{
				return _y;
			}
			set
			{
				if(this._y != value)
				{
					this._y = value;
					this.mask.AddFlag(__FLAG_Y);
				}
			}
		}

		public bool HasY()
		{
			return this.mask.CheckFlag(__FLAG_Y);
		}
		#endregion //y

		#region ---z---
		private float _z = 0.0f;
		public float z
		{
			get
			{
				return _z;
			}
			set
			{
				if(this._z != value)
				{
					this._z = value;
					this.mask.AddFlag(__FLAG_Z);
				}
			}
		}

		public bool HasZ()
		{
			return this.mask.CheckFlag(__FLAG_Z);
		}
		#endregion //z

		#region ---faceTo---
		private float _faceTo = 0.0f;
		public float faceTo
		{
			get
			{
				return _faceTo;
			}
			set
			{
				if(this._faceTo != value)
				{
					this._faceTo = value;
					this.mask.AddFlag(__FLAG_FACETO);
				}
			}
		}

		public bool HasFaceTo()
		{
			return this.mask.CheckFlag(__FLAG_FACETO);
		}
		#endregion //faceTo
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {
			if (mask.CheckFlag(__FLAG_UNITID))
			{
				bw.WriteInt(_unitId);
			}
			if (mask.CheckFlag(__FLAG_TYPE))
			{
				bw.WriteByte(_type);
			}
			if (mask.CheckFlag(__FLAG_X))
			{
				bw.WriteFloat(_x);
			}
			if (mask.CheckFlag(__FLAG_Y))
			{
				bw.WriteFloat(_y);
			}
			if (mask.CheckFlag(__FLAG_Z))
			{
				bw.WriteFloat(_z);
			}
			if (mask.CheckFlag(__FLAG_FACETO))
			{
				bw.WriteFloat(_faceTo);
			}
        }
		#endregion

		#region ---Decode---
		public void Decode(IReadableBuffer br)
		{
			this.mask.Decode(br);
			if (HasUnitId())
			{
				_unitId = br.ReadInt();
			}
			if (HasType())
			{
				_type = br.ReadByte();
			}
			if (HasX())
			{
				_x = br.ReadFloat();
			}
			if (HasY())
			{
				_y = br.ReadFloat();
			}
			if (HasZ())
			{
				_z = br.ReadFloat();
			}
			if (HasFaceTo())
			{
				_faceTo = br.ReadFloat();
			}
		}
		#endregion

		#region ---Clone---
		public MoveRequest Clone()
		{            
			var _clone = ObjectCache.Get<MoveRequest>();
			_clone._unitId = this._unitId;
			_clone._type = this._type;
			_clone._x = this._x;
			_clone._y = this._y;
			_clone._z = this._z;
			_clone._faceTo = this._faceTo;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_unitId = 0;
			_type = 0;
			_x = 0.0f;
			_y = 0.0f;
			_z = 0.0f;
			_faceTo = 0.0f;
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("MoveRequest{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasUnitId())
			{
				sb.Append(",\r\n").Append(curIndent).Append("unitId = ");
				sb.Append(unitId);
			}
			if(HasType())
			{
				sb.Append(",\r\n").Append(curIndent).Append("type = ");
				sb.Append(type);
			}
			if(HasX())
			{
				sb.Append(",\r\n").Append(curIndent).Append("x = ");
				sb.Append(x);
			}
			if(HasY())
			{
				sb.Append(",\r\n").Append(curIndent).Append("y = ");
				sb.Append(y);
			}
			if(HasZ())
			{
				sb.Append(",\r\n").Append(curIndent).Append("z = ");
				sb.Append(z);
			}
			if(HasFaceTo())
			{
				sb.Append(",\r\n").Append(curIndent).Append("faceTo = ");
				sb.Append(faceTo);
			}
			sb.Append("\r\n");
            sb.Append(newLineIndent).Append('}');
            return StringBuilderCache.GetAndRelease(sb);
        }
		#endregion

		#region ---ToString---
        public override string ToString()
        {
            return ToFormatString("");
        }
		#endregion
    }
}