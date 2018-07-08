using mana.Foundation;

namespace xxd.game
{
    public class CreateDungeon : DataObject
    {

		#region ---flags---
		public const byte __FLAG_DUNGEONTMPL = 0;
		public const byte __FLAG_DIFFICULTY = 1;
		public const byte __FLAG_DUNGEONLEVEL = 2;
		public const byte __FLAG_OTHERS = 3;
		public const long __MASK_ALL_VALUE = 0xf;

		#endregion

		#region ---mask---
		public readonly Mask mask = new Mask();
		#endregion

		#region ---dungeonTmpl---
		private string _dungeonTmpl = null;
		/// <summary>
        /// 副本模板
        /// </summary>
		public string dungeonTmpl
		{
			get
			{
				return _dungeonTmpl;
			}
			set
			{
				if(this._dungeonTmpl != value)
				{
					this._dungeonTmpl = value;
					this.mask.AddFlag(__FLAG_DUNGEONTMPL);
				}
			}
		}

		public bool HasDungeonTmpl()
		{
			return this.mask.CheckFlag(__FLAG_DUNGEONTMPL);
		}
		#endregion //dungeonTmpl

		#region ---difficulty---
		private int _difficulty = 0;
		/// <summary>
        /// 挑战难度
        /// </summary>
		public int difficulty
		{
			get
			{
				return _difficulty;
			}
			set
			{
				if(this._difficulty != value)
				{
					this._difficulty = value;
					this.mask.AddFlag(__FLAG_DIFFICULTY);
				}
			}
		}

		public bool HasDifficulty()
		{
			return this.mask.CheckFlag(__FLAG_DIFFICULTY);
		}
		#endregion //difficulty

		#region ---dungeonLevel---
		private int _dungeonLevel = 0;
		/// <summary>
        /// 副本等级
        /// </summary>
		public int dungeonLevel
		{
			get
			{
				return _dungeonLevel;
			}
			set
			{
				if(this._dungeonLevel != value)
				{
					this._dungeonLevel = value;
					this.mask.AddFlag(__FLAG_DUNGEONLEVEL);
				}
			}
		}

		public bool HasDungeonLevel()
		{
			return this.mask.CheckFlag(__FLAG_DUNGEONLEVEL);
		}
		#endregion //dungeonLevel

		#region ---others---
		private string _others = null;
		/// <summary>
        /// 副本其他参数
        /// </summary>
		public string others
		{
			get
			{
				return _others;
			}
			set
			{
				if(this._others != value)
				{
					this._others = value;
					this.mask.AddFlag(__FLAG_OTHERS);
				}
			}
		}

		public bool HasOthers()
		{
			return this.mask.CheckFlag(__FLAG_OTHERS);
		}
		#endregion //others
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {		
			this.mask.Encode(bw);
			if (mask.CheckFlag(__FLAG_DUNGEONTMPL))
			{
				bw.WriteUTF8(_dungeonTmpl);
			}
			if (mask.CheckFlag(__FLAG_DIFFICULTY))
			{
				bw.WriteInt(_difficulty);
			}
			if (mask.CheckFlag(__FLAG_DUNGEONLEVEL))
			{
				bw.WriteInt(_dungeonLevel);
			}
			if (mask.CheckFlag(__FLAG_OTHERS))
			{
				bw.WriteUTF8(_others);
			}
        }
		#endregion

		#region ---Decode---
		public void Decode(IReadableBuffer br)
		{
			this.mask.Decode(br);
			if (HasDungeonTmpl())
			{
				_dungeonTmpl = br.ReadUTF8();
			}
			if (HasDifficulty())
			{
				_difficulty = br.ReadInt();
			}
			if (HasDungeonLevel())
			{
				_dungeonLevel = br.ReadInt();
			}
			if (HasOthers())
			{
				_others = br.ReadUTF8();
			}
		}
		#endregion

		#region ---Clone---
		public CreateDungeon Clone()
		{            
			var _clone = ObjectCache.Get<CreateDungeon>();
			_clone._dungeonTmpl = this._dungeonTmpl;
			_clone._difficulty = this._difficulty;
			_clone._dungeonLevel = this._dungeonLevel;
			_clone._others = this._others;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_dungeonTmpl = null;
			_difficulty = 0;
			_dungeonLevel = 0;
			_others = null;
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("CreateDungeon{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasDungeonTmpl())
			{
				sb.Append(",\r\n").Append(curIndent).Append("dungeonTmpl = ");
				sb.Append(dungeonTmpl);
			}
			if(HasDifficulty())
			{
				sb.Append(",\r\n").Append(curIndent).Append("difficulty = ");
				sb.Append(difficulty);
			}
			if(HasDungeonLevel())
			{
				sb.Append(",\r\n").Append(curIndent).Append("dungeonLevel = ");
				sb.Append(dungeonLevel);
			}
			if(HasOthers())
			{
				sb.Append(",\r\n").Append(curIndent).Append("others = ");
				sb.Append(others);
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