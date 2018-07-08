using mana.Foundation;

namespace xxd.battle
{
    public class UnitSync : DataObject
    {

		#region ---flags---
		public const byte __FLAG_UNITID = 0;
		public const byte __FLAG_FACETO = 1;
		public const byte __FLAG_X = 2;
		public const byte __FLAG_Y = 3;
		public const byte __FLAG_Z = 4;
		public const byte __FLAG_HPPERCENT = 5;
		public const byte __FLAG_MPPERCENT = 6;
		public const byte __FLAG_ACTIONSTATE = 7;
		public const byte __FLAG_ANIMPLAYCODE = 8;
		public const byte __FLAG_ANIMPLAYTIME = 9;
		public const byte __FLAG_EFFECTSHOW = 10;
		public const byte __FLAG_CONTROLFLAG = 11;
		public const long __MASK_ALL_VALUE = 0xfff;

		#endregion
		#region ---constants---
		public const int CF_STUN = 0;
		public const int CF_SILENT = 1;
		public const int CF_ENTANGLE = 2;
		public const int CF_SNARED = 3;
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

		#region ---hpPercent---
		private float _hpPercent = 0.0f;
		public float hpPercent
		{
			get
			{
				return _hpPercent;
			}
			set
			{
				if(this._hpPercent != value)
				{
					this._hpPercent = value;
					this.mask.AddFlag(__FLAG_HPPERCENT);
				}
			}
		}

		public bool HasHpPercent()
		{
			return this.mask.CheckFlag(__FLAG_HPPERCENT);
		}
		#endregion //hpPercent

		#region ---mpPercent---
		private float _mpPercent = 0.0f;
		public float mpPercent
		{
			get
			{
				return _mpPercent;
			}
			set
			{
				if(this._mpPercent != value)
				{
					this._mpPercent = value;
					this.mask.AddFlag(__FLAG_MPPERCENT);
				}
			}
		}

		public bool HasMpPercent()
		{
			return this.mask.CheckFlag(__FLAG_MPPERCENT);
		}
		#endregion //mpPercent

		#region ---actionState---
		private byte _actionState = 0;
		public byte actionState
		{
			get
			{
				return _actionState;
			}
			set
			{
				if(this._actionState != value)
				{
					this._actionState = value;
					this.mask.AddFlag(__FLAG_ACTIONSTATE);
				}
			}
		}

		public bool HasActionState()
		{
			return this.mask.CheckFlag(__FLAG_ACTIONSTATE);
		}
		#endregion //actionState

		#region ---AnimPlayCode---
		private int _AnimPlayCode = 0;
		public int AnimPlayCode
		{
			get
			{
				return _AnimPlayCode;
			}
			set
			{
				if(this._AnimPlayCode != value)
				{
					this._AnimPlayCode = value;
					this.mask.AddFlag(__FLAG_ANIMPLAYCODE);
				}
			}
		}

		public bool HasAnimPlayCode()
		{
			return this.mask.CheckFlag(__FLAG_ANIMPLAYCODE);
		}
		#endregion //AnimPlayCode

		#region ---AnimPlayTime---
		private float _AnimPlayTime = 0.0f;
		public float AnimPlayTime
		{
			get
			{
				return _AnimPlayTime;
			}
			set
			{
				if(this._AnimPlayTime != value)
				{
					this._AnimPlayTime = value;
					this.mask.AddFlag(__FLAG_ANIMPLAYTIME);
				}
			}
		}

		public bool HasAnimPlayTime()
		{
			return this.mask.CheckFlag(__FLAG_ANIMPLAYTIME);
		}
		#endregion //AnimPlayTime

		#region ---effectShow---
		private int _effectShow = 0;
		public int effectShow
		{
			get
			{
				return _effectShow;
			}
			set
			{
				if(this._effectShow != value)
				{
					this._effectShow = value;
					this.mask.AddFlag(__FLAG_EFFECTSHOW);
				}
			}
		}

		public bool HasEffectShow()
		{
			return this.mask.CheckFlag(__FLAG_EFFECTSHOW);
		}
		#endregion //effectShow

		#region ---controlFlag---
		private int _controlFlag = 0;
		public int controlFlag
		{
			get
			{
				return _controlFlag;
			}
			set
			{
				if(this._controlFlag != value)
				{
					this._controlFlag = value;
					this.mask.AddFlag(__FLAG_CONTROLFLAG);
				}
			}
		}

		public bool HasControlFlag()
		{
			return this.mask.CheckFlag(__FLAG_CONTROLFLAG);
		}
		#endregion //controlFlag
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {		
			this.mask.Encode(bw);
			if (mask.CheckFlag(__FLAG_UNITID))
			{
				bw.WriteInt(_unitId);
			}
			if (mask.CheckFlag(__FLAG_FACETO))
			{
				bw.WriteFloat(_faceTo);
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
			if (mask.CheckFlag(__FLAG_HPPERCENT))
			{
				bw.WriteFloat(_hpPercent);
			}
			if (mask.CheckFlag(__FLAG_MPPERCENT))
			{
				bw.WriteFloat(_mpPercent);
			}
			if (mask.CheckFlag(__FLAG_ACTIONSTATE))
			{
				bw.WriteByte(_actionState);
			}
			if (mask.CheckFlag(__FLAG_ANIMPLAYCODE))
			{
				bw.WriteInt(_AnimPlayCode);
			}
			if (mask.CheckFlag(__FLAG_ANIMPLAYTIME))
			{
				bw.WriteFloat(_AnimPlayTime);
			}
			if (mask.CheckFlag(__FLAG_EFFECTSHOW))
			{
				bw.WriteInt(_effectShow);
			}
			if (mask.CheckFlag(__FLAG_CONTROLFLAG))
			{
				bw.WriteInt(_controlFlag);
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
			if (HasFaceTo())
			{
				_faceTo = br.ReadFloat();
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
			if (HasHpPercent())
			{
				_hpPercent = br.ReadFloat();
			}
			if (HasMpPercent())
			{
				_mpPercent = br.ReadFloat();
			}
			if (HasActionState())
			{
				_actionState = br.ReadByte();
			}
			if (HasAnimPlayCode())
			{
				_AnimPlayCode = br.ReadInt();
			}
			if (HasAnimPlayTime())
			{
				_AnimPlayTime = br.ReadFloat();
			}
			if (HasEffectShow())
			{
				_effectShow = br.ReadInt();
			}
			if (HasControlFlag())
			{
				_controlFlag = br.ReadInt();
			}
		}
		#endregion

		#region ---Clone---
		public UnitSync Clone()
		{            
			var _clone = ObjectCache.Get<UnitSync>();
			_clone._unitId = this._unitId;
			_clone._faceTo = this._faceTo;
			_clone._x = this._x;
			_clone._y = this._y;
			_clone._z = this._z;
			_clone._hpPercent = this._hpPercent;
			_clone._mpPercent = this._mpPercent;
			_clone._actionState = this._actionState;
			_clone._AnimPlayCode = this._AnimPlayCode;
			_clone._AnimPlayTime = this._AnimPlayTime;
			_clone._effectShow = this._effectShow;
			_clone._controlFlag = this._controlFlag;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_unitId = 0;
			_faceTo = 0.0f;
			_x = 0.0f;
			_y = 0.0f;
			_z = 0.0f;
			_hpPercent = 0.0f;
			_mpPercent = 0.0f;
			_actionState = 0;
			_AnimPlayCode = 0;
			_AnimPlayTime = 0.0f;
			_effectShow = 0;
			_controlFlag = 0;
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("UnitSync{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasUnitId())
			{
				sb.Append(",\r\n").Append(curIndent).Append("unitId = ");
				sb.Append(unitId);
			}
			if(HasFaceTo())
			{
				sb.Append(",\r\n").Append(curIndent).Append("faceTo = ");
				sb.Append(faceTo);
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
			if(HasHpPercent())
			{
				sb.Append(",\r\n").Append(curIndent).Append("hpPercent = ");
				sb.Append(hpPercent);
			}
			if(HasMpPercent())
			{
				sb.Append(",\r\n").Append(curIndent).Append("mpPercent = ");
				sb.Append(mpPercent);
			}
			if(HasActionState())
			{
				sb.Append(",\r\n").Append(curIndent).Append("actionState = ");
				sb.Append(actionState);
			}
			if(HasAnimPlayCode())
			{
				sb.Append(",\r\n").Append(curIndent).Append("AnimPlayCode = ");
				sb.Append(AnimPlayCode);
			}
			if(HasAnimPlayTime())
			{
				sb.Append(",\r\n").Append(curIndent).Append("AnimPlayTime = ");
				sb.Append(AnimPlayTime);
			}
			if(HasEffectShow())
			{
				sb.Append(",\r\n").Append(curIndent).Append("effectShow = ");
				sb.Append(effectShow);
			}
			if(HasControlFlag())
			{
				sb.Append(",\r\n").Append(curIndent).Append("controlFlag = ");
				sb.Append(controlFlag);
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