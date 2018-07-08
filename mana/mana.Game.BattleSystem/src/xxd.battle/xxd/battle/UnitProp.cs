using mana.Foundation;

namespace xxd.battle
{
    public class UnitProp : DataObject
    {

		#region ---flags---
		public const byte __FLAG_AITMPLID = 0;
		public const byte __FLAG_HPMAX = 1;
		public const byte __FLAG_MPMAX = 2;
		public const byte __FLAG_PHYATK = 3;
		public const byte __FLAG_PHYDEF = 4;
		public const byte __FLAG_MAGATK = 5;
		public const byte __FLAG_MAGDEF = 6;
		public const byte __FLAG_ATKTIMELENGTH = 7;
		public const byte __FLAG_ATKKEYFRAMETIME = 8;
		public const byte __FLAG_STIFFTIME = 9;
		public const byte __FLAG_FOV = 10;
		public const byte __FLAG_ATKRANGE = 11;
		public const byte __FLAG_BODYSIZE = 12;
		public const byte __FLAG_HIT = 13;
		public const byte __FLAG_DODGE = 14;
		public const byte __FLAG_CRIT = 15;
		public const byte __FLAG_TOUGHNESS = 16;
		public const byte __FLAG_MOVESPEED = 17;
		public const byte __FLAG_ABILITIESDATA = 18;
		public const byte __FLAG_EXTPROPS = 19;
		public const long __MASK_ALL_VALUE = 0xfffff;

		#endregion

		#region ---mask---
		public readonly Mask mask = new Mask();
		#endregion

		#region ---aiTmplId---
		private short _aiTmplId = 0;
		public short aiTmplId
		{
			get
			{
				return _aiTmplId;
			}
			set
			{
				if(this._aiTmplId != value)
				{
					this._aiTmplId = value;
					this.mask.AddFlag(__FLAG_AITMPLID);
				}
			}
		}

		public bool HasAiTmplId()
		{
			return this.mask.CheckFlag(__FLAG_AITMPLID);
		}
		#endregion //aiTmplId

		#region ---hpMax---
		private int _hpMax = 0;
		public int hpMax
		{
			get
			{
				return _hpMax;
			}
			set
			{
				if(this._hpMax != value)
				{
					this._hpMax = value;
					this.mask.AddFlag(__FLAG_HPMAX);
				}
			}
		}

		public bool HasHpMax()
		{
			return this.mask.CheckFlag(__FLAG_HPMAX);
		}
		#endregion //hpMax

		#region ---mpMax---
		private int _mpMax = 0;
		public int mpMax
		{
			get
			{
				return _mpMax;
			}
			set
			{
				if(this._mpMax != value)
				{
					this._mpMax = value;
					this.mask.AddFlag(__FLAG_MPMAX);
				}
			}
		}

		public bool HasMpMax()
		{
			return this.mask.CheckFlag(__FLAG_MPMAX);
		}
		#endregion //mpMax

		#region ---phyAtk---
		private int _phyAtk = 0;
		public int phyAtk
		{
			get
			{
				return _phyAtk;
			}
			set
			{
				if(this._phyAtk != value)
				{
					this._phyAtk = value;
					this.mask.AddFlag(__FLAG_PHYATK);
				}
			}
		}

		public bool HasPhyAtk()
		{
			return this.mask.CheckFlag(__FLAG_PHYATK);
		}
		#endregion //phyAtk

		#region ---phyDef---
		private int _phyDef = 0;
		public int phyDef
		{
			get
			{
				return _phyDef;
			}
			set
			{
				if(this._phyDef != value)
				{
					this._phyDef = value;
					this.mask.AddFlag(__FLAG_PHYDEF);
				}
			}
		}

		public bool HasPhyDef()
		{
			return this.mask.CheckFlag(__FLAG_PHYDEF);
		}
		#endregion //phyDef

		#region ---magAtk---
		private int _magAtk = 0;
		public int magAtk
		{
			get
			{
				return _magAtk;
			}
			set
			{
				if(this._magAtk != value)
				{
					this._magAtk = value;
					this.mask.AddFlag(__FLAG_MAGATK);
				}
			}
		}

		public bool HasMagAtk()
		{
			return this.mask.CheckFlag(__FLAG_MAGATK);
		}
		#endregion //magAtk

		#region ---magDef---
		private int _magDef = 0;
		public int magDef
		{
			get
			{
				return _magDef;
			}
			set
			{
				if(this._magDef != value)
				{
					this._magDef = value;
					this.mask.AddFlag(__FLAG_MAGDEF);
				}
			}
		}

		public bool HasMagDef()
		{
			return this.mask.CheckFlag(__FLAG_MAGDEF);
		}
		#endregion //magDef

		#region ---atkTimeLength---
		private float _atkTimeLength = 0.0f;
		/// <summary>
        /// 攻击总时间
        /// </summary>
		public float atkTimeLength
		{
			get
			{
				return _atkTimeLength;
			}
			set
			{
				if(this._atkTimeLength != value)
				{
					this._atkTimeLength = value;
					this.mask.AddFlag(__FLAG_ATKTIMELENGTH);
				}
			}
		}

		public bool HasAtkTimeLength()
		{
			return this.mask.CheckFlag(__FLAG_ATKTIMELENGTH);
		}
		#endregion //atkTimeLength

		#region ---atkKeyFrameTime---
		private float _atkKeyFrameTime = 0.0f;
		/// <summary>
        /// 攻击前摇时间
        /// </summary>
		public float atkKeyFrameTime
		{
			get
			{
				return _atkKeyFrameTime;
			}
			set
			{
				if(this._atkKeyFrameTime != value)
				{
					this._atkKeyFrameTime = value;
					this.mask.AddFlag(__FLAG_ATKKEYFRAMETIME);
				}
			}
		}

		public bool HasAtkKeyFrameTime()
		{
			return this.mask.CheckFlag(__FLAG_ATKKEYFRAMETIME);
		}
		#endregion //atkKeyFrameTime

		#region ---stiffTime---
		private float _stiffTime = 0.0f;
		/// <summary>
        /// 僵直时间
        /// </summary>
		public float stiffTime
		{
			get
			{
				return _stiffTime;
			}
			set
			{
				if(this._stiffTime != value)
				{
					this._stiffTime = value;
					this.mask.AddFlag(__FLAG_STIFFTIME);
				}
			}
		}

		public bool HasStiffTime()
		{
			return this.mask.CheckFlag(__FLAG_STIFFTIME);
		}
		#endregion //stiffTime

		#region ---fov---
		private float _fov = 0.0f;
		public float fov
		{
			get
			{
				return _fov;
			}
			set
			{
				if(this._fov != value)
				{
					this._fov = value;
					this.mask.AddFlag(__FLAG_FOV);
				}
			}
		}

		public bool HasFov()
		{
			return this.mask.CheckFlag(__FLAG_FOV);
		}
		#endregion //fov

		#region ---atkRange---
		private float _atkRange = 0.0f;
		public float atkRange
		{
			get
			{
				return _atkRange;
			}
			set
			{
				if(this._atkRange != value)
				{
					this._atkRange = value;
					this.mask.AddFlag(__FLAG_ATKRANGE);
				}
			}
		}

		public bool HasAtkRange()
		{
			return this.mask.CheckFlag(__FLAG_ATKRANGE);
		}
		#endregion //atkRange

		#region ---bodySize---
		private float _bodySize = 0.0f;
		public float bodySize
		{
			get
			{
				return _bodySize;
			}
			set
			{
				if(this._bodySize != value)
				{
					this._bodySize = value;
					this.mask.AddFlag(__FLAG_BODYSIZE);
				}
			}
		}

		public bool HasBodySize()
		{
			return this.mask.CheckFlag(__FLAG_BODYSIZE);
		}
		#endregion //bodySize

		#region ---hit---
		private int _hit = 0;
		public int hit
		{
			get
			{
				return _hit;
			}
			set
			{
				if(this._hit != value)
				{
					this._hit = value;
					this.mask.AddFlag(__FLAG_HIT);
				}
			}
		}

		public bool HasHit()
		{
			return this.mask.CheckFlag(__FLAG_HIT);
		}
		#endregion //hit

		#region ---dodge---
		private int _dodge = 0;
		public int dodge
		{
			get
			{
				return _dodge;
			}
			set
			{
				if(this._dodge != value)
				{
					this._dodge = value;
					this.mask.AddFlag(__FLAG_DODGE);
				}
			}
		}

		public bool HasDodge()
		{
			return this.mask.CheckFlag(__FLAG_DODGE);
		}
		#endregion //dodge

		#region ---crit---
		private int _crit = 0;
		public int crit
		{
			get
			{
				return _crit;
			}
			set
			{
				if(this._crit != value)
				{
					this._crit = value;
					this.mask.AddFlag(__FLAG_CRIT);
				}
			}
		}

		public bool HasCrit()
		{
			return this.mask.CheckFlag(__FLAG_CRIT);
		}
		#endregion //crit

		#region ---toughness---
		private int _toughness = 0;
		public int toughness
		{
			get
			{
				return _toughness;
			}
			set
			{
				if(this._toughness != value)
				{
					this._toughness = value;
					this.mask.AddFlag(__FLAG_TOUGHNESS);
				}
			}
		}

		public bool HasToughness()
		{
			return this.mask.CheckFlag(__FLAG_TOUGHNESS);
		}
		#endregion //toughness

		#region ---moveSpeed---
		private short _moveSpeed = 0;
		public short moveSpeed
		{
			get
			{
				return _moveSpeed;
			}
			set
			{
				if(this._moveSpeed != value)
				{
					this._moveSpeed = value;
					this.mask.AddFlag(__FLAG_MOVESPEED);
				}
			}
		}

		public bool HasMoveSpeed()
		{
			return this.mask.CheckFlag(__FLAG_MOVESPEED);
		}
		#endregion //moveSpeed

		#region ---abilitiesData---
		private AbilityData[] _abilitiesData = null;
		public AbilityData[] abilitiesData
		{
			get
			{
				return _abilitiesData;
			}
			set
			{
				if(this._abilitiesData != value)
				{
					this._abilitiesData = value;
					this.mask.AddFlag(__FLAG_ABILITIESDATA);
				}
			}
		}

		public bool HasAbilitiesData()
		{
			return this.mask.CheckFlag(__FLAG_ABILITIESDATA);
		}
		#endregion //abilitiesData

		#region ---extProps---
		private UnitExtProp[] _extProps = null;
		public UnitExtProp[] extProps
		{
			get
			{
				return _extProps;
			}
			set
			{
				if(this._extProps != value)
				{
					this._extProps = value;
					this.mask.AddFlag(__FLAG_EXTPROPS);
				}
			}
		}

		public bool HasExtProps()
		{
			return this.mask.CheckFlag(__FLAG_EXTPROPS);
		}
		#endregion //extProps
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {		
			this.mask.Encode(bw);
			if (mask.CheckFlag(__FLAG_AITMPLID))
			{
				bw.WriteShort(_aiTmplId);
			}
			if (mask.CheckFlag(__FLAG_HPMAX))
			{
				bw.WriteInt(_hpMax);
			}
			if (mask.CheckFlag(__FLAG_MPMAX))
			{
				bw.WriteInt(_mpMax);
			}
			if (mask.CheckFlag(__FLAG_PHYATK))
			{
				bw.WriteInt(_phyAtk);
			}
			if (mask.CheckFlag(__FLAG_PHYDEF))
			{
				bw.WriteInt(_phyDef);
			}
			if (mask.CheckFlag(__FLAG_MAGATK))
			{
				bw.WriteInt(_magAtk);
			}
			if (mask.CheckFlag(__FLAG_MAGDEF))
			{
				bw.WriteInt(_magDef);
			}
			if (mask.CheckFlag(__FLAG_ATKTIMELENGTH))
			{
				bw.WriteFloat(_atkTimeLength);
			}
			if (mask.CheckFlag(__FLAG_ATKKEYFRAMETIME))
			{
				bw.WriteFloat(_atkKeyFrameTime);
			}
			if (mask.CheckFlag(__FLAG_STIFFTIME))
			{
				bw.WriteFloat(_stiffTime);
			}
			if (mask.CheckFlag(__FLAG_FOV))
			{
				bw.WriteFloat(_fov);
			}
			if (mask.CheckFlag(__FLAG_ATKRANGE))
			{
				bw.WriteFloat(_atkRange);
			}
			if (mask.CheckFlag(__FLAG_BODYSIZE))
			{
				bw.WriteFloat(_bodySize);
			}
			if (mask.CheckFlag(__FLAG_HIT))
			{
				bw.WriteInt(_hit);
			}
			if (mask.CheckFlag(__FLAG_DODGE))
			{
				bw.WriteInt(_dodge);
			}
			if (mask.CheckFlag(__FLAG_CRIT))
			{
				bw.WriteInt(_crit);
			}
			if (mask.CheckFlag(__FLAG_TOUGHNESS))
			{
				bw.WriteInt(_toughness);
			}
			if (mask.CheckFlag(__FLAG_MOVESPEED))
			{
				bw.WriteShort(_moveSpeed);
			}
			if (mask.CheckFlag(__FLAG_ABILITIESDATA))
			{
				bw.WriteArray(_abilitiesData);
			}
			if (mask.CheckFlag(__FLAG_EXTPROPS))
			{
				bw.WriteArray(_extProps);
			}
        }
		#endregion

		#region ---Decode---
		public void Decode(IReadableBuffer br)
		{
			this.mask.Decode(br);
			if (HasAiTmplId())
			{
				_aiTmplId = br.ReadShort();
			}
			if (HasHpMax())
			{
				_hpMax = br.ReadInt();
			}
			if (HasMpMax())
			{
				_mpMax = br.ReadInt();
			}
			if (HasPhyAtk())
			{
				_phyAtk = br.ReadInt();
			}
			if (HasPhyDef())
			{
				_phyDef = br.ReadInt();
			}
			if (HasMagAtk())
			{
				_magAtk = br.ReadInt();
			}
			if (HasMagDef())
			{
				_magDef = br.ReadInt();
			}
			if (HasAtkTimeLength())
			{
				_atkTimeLength = br.ReadFloat();
			}
			if (HasAtkKeyFrameTime())
			{
				_atkKeyFrameTime = br.ReadFloat();
			}
			if (HasStiffTime())
			{
				_stiffTime = br.ReadFloat();
			}
			if (HasFov())
			{
				_fov = br.ReadFloat();
			}
			if (HasAtkRange())
			{
				_atkRange = br.ReadFloat();
			}
			if (HasBodySize())
			{
				_bodySize = br.ReadFloat();
			}
			if (HasHit())
			{
				_hit = br.ReadInt();
			}
			if (HasDodge())
			{
				_dodge = br.ReadInt();
			}
			if (HasCrit())
			{
				_crit = br.ReadInt();
			}
			if (HasToughness())
			{
				_toughness = br.ReadInt();
			}
			if (HasMoveSpeed())
			{
				_moveSpeed = br.ReadShort();
			}
			if (HasAbilitiesData())
			{
				_abilitiesData = br.ReadArray<AbilityData>();
			}
			if (HasExtProps())
			{
				_extProps = br.ReadArray<UnitExtProp>();
			}
		}
		#endregion

		#region ---Clone---
		public UnitProp Clone()
		{            
			var _clone = ObjectCache.Get<UnitProp>();
			_clone._aiTmplId = this._aiTmplId;
			_clone._hpMax = this._hpMax;
			_clone._mpMax = this._mpMax;
			_clone._phyAtk = this._phyAtk;
			_clone._phyDef = this._phyDef;
			_clone._magAtk = this._magAtk;
			_clone._magDef = this._magDef;
			_clone._atkTimeLength = this._atkTimeLength;
			_clone._atkKeyFrameTime = this._atkKeyFrameTime;
			_clone._stiffTime = this._stiffTime;
			_clone._fov = this._fov;
			_clone._atkRange = this._atkRange;
			_clone._bodySize = this._bodySize;
			_clone._hit = this._hit;
			_clone._dodge = this._dodge;
			_clone._crit = this._crit;
			_clone._toughness = this._toughness;
			_clone._moveSpeed = this._moveSpeed;
			_clone._abilitiesData = this._abilitiesData;
			_clone._extProps = this._extProps;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_aiTmplId = 0;
			_hpMax = 0;
			_mpMax = 0;
			_phyAtk = 0;
			_phyDef = 0;
			_magAtk = 0;
			_magDef = 0;
			_atkTimeLength = 0.0f;
			_atkKeyFrameTime = 0.0f;
			_stiffTime = 0.0f;
			_fov = 0.0f;
			_atkRange = 0.0f;
			_bodySize = 0.0f;
			_hit = 0;
			_dodge = 0;
			_crit = 0;
			_toughness = 0;
			_moveSpeed = 0;
			if(_abilitiesData != null)
			{
				_abilitiesData.ReleaseToCache();
                _abilitiesData = null;
			}
			if(_extProps != null)
			{
				_extProps.ReleaseToCache();
                _extProps = null;
			}
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("UnitProp{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasAiTmplId())
			{
				sb.Append(",\r\n").Append(curIndent).Append("aiTmplId = ");
				sb.Append(aiTmplId);
			}
			if(HasHpMax())
			{
				sb.Append(",\r\n").Append(curIndent).Append("hpMax = ");
				sb.Append(hpMax);
			}
			if(HasMpMax())
			{
				sb.Append(",\r\n").Append(curIndent).Append("mpMax = ");
				sb.Append(mpMax);
			}
			if(HasPhyAtk())
			{
				sb.Append(",\r\n").Append(curIndent).Append("phyAtk = ");
				sb.Append(phyAtk);
			}
			if(HasPhyDef())
			{
				sb.Append(",\r\n").Append(curIndent).Append("phyDef = ");
				sb.Append(phyDef);
			}
			if(HasMagAtk())
			{
				sb.Append(",\r\n").Append(curIndent).Append("magAtk = ");
				sb.Append(magAtk);
			}
			if(HasMagDef())
			{
				sb.Append(",\r\n").Append(curIndent).Append("magDef = ");
				sb.Append(magDef);
			}
			if(HasAtkTimeLength())
			{
				sb.Append(",\r\n").Append(curIndent).Append("atkTimeLength = ");
				sb.Append(atkTimeLength);
			}
			if(HasAtkKeyFrameTime())
			{
				sb.Append(",\r\n").Append(curIndent).Append("atkKeyFrameTime = ");
				sb.Append(atkKeyFrameTime);
			}
			if(HasStiffTime())
			{
				sb.Append(",\r\n").Append(curIndent).Append("stiffTime = ");
				sb.Append(stiffTime);
			}
			if(HasFov())
			{
				sb.Append(",\r\n").Append(curIndent).Append("fov = ");
				sb.Append(fov);
			}
			if(HasAtkRange())
			{
				sb.Append(",\r\n").Append(curIndent).Append("atkRange = ");
				sb.Append(atkRange);
			}
			if(HasBodySize())
			{
				sb.Append(",\r\n").Append(curIndent).Append("bodySize = ");
				sb.Append(bodySize);
			}
			if(HasHit())
			{
				sb.Append(",\r\n").Append(curIndent).Append("hit = ");
				sb.Append(hit);
			}
			if(HasDodge())
			{
				sb.Append(",\r\n").Append(curIndent).Append("dodge = ");
				sb.Append(dodge);
			}
			if(HasCrit())
			{
				sb.Append(",\r\n").Append(curIndent).Append("crit = ");
				sb.Append(crit);
			}
			if(HasToughness())
			{
				sb.Append(",\r\n").Append(curIndent).Append("toughness = ");
				sb.Append(toughness);
			}
			if(HasMoveSpeed())
			{
				sb.Append(",\r\n").Append(curIndent).Append("moveSpeed = ");
				sb.Append(moveSpeed);
			}
			if(HasAbilitiesData())
			{
				sb.Append(",\r\n").Append(curIndent).Append("abilitiesData = ");
				sb.Append(abilitiesData == null ? "null" : abilitiesData.ToFormatString(curIndent));
			}
			if(HasExtProps())
			{
				sb.Append(",\r\n").Append(curIndent).Append("extProps = ");
				sb.Append(extProps == null ? "null" : extProps.ToFormatString(curIndent));
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