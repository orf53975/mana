using mana.Foundation;

namespace xxd.battle
{
    public class RemoveUnit : DataObject
    {

		#region ---flags---
		public const byte __FLAG_UNITID = 0;
		public const long __MASK_ALL_VALUE = 0x1;

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
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {		
			this.mask.Encode(bw);
			if (mask.CheckFlag(__FLAG_UNITID))
			{
				bw.WriteInt(_unitId);
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
		}
		#endregion

		#region ---Clone---
		public RemoveUnit Clone()
		{            
			var _clone = ObjectCache.Get<RemoveUnit>();
			_clone._unitId = this._unitId;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_unitId = 0;
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("RemoveUnit{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasUnitId())
			{
				sb.Append(",\r\n").Append(curIndent).Append("unitId = ");
				sb.Append(unitId);
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