using System;
using Unity.Netcode;

public struct PlayerData : IEquatable<PlayerData> ,INetworkSerializable
{
    public ulong playerClientId;
    public int colorId;

    public bool Equals(PlayerData other)
    {
        return playerClientId == other.playerClientId && colorId == other.colorId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerClientId);
        serializer.SerializeValue(ref colorId);
    }
}
