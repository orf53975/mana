using mana.Foundation.Network.Server;
using xxd.battle.opration;
using xxd.game;
namespace xxd.battle
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
			ProtocolManager.AddTypeCode<BattleCreateInfo>();
			ProtocolManager.AddTypeCode<BattleSnap>();
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
			ProtocolManager.AddTypeCode<ChallengeDungeon>();
			ProtocolManager.AddTypeCode<CreateDungeon>();
		}
    }
}