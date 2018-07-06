using mana.Foundation;

namespace xxd.battle.opration
{
	[DataObjectConfig(TypeCode = 0x1001)]
    public class CastRequest : DataObject
    {

		#region ---flags---
		public const byte __FLAG_UNITID = 0;
		public const byte __FLAG_TARGETID = 1;
		public const byte __FLAG_ABILITYID = 2;
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

		#region ---targetId---
		private int _targetId = 0;
		public int targetId
		{
			get
			{
				return _targetId;
			}
			set
			{
				if(this._targetId != value)
				{
					this._targetId = value;
					this.mask.AddFlag(__FLAG_TARGETID);
				}
			}
		}

		public bool HasTargetId()
		{
			return this.mask.CheckFlag(__FLAG_TARGETID);
		}
		#endregion //targetId

		#region ---abilityId---
		private int _abilityId = 0;
		public int abilityId
		{
			get
			{
				return _abilityId;
			}
			set
			{
				if(this._abilityId != value)
				{
					this._abilityId = value;
					this.mask.AddFlag(__FLAG_ABILITYID);
				}
			}
		}

		public bool HasAbilityId()
		{
			return this.mask.CheckFlag(__FLAG_ABILITYID);
		}
		#endregion //abilityId
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {
			if (mask.CheckFlag(__FLAG_UNITID))
			{
				bw.WriteInt(_unitId);
			}
			if (mask.CheckFlag(__FLAG_TARGETID))
			{
				bw.WriteInt(_targetId);
			}
			if (mask.CheckFlag(__FLAG_ABILITYID))
			{
				bw.WriteInt(_abilityId);
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
			if (HasTargetId())
			{
				_targetId = br.ReadInt();
			}
			if (HasAbilityId())
			{
				_abilityId = br.ReadInt();
			}
		}
		#endregion

		#region ---Clone---
		public CastRequest Clone()
		{            
			var _clone = ObjectCache.Get<CastRequest>();
			_clone._unitId = this._unitId;
			_clone._targetId = this._targetId;
			_clone._abilityId = this._abilityId;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_unitId = 0;
			_targetId = 0;
			_abilityId = 0;
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("CastRequest{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasUnitId())
			{
				sb.Append(",\r\n").Append(curIndent).Append("unitId = ");
				sb.Append(unitId);
			}
			if(HasTargetId())
			{
				sb.Append(",\r\n").Append(curIndent).Append("targetId = ");
				sb.Append(targetId);
			}
			if(HasAbilityId())
			{
				sb.Append(",\r\n").Append(curIndent).Append("abilityId = ");
				sb.Append(abilityId);
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