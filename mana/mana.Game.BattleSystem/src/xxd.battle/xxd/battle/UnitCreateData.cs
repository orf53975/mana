using mana.Foundation;

namespace xxd.battle
{
    public class UnitCreateData : DataObject
    {

		#region ---flags---
		public const byte __FLAG_INFO = 0;
		public const byte __FLAG_PROP = 1;
		public const long __MASK_ALL_VALUE = 0x3;

		#endregion

		#region ---mask---
		public readonly Mask mask = new Mask();
		#endregion

		#region ---info---
		private UnitInfo _info = null;
		public UnitInfo info
		{
			get
			{
				return _info;
			}
			set
			{
				if(this._info != value)
				{
					this._info = value;
					this.mask.AddFlag(__FLAG_INFO);
				}
			}
		}

		public bool HasInfo()
		{
			return this.mask.CheckFlag(__FLAG_INFO);
		}
		#endregion //info

		#region ---prop---
		private UnitProp _prop = null;
		public UnitProp prop
		{
			get
			{
				return _prop;
			}
			set
			{
				if(this._prop != value)
				{
					this._prop = value;
					this.mask.AddFlag(__FLAG_PROP);
				}
			}
		}

		public bool HasProp()
		{
			return this.mask.CheckFlag(__FLAG_PROP);
		}
		#endregion //prop
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {		
			this.mask.Encode(bw);
			if (mask.CheckFlag(__FLAG_INFO))
			{
				bw.Write(_info);
			}
			if (mask.CheckFlag(__FLAG_PROP))
			{
				bw.Write(_prop);
			}
        }
		#endregion

		#region ---Decode---
		public void Decode(IReadableBuffer br)
		{
			this.mask.Decode(br);
			if (HasInfo())
			{
				_info = br.Read<UnitInfo>();
			}
			if (HasProp())
			{
				_prop = br.Read<UnitProp>();
			}
		}
		#endregion

		#region ---Clone---
		public UnitCreateData Clone()
		{            
			var _clone = ObjectCache.Get<UnitCreateData>();
			_clone._info = this._info;
			_clone._prop = this._prop;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			if(_info != null)
			{
				_info.ReleaseToCache();
                _info = null;
			}
			if(_prop != null)
			{
				_prop.ReleaseToCache();
                _prop = null;
			}
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("UnitCreateData{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasInfo())
			{
				sb.Append(",\r\n").Append(curIndent).Append("info = ");
				sb.Append(info == null ? "null" : info.ToFormatString(curIndent));
			}
			if(HasProp())
			{
				sb.Append(",\r\n").Append(curIndent).Append("prop = ");
				sb.Append(prop == null ? "null" : prop.ToFormatString(curIndent));
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