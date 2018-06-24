using mana.Foundation;

namespace xxd.sync.opration.Dabad
{
    public class Class3 : DataObject
    {

		#region ---flags---
		public const long __MASK_ALL_VALUE = 0x0;

		#endregion

		#region ---mask---
		public readonly Mask mask = new Mask();
		#endregion
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {
        }
		#endregion

		#region ---Decode---
		public void Decode(IReadableBuffer br)
		{
			this.mask.Decode(br);
		}
		#endregion

		#region ---Clone---
		public Class3 Clone()
		{            
			var _clone = ObjectCache.Get<Class3>();
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("Class3{\r\n");
			var curIndent = newLineIndent + '\t';
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