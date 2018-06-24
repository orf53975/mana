using mana.Foundation;

namespace xxd.sync
{
    public class UnitExtProp : DataObject
    {

		#region ---flags---
		public const byte __FLAG_KEY = 0;
		public const byte __FLAG_VALUE = 1;
		public const long __MASK_ALL_VALUE = 0x3;

		#endregion

		#region ---mask---
		public readonly Mask mask = new Mask();
		#endregion

		#region ---key---
		private string _key = null;
		public string key
		{
			get
			{
				return _key;
			}
			set
			{
				if(this._key != value)
				{
					this._key = value;
					this.mask.AddFlag(__FLAG_KEY);
				}
			}
		}

		public bool HasKey()
		{
			return this.mask.CheckFlag(__FLAG_KEY);
		}
		#endregion //key

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
			if (mask.CheckFlag(__FLAG_KEY))
			{
				bw.WriteUTF8(_key);
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
			if (HasKey())
			{
				_key = br.ReadUTF8();
			}
			if (HasValue())
			{
				_value = br.ReadInt();
			}
		}
		#endregion

		#region ---Clone---
		public UnitExtProp Clone()
		{            
			var _clone = ObjectCache.Get<UnitExtProp>();
			_clone._key = this._key;
			_clone._value = this._value;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_key = null;
			_value = 0;
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("UnitExtProp{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasKey())
			{
				sb.Append(",\r\n").Append(curIndent).Append("key = ");
				sb.Append(key);
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