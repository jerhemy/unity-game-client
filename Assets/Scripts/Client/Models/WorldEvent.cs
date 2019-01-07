using Client.Net;

namespace Client.Models
{
    // Zone Point -> [Zone ID, Src Zone, Src X,Y,Z, Destination Zone, Destination X,Y,Z]
    
    public struct ZoneRequest
    {
        public int zoneId;
    }
    
    public struct ZoneResponse
    {
        public int zoneId;
    }
    
    public struct OP_InviteGroup
    {
        public ulong playerId;
    }
    
    
}