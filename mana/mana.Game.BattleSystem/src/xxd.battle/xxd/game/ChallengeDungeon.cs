using mana.Foundation;

namespace xxd.game
{
	/// <summary>
	/// 挑战副本
	/// </summary>
    public class ChallengeDungeon : DataObject
    {

		#region ---flags---
		public const byte __FLAG_DUNGEONTMPL = 0;
		public const byte __FLAG_DIFFICULTY = 1;
		public const long __MASK_ALL_VALUE = 0x3;

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
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {
			if (mask.CheckFlag(__FLAG_DUNGEONTMPL))
			{
				bw.WriteUTF8(_dungeonTmpl);
			}
			if (mask.CheckFlag(__FLAG_DIFFICULTY))
			{
				bw.WriteInt(_difficulty);
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
		}
		#endregion

		#region ---Clone---
		public ChallengeDungeon Clone()
		{            
			var _clone = ObjectCache.Get<ChallengeDungeon>();
			_clone._dungeonTmpl = this._dungeonTmpl;
			_clone._difficulty = this._difficulty;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_dungeonTmpl = null;
			_difficulty = 0;
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("ChallengeDungeon{\r\n");
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