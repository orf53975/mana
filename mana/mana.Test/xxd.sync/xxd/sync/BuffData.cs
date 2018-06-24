using mana.Foundation;

namespace xxd.sync
{
    public class BuffData : DataObject
    {

		#region ---flags---
		public const byte __FLAG_TMPLID = 0;
		public const byte __FLAG_LEV = 1;
		public const long __MASK_ALL_VALUE = 0x3;

		#endregion

		#region ---mask---
		public readonly Mask mask = new Mask();
		#endregion

		#region ---tmplId---
		private int _tmplId = 0;
		public int tmplId
		{
			get
			{
				return _tmplId;
			}
			set
			{
				if(this._tmplId != value)
				{
					this._tmplId = value;
					this.mask.AddFlag(__FLAG_TMPLID);
				}
			}
		}

		public bool HasTmplId()
		{
			return this.mask.CheckFlag(__FLAG_TMPLID);
		}
		#endregion //tmplId

		#region ---lev---
		private int _lev = 0;
		public int lev
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
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {
			if (mask.CheckFlag(__FLAG_TMPLID))
			{
				bw.WriteInt(_tmplId);
			}
			if (mask.CheckFlag(__FLAG_LEV))
			{
				bw.WriteInt(_lev);
			}
        }
		#endregion

		#region ---Decode---
		public void Decode(IReadableBuffer br)
		{
			this.mask.Decode(br);
			if (HasTmplId())
			{
				_tmplId = br.ReadInt();
			}
			if (HasLev())
			{
				_lev = br.ReadInt();
			}
		}
		#endregion

		#region ---Clone---
		public BuffData Clone()
		{            
			var _clone = ObjectCache.Get<BuffData>();
			_clone._tmplId = this._tmplId;
			_clone._lev = this._lev;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_tmplId = 0;
			_lev = 0;
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("BuffData{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasTmplId())
			{
				sb.Append(",\r\n").Append(curIndent).Append("tmplId = ");
				sb.Append(tmplId);
			}
			if(HasLev())
			{
				sb.Append(",\r\n").Append(curIndent).Append("lev = ");
				sb.Append(lev);
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