namespace Net
{
    public enum OP
    {
        // Reliabled (1 - 4000)
        ClientConnect = 0x001,
        ClientDisconnect = 0x002,
        ClientGetPlayer = 0x003,
          
        // Unreliable Ordered - (4001 - 8000)
        ServerAddEntity = 0x03E8,
        ServerRemoveEntity = 0x03E9,
        ServerUpdateEntity = 0x03EA
        
        
        // Unreliable - (8001 - 12000)
    }
}