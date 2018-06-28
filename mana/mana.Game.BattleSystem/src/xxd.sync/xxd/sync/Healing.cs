using mana.Foundation;

namespace xxd.sync
{
    public class Healing : DataObject
    {

		#region ---flags---
		public const byte __FLAG_UNITID = 0;
		public const byte __FLAG_BASEVALUE = 1;
		public const byte __FLAG_VALUE = 2;
		public const long __MASK_ALL_VALUE = 0x7;

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

		#region ---baseValue---
		private int _baseValue = 0;
		public int baseValue
		{
			get
			{
				return _baseValue;
			}
			set
			{
				if(this._baseValue != value)
				{
					this._baseValue = value;
					this.mask.AddFlag(__FLAG_BASEVALUE);
				}
			}
		}

		public bool HasBaseValue()
		{
			return this.mask.CheckFlag(__FLAG_BASEVALUE);
		}
		#endregion //baseValue

		#region ---value---
		private int _value = 0;
		public int value
		{
			get
			{
				return _value;
			}
			set
			{
				if(this._value != value)
				{
					this._value = value;
					this.mask.AddFlag(__FLAG_VALUE);
				}
			}
		}

		public bool HasValue()
		{
			return this.mask.CheckFlag(__FLAG_VALUE);
		}
		#endregion //value
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {
			if (mask.CheckFlag(__FLAG_UNITID))
			{
				bw.WriteInt(_unitId);
			}
			if (mask.CheckFlag(__FLAG_BASEVALUE))
			{
				bw.WriteInt(_baseValue);
			}
			if (mask.CheckFlag(__FLAG_VALUE))
			{
				bw.WriteInt(_value);
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
			if (HasBaseValue())
			{
				_baseValue = br.ReadInt();
			}
			if (HasValue())
			{
				_value = br.ReadInt();
			}
		}
		#endregion

		#region ---Clone---
		public Healing Clone()
		{            
			var _clone = ObjectCache.Get<Healing>();
			_clone._unitId = this._unitId;
			_clone._baseValue = this._baseValue;
			_clone._value = this._value;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_unitId = 0;
			_baseValue = 0;
			_value = 0;
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("Healing{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasUnitId())
			{
				sb.Append(",\r\n").Append(curIndent).Append("unitId = ");
				sb.Append(unitId);
			}
			if(HasBaseValue())
			{
				sb.Append(",\r\n").Append(curIndent).Append("baseValue = ");
				sb.Append(baseValue);
			}
			if(HasValue())
			{
				sb.Append(",\r\n").Append(curIndent).Append("value = ");
				sb.Append(value);
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