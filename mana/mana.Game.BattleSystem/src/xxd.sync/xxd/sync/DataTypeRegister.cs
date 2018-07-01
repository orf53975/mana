using mana.Foundation.Network.Sever;
using xxd.sync.opration;
namespace xxd.sync
{
	/// <summary>
	/// Auto Gen
	/// </summary>
	class DataTypeRegister : IDataTypeRegister
    {
		public void RegistDataType()
		{
			ProtocolManager.AddTypeCode<AbilityData>();
			ProtocolManager.AddTypeCode<AddUnit>();
			ProtocolManager.AddTypeCode<BattleCreateData>();
			ProtocolManager.AddTypeCode<BattleSnapData>();
			ProtocolManager.AddTypeCode<BattleSync>();
			ProtocolManager.AddTypeCode<BuffData>();
			ProtocolManager.AddTypeCode<Damage>();
			ProtocolManager.AddTypeCode<Healing>();
			ProtocolManager.AddTypeCode<RemoveUnit>();
			ProtocolManager.AddTypeCode<UnitCreateData>();
			ProtocolManager.AddTypeCode<UnitExtProp>();
			ProtocolManager.AddTypeCode<UnitInfo>();
			ProtocolManager.AddTypeCode<UnitProp>();
			ProtocolManager.AddTypeCode<UnitSync>();
			ProtocolManager.AddTypeCode<CastRequest>();
			ProtocolManager.AddTypeCode<MoveRequest>();
		}
    }
}