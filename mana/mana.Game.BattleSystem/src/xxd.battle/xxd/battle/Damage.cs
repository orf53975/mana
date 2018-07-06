using mana.Foundation;

namespace xxd.battle
{
    public class Damage : DataObject
    {

		#region ---flags---
		public const byte __FLAG_UNITID = 0;
		public const byte __FLAG_BASEVALUE = 1;
		public const byte __FLAG_VALUE = 2;
		public const byte __FLAG_DAMAGETYPE = 3;
		public const byte __FLAG_DAMAGEFLAG = 4;
		public const byte __FLAG_KILLEDPARAM = 5;
		public const long __MASK_ALL_VALUE = 0x3f;

		#endregion
		#region ---constants---
		public const byte dtype_pure = 0;
		public const byte dtype_physical = 1;
		public const byte dtype_magic = 2;
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

		#region ---damageType---
		private int _damageType = 0;
		public int damageType
		{
			get
			{
				return _damageType;
			}
			set
			{
				if(this._damageType != value)
				{
					this._damageType = value;
					this.mask.AddFlag(__FLAG_DAMAGETYPE);
				}
			}
		}

		public bool HasDamageType()
		{
			return this.mask.CheckFlag(__FLAG_DAMAGETYPE);
		}
		#endregion //damageType

		#region ---damageFlag---
		private int _damageFlag = 0;
		public int damageFlag
		{
			get
			{
				return _damageFlag;
			}
			set
			{
				if(this._damageFlag != value)
				{
					this._damageFlag = value;
					this.mask.AddFlag(__FLAG_DAMAGEFLAG);
				}
			}
		}

		public bool HasDamageFlag()
		{
			return this.mask.CheckFlag(__FLAG_DAMAGEFLAG);
		}
		#endregion //damageFlag

		#region ---killedParam---
		private byte _killedParam = 0;
		public byte killedParam
		{
			get
			{
				return _killedParam;
			}
			set
			{
				if(this._killedParam != value)
				{
					this._killedParam = value;
					this.mask.AddFlag(__FLAG_KILLEDPARAM);
				}
			}
		}

		public bool HasKilledParam()
		{
			return this.mask.CheckFlag(__FLAG_KILLEDPARAM);
		}
		#endregion //killedParam
		
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
			if (mask.CheckFlag(__FLAG_DAMAGETYPE))
			{
				bw.WriteInt(_damageType);
			}
			if (mask.CheckFlag(__FLAG_DAMAGEFLAG))
			{
				bw.WriteInt(_damageFlag);
			}
			if (mask.CheckFlag(__FLAG_KILLEDPARAM))
			{
				bw.WriteByte(_killedParam);
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
			if (HasDamageType())
			{
				_damageType = br.ReadInt();
			}
			if (HasDamageFlag())
			{
				_damageFlag = br.ReadInt();
			}
			if (HasKilledParam())
			{
				_killedParam = br.ReadByte();
			}
		}
		#endregion

		#region ---Clone---
		public Damage Clone()
		{            
			var _clone = ObjectCache.Get<Damage>();
			_clone._unitId = this._unitId;
			_clone._baseValue = this._baseValue;
			_clone._value = this._value;
			_clone._damageType = this._damageType;
			_clone._damageFlag = this._damageFlag;
			_clone._killedParam = this._killedParam;
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
			_damageType = 0;
			_damageFlag = 0;
			_killedParam = 0;
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("Damage{\r\n");
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
			if(HasDamageType())
			{
				sb.Append(",\r\n").Append(curIndent).Append("damageType = ");
				sb.Append(damageType);
			}
			if(HasDamageFlag())
			{
				sb.Append(",\r\n").Append(curIndent).Append("damageFlag = ");
				sb.Append(damageFlag);
			}
			if(HasKilledParam())
			{
				sb.Append(",\r\n").Append(curIndent).Append("killedParam = ");
				sb.Append(killedParam);
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