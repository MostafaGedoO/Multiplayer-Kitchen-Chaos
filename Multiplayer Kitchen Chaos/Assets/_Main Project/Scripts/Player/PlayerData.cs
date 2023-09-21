using System;
using Unity.Collections;
using Unity.Netcode;

public struct PlayerData : IEquatable<PlayerData> ,INetworkSerializable
{
    public ulong playerClientId;
    public int colorId;
    public FixedString64Bytes playerName;

    public bool Equals(PlayerData other)
    {
        return playerClientId == other.playerClientId && colorId == other.colorId && playerName == other.playerName;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerClientId);
        serializer.SerializeValue(ref colorId);
        serializer.SerializeValue(ref playerName);
    }
}
