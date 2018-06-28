using mana.Foundation;

namespace xxd.sync
{
    public class BattleSync : DataObject
    {

		#region ---flags---
		public const byte __FLAG_ACTIONS = 0;
		public const long __MASK_ALL_VALUE = 0x1;

		#endregion

		#region ---mask---
		public readonly Mask mask = new Mask();
		#endregion

		#region ---actions---
		private DataObject[] _actions = null;
		public DataObject[] actions
		{
			get
			{
				return _actions;
			}
			set
			{
				if(this._actions != value)
				{
					this._actions = value;
					this.mask.AddFlag(__FLAG_ACTIONS);
				}
			}
		}

		public bool HasActions()
		{
			return this.mask.CheckFlag(__FLAG_ACTIONS);
		}
		#endregion //actions
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {
			if (mask.CheckFlag(__FLAG_ACTIONS))
			{
				bw.WriteUnknowArray(_actions);
			}
        }
		#endregion

		#region ---Decode---
		public void Decode(IReadableBuffer br)
		{
			this.mask.Decode(br);
			if (HasActions())
			{
				_actions = br.ReadUnknowArray();
			}
		}
		#endregion

		#region ---Clone---
		public BattleSync Clone()
		{            
			var _clone = ObjectCache.Get<BattleSync>();
			_clone._actions = this._actions;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			if(_actions != null)
			{
				_actions.ReleaseToCache();
                _actions = null;
			}
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("BattleSync{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasActions())
			{
				sb.Append(",\r\n").Append(curIndent).Append("actions = ");
				sb.Append(actions == null ? "null" : actions.ToFormatString(curIndent));
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