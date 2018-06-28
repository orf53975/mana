using mana.Foundation;

namespace xxd.sync
{
	/// <summary>
	/// 当前场景快照数据
	/// </summary>
    public class BattleSnapData : DataObject
    {

		#region ---flags---
		public const byte __FLAG_TYPE = 0;
		public const byte __FLAG_UUID = 1;
		public const byte __FLAG_UNITS = 2;
		public const long __MASK_ALL_VALUE = 0x7;

		#endregion

		#region ---mask---
		public readonly Mask mask = new Mask();
		#endregion

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

		#region ---uuid---
		private long _uuid = 0;
		public long uuid
		{
			get
			{
				return _uuid;
			}
			set
			{
				if(this._uuid != value)
				{
					this._uuid = value;
					this.mask.AddFlag(__FLAG_UUID);
				}
			}
		}

		public bool HasUuid()
		{
			return this.mask.CheckFlag(__FLAG_UUID);
		}
		#endregion //uuid

		#region ---units---
		private UnitInfo[] _units = null;
		public UnitInfo[] units
		{
			get
			{
				return _units;
			}
			set
			{
				if(this._units != value)
				{
					this._units = value;
					this.mask.AddFlag(__FLAG_UNITS);
				}
			}
		}

		public bool HasUnits()
		{
			return this.mask.CheckFlag(__FLAG_UNITS);
		}
		#endregion //units
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {
			if (mask.CheckFlag(__FLAG_TYPE))
			{
				bw.WriteByte(_type);
			}
			if (mask.CheckFlag(__FLAG_UUID))
			{
				bw.WriteLong(_uuid);
			}
			if (mask.CheckFlag(__FLAG_UNITS))
			{
				bw.WriteArray(_units);
			}
        }
		#endregion

		#region ---Decode---
		public void Decode(IReadableBuffer br)
		{
			this.mask.Decode(br);
			if (HasType())
			{
				_type = br.ReadByte();
			}
			if (HasUuid())
			{
				_uuid = br.ReadLong();
			}
			if (HasUnits())
			{
				_units = br.ReadArray<UnitInfo>();
			}
		}
		#endregion

		#region ---Clone---
		public BattleSnapData Clone()
		{            
			var _clone = ObjectCache.Get<BattleSnapData>();
			_clone._type = this._type;
			_clone._uuid = this._uuid;
			_clone._units = this._units;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_type = 0;
			_uuid = 0;
			if(_units != null)
			{
				_units.ReleaseToCache();
                _units = null;
			}
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("BattleSnapData{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasType())
			{
				sb.Append(",\r\n").Append(curIndent).Append("type = ");
				sb.Append(type);
			}
			if(HasUuid())
			{
				sb.Append(",\r\n").Append(curIndent).Append("uuid = ");
				sb.Append(uuid);
			}
			if(HasUnits())
			{
				sb.Append(",\r\n").Append(curIndent).Append("units = ");
				sb.Append(units == null ? "null" : units.ToFormatString(curIndent));
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