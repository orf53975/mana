using mana.Foundation;

namespace xxd.battle
{
    public class BattleCreateInfo : DataObject
    {

		#region ---flags---
		public const byte __FLAG_SCENEMAP = 0;
		public const byte __FLAG_UNITS = 1;
		public const long __MASK_ALL_VALUE = 0x3;

		#endregion

		#region ---mask---
		public readonly Mask mask = new Mask();
		#endregion

		#region ---sceneMap---
		private string _sceneMap = null;
		public string sceneMap
		{
			get
			{
				return _sceneMap;
			}
			set
			{
				if(this._sceneMap != value)
				{
					this._sceneMap = value;
					this.mask.AddFlag(__FLAG_SCENEMAP);
				}
			}
		}

		public bool HasSceneMap()
		{
			return this.mask.CheckFlag(__FLAG_SCENEMAP);
		}
		#endregion //sceneMap

		#region ---units---
		private UnitCreateData[] _units = null;
		public UnitCreateData[] units
		{
			get
			{
				return _units;
			}
			set
			{
				if(this._units != value)
				{
					this._units = value;
					this.mask.AddFlag(__FLAG_UNITS);
				}
			}
		}

		public bool HasUnits()
		{
			return this.mask.CheckFlag(__FLAG_UNITS);
		}
		#endregion //units
		
		#region ---Encode---
        public void Encode(IWritableBuffer bw)
        {		
			this.mask.Encode(bw);
			if (mask.CheckFlag(__FLAG_SCENEMAP))
			{
				bw.WriteUTF8(_sceneMap);
			}
			if (mask.CheckFlag(__FLAG_UNITS))
			{
				bw.WriteArray(_units);
			}
        }
		#endregion

		#region ---Decode---
		public void Decode(IReadableBuffer br)
		{
			this.mask.Decode(br);
			if (HasSceneMap())
			{
				_sceneMap = br.ReadUTF8();
			}
			if (HasUnits())
			{
				_units = br.ReadArray<UnitCreateData>();
			}
		}
		#endregion

		#region ---Clone---
		public BattleCreateInfo Clone()
		{            
			var _clone = ObjectCache.Get<BattleCreateInfo>();
			_clone._sceneMap = this._sceneMap;
			_clone._units = this._units;
			return _clone;
		}
		#endregion
		
		#region ---ReleaseToCache---
		public void ReleaseToCache()
        {
			this.mask.ClearAllFlag();
			_sceneMap = null;
			if(_units != null)
			{
				_units.ReleaseToCache();
                _units = null;
			}
			ObjectCache.Put(this);
        }
		#endregion

		#region ---ToFormatString---
		public string ToFormatString(string newLineIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("BattleCreateInfo{\r\n");
			var curIndent = newLineIndent + '\t';
			if(HasSceneMap())
			{
				sb.Append(",\r\n").Append(curIndent).Append("sceneMap = ");
				sb.Append(sceneMap);
			}
			if(HasUnits())
			{
				sb.Append(",\r\n").Append(curIndent).Append("units = ");
				sb.Append(units == null ? "null" : units.ToFormatString(curIndent));
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