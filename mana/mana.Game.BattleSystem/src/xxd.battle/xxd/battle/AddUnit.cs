using mana.Foundation;

namespace xxd.battle
{
    public class AddUnit : DataObject
    {

		#region ---flags---
		public const byte __FLAG_DATA = 0;
		public const long __MASK_ALL_VALUE = 0x1;

		#endregion

		#region ---mask---
		public readonly Mask mask = new Mask();
		#endregion

		#region ---data---
		private UnitInfo _data = null;
		public UnitInfo data
		{
			get
			{
				return _data;
			}
			set
			{
				if(this._data != value)
				{
					this._data = value;
					this.mask.AddFlag(__FLAG_DATA);
				}
			}
		}

		public bool HasData()
		{
			return this.mask.CheckFlag(__FLAG_DATA);
		}
		#endregion //data
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {		
			this.mask.Encode(bw);
			if (mask.CheckFlag(__FLAG_DATA))
			{
				bw.Write(_data);
			}
        }
		#endregion

		#region ---Decode---
		public void Decode(IReadableBuffer br)
		{
			this.mask.Decode(br);
			if (HasData())
			{
				_data = br.Read<UnitInfo>();
			}
		}
		#endregion

		#region ---Clone---
		public AddUnit Clone()
		{            
			var _clone = ObjectCache.Get<AddUnit>();
			_clone._data = this._data;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			if(_data != null)
			{
				_data.ReleaseToCache();
                _data = null;
			}
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("AddUnit{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasData())
			{
				sb.Append(",\r\n").Append(curIndent).Append("data = ");
				sb.Append(data == null ? "null" : data.ToFormatString(curIndent));
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