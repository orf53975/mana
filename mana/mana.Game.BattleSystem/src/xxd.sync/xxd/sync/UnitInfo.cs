using mana.Foundation;

namespace xxd.sync
{
    public class UnitInfo : DataObject
    {

		#region ---flags---
		public const byte __FLAG_PLAYERID = 0;
		public const byte __FLAG_UID = 1;
		public const byte __FLAG_NAME = 2;
		public const byte __FLAG_APPEARANCE = 3;
		public const byte __FLAG_CATEGORY = 4;
		public const byte __FLAG_TYPE = 5;
		public const byte __FLAG_CAMP = 6;
		public const byte __FLAG_LEV = 7;
		public const byte __FLAG_X = 8;
		public const byte __FLAG_Y = 9;
		public const byte __FLAG_Z = 10;
		public const byte __FLAG_FACETO = 11;
		public const byte __FLAG_HPPERCENT = 12;
		public const byte __FLAG_MPPERCENT = 13;
		public const byte __FLAG_MOVESPEED = 14;
		public const byte __FLAG_ACTIONSTATE = 15;
		public const byte __FLAG_CURRENTANIM = 16;
		public const byte __FLAG_EFFECTSHOW = 17;
		public const byte __FLAG_CONTROLFLAG = 18;
		public const byte __FLAG_BUFFSDATA = 19;
		public const long __MASK_ALL_VALUE = 0xfffff;

		#endregion

		#region ---mask---
		public readonly Mask mask = new Mask();
		#endregion

		#region ---playerId---
		private int _playerId = 0;
		public int playerId
		{
			get
			{
				return _playerId;
			}
			set
			{
				if(this._playerId != value)
				{
					this._playerId = value;
					this.mask.AddFlag(__FLAG_PLAYERID);
				}
			}
		}

		public bool HasPlayerId()
		{
			return this.mask.CheckFlag(__FLAG_PLAYERID);
		}
		#endregion //playerId

		#region ---uid---
		private int _uid = 0;
		public int uid
		{
			get
			{
				return _uid;
			}
			set
			{
				if(this._uid != value)
				{
					this._uid = value;
					this.mask.AddFlag(__FLAG_UID);
				}
			}
		}

		public bool HasUid()
		{
			return this.mask.CheckFlag(__FLAG_UID);
		}
		#endregion //uid

		#region ---name---
		private string _name = null;
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				if(this._name != value)
				{
					this._name = value;
					this.mask.AddFlag(__FLAG_NAME);
				}
			}
		}

		public bool HasName()
		{
			return this.mask.CheckFlag(__FLAG_NAME);
		}
		#endregion //name

		#region ---appearance---
		private string _appearance = null;
		public string appearance
		{
			get
			{
				return _appearance;
			}
			set
			{
				if(this._appearance != value)
				{
					this._appearance = value;
					this.mask.AddFlag(__FLAG_APPEARANCE);
				}
			}
		}

		public bool HasAppearance()
		{
			return this.mask.CheckFlag(__FLAG_APPEARANCE);
		}
		#endregion //appearance

		#region ---category---
		private byte _category = 0;
		public byte category
		{
			get
			{
				return _category;
			}
			set
			{
				if(this._category != value)
				{
					this._category = value;
					this.mask.AddFlag(__FLAG_CATEGORY);
				}
			}
		}

		public bool HasCategory()
		{
			return this.mask.CheckFlag(__FLAG_CATEGORY);
		}
		#endregion //category

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

		#region ---camp---
		private byte _camp = 0;
		public byte camp
		{
			get
			{
				return _camp;
			}
			set
			{
				if(this._camp != value)
				{
					this._camp = value;
					this.mask.AddFlag(__FLAG_CAMP);
				}
			}
		}

		public bool HasCamp()
		{
			return this.mask.CheckFlag(__FLAG_CAMP);
		}
		#endregion //camp

		#region ---lev---
		private short _lev = 0;
		public short lev
		{
			get
			{
				return _lev;
			}
			set
			{
				if(this._lev != value)
				{
					this._lev = value;
					this.mask.AddFlag(__FLAG_LEV);
				}
			}
		}

		public bool HasLev()
		{
			return this.mask.CheckFlag(__FLAG_LEV);
		}
		#endregion //lev

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

		#region ---currentAnim---
		private int _currentAnim = 0;
		public int currentAnim
		{
			get
			{
				return _currentAnim;
			}
			set
			{
				if(this._currentAnim != value)
				{
					this._currentAnim = value;
					this.mask.AddFlag(__FLAG_CURRENTANIM);
				}
			}
		}

		public bool HasCurrentAnim()
		{
			return this.mask.CheckFlag(__FLAG_CURRENTANIM);
		}
		#endregion //currentAnim

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

		#region ---buffsData---
		private BuffData[] _buffsData = null;
		public BuffData[] buffsData
		{
			get
			{
				return _buffsData;
			}
			set
			{
				if(this._buffsData != value)
				{
					this._buffsData = value;
					this.mask.AddFlag(__FLAG_BUFFSDATA);
				}
			}
		}

		public bool HasBuffsData()
		{
			return this.mask.CheckFlag(__FLAG_BUFFSDATA);
		}
		#endregion //buffsData
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {
			if (mask.CheckFlag(__FLAG_PLAYERID))
			{
				bw.WriteInt(_playerId);
			}
			if (mask.CheckFlag(__FLAG_UID))
			{
				bw.WriteInt(_uid);
			}
			if (mask.CheckFlag(__FLAG_NAME))
			{
				bw.WriteUTF8(_name);
			}
			if (mask.CheckFlag(__FLAG_APPEARANCE))
			{
				bw.WriteUTF8(_appearance);
			}
			if (mask.CheckFlag(__FLAG_CATEGORY))
			{
				bw.WriteByte(_category);
			}
			if (mask.CheckFlag(__FLAG_TYPE))
			{
				bw.WriteByte(_type);
			}
			if (mask.CheckFlag(__FLAG_CAMP))
			{
				bw.WriteByte(_camp);
			}
			if (mask.CheckFlag(__FLAG_LEV))
			{
				bw.WriteShort(_lev);
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
			if (mask.CheckFlag(__FLAG_HPPERCENT))
			{
				bw.WriteFloat(_hpPercent);
			}
			if (mask.CheckFlag(__FLAG_MPPERCENT))
			{
				bw.WriteFloat(_mpPercent);
			}
			if (mask.CheckFlag(__FLAG_MOVESPEED))
			{
				bw.WriteShort(_moveSpeed);
			}
			if (mask.CheckFlag(__FLAG_ACTIONSTATE))
			{
				bw.WriteByte(_actionState);
			}
			if (mask.CheckFlag(__FLAG_CURRENTANIM))
			{
				bw.WriteInt(_currentAnim);
			}
			if (mask.CheckFlag(__FLAG_EFFECTSHOW))
			{
				bw.WriteInt(_effectShow);
			}
			if (mask.CheckFlag(__FLAG_CONTROLFLAG))
			{
				bw.WriteInt(_controlFlag);
			}
			if (mask.CheckFlag(__FLAG_BUFFSDATA))
			{
				bw.WriteArray(_buffsData);
			}
        }
		#endregion

		#region ---Decode---
		public void Decode(IReadableBuffer br)
		{
			this.mask.Decode(br);
			if (HasPlayerId())
			{
				_playerId = br.ReadInt();
			}
			if (HasUid())
			{
				_uid = br.ReadInt();
			}
			if (HasName())
			{
				_name = br.ReadUTF8();
			}
			if (HasAppearance())
			{
				_appearance = br.ReadUTF8();
			}
			if (HasCategory())
			{
				_category = br.ReadByte();
			}
			if (HasType())
			{
				_type = br.ReadByte();
			}
			if (HasCamp())
			{
				_camp = br.ReadByte();
			}
			if (HasLev())
			{
				_lev = br.ReadShort();
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
			if (HasHpPercent())
			{
				_hpPercent = br.ReadFloat();
			}
			if (HasMpPercent())
			{
				_mpPercent = br.ReadFloat();
			}
			if (HasMoveSpeed())
			{
				_moveSpeed = br.ReadShort();
			}
			if (HasActionState())
			{
				_actionState = br.ReadByte();
			}
			if (HasCurrentAnim())
			{
				_currentAnim = br.ReadInt();
			}
			if (HasEffectShow())
			{
				_effectShow = br.ReadInt();
			}
			if (HasControlFlag())
			{
				_controlFlag = br.ReadInt();
			}
			if (HasBuffsData())
			{
				_buffsData = br.ReadArray<BuffData>();
			}
		}
		#endregion

		#region ---Clone---
		public UnitInfo Clone()
		{            
			var _clone = ObjectCache.Get<UnitInfo>();
			_clone._playerId = this._playerId;
			_clone._uid = this._uid;
			_clone._name = this._name;
			_clone._appearance = this._appearance;
			_clone._category = this._category;
			_clone._type = this._type;
			_clone._camp = this._camp;
			_clone._lev = this._lev;
			_clone._x = this._x;
			_clone._y = this._y;
			_clone._z = this._z;
			_clone._faceTo = this._faceTo;
			_clone._hpPercent = this._hpPercent;
			_clone._mpPercent = this._mpPercent;
			_clone._moveSpeed = this._moveSpeed;
			_clone._actionState = this._actionState;
			_clone._currentAnim = this._currentAnim;
			_clone._effectShow = this._effectShow;
			_clone._controlFlag = this._controlFlag;
			_clone._buffsData = this._buffsData;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_playerId = 0;
			_uid = 0;
			_name = null;
			_appearance = null;
			_category = 0;
			_type = 0;
			_camp = 0;
			_lev = 0;
			_x = 0.0f;
			_y = 0.0f;
			_z = 0.0f;
			_faceTo = 0.0f;
			_hpPercent = 0.0f;
			_mpPercent = 0.0f;
			_moveSpeed = 0;
			_actionState = 0;
			_currentAnim = 0;
			_effectShow = 0;
			_controlFlag = 0;
			if(_buffsData != null)
			{
				_buffsData.ReleaseToCache();
                _buffsData = null;
			}
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("UnitInfo{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasPlayerId())
			{
				sb.Append(",\r\n").Append(curIndent).Append("playerId = ");
				sb.Append(playerId);
			}
			if(HasUid())
			{
				sb.Append(",\r\n").Append(curIndent).Append("uid = ");
				sb.Append(uid);
			}
			if(HasName())
			{
				sb.Append(",\r\n").Append(curIndent).Append("name = ");
				sb.Append(name);
			}
			if(HasAppearance())
			{
				sb.Append(",\r\n").Append(curIndent).Append("appearance = ");
				sb.Append(appearance);
			}
			if(HasCategory())
			{
				sb.Append(",\r\n").Append(curIndent).Append("category = ");
				sb.Append(category);
			}
			if(HasType())
			{
				sb.Append(",\r\n").Append(curIndent).Append("type = ");
				sb.Append(type);
			}
			if(HasCamp())
			{
				sb.Append(",\r\n").Append(curIndent).Append("camp = ");
				sb.Append(camp);
			}
			if(HasLev())
			{
				sb.Append(",\r\n").Append(curIndent).Append("lev = ");
				sb.Append(lev);
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
			if(HasMoveSpeed())
			{
				sb.Append(",\r\n").Append(curIndent).Append("moveSpeed = ");
				sb.Append(moveSpeed);
			}
			if(HasActionState())
			{
				sb.Append(",\r\n").Append(curIndent).Append("actionState = ");
				sb.Append(actionState);
			}
			if(HasCurrentAnim())
			{
				sb.Append(",\r\n").Append(curIndent).Append("currentAnim = ");
				sb.Append(currentAnim);
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
			if(HasBuffsData())
			{
				sb.Append(",\r\n").Append(curIndent).Append("buffsData = ");
				sb.Append(buffsData == null ? "null" : buffsData.ToFormatString(curIndent));
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