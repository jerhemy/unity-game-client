using System.IO;

namespace Net
{
    public struct NetworkPacket
    {
        
    }


    public struct OP_ClientPacket
    {
        public long id;
        public string name;
        
        public float x;
        public float y;
        public float z;
        public float direction;

        public int model;
        
        public byte[] GetBytes()
        {
            using(MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write(id);
                writer.Write(name);
                writer.Write(x);
                writer.Write(y);
                writer.Write(z);
                writer.Write(direction);
                writer.Write(model);
                
                return ms.ToArray();
            }
        }

        public static OP_ClientPacket Deserialize(byte[] data)
        {
            using(MemoryStream ms = new MemoryStream())
            using (BinaryReader reader = new BinaryReader(ms))
            {
                var obj = new OP_ClientPacket
                {
                    id = reader.ReadInt64(),
                    name = reader.ReadString(),
                    x = reader.ReadSingle(),
                    y = reader.ReadSingle(),
                    z = reader.ReadSingle(),
                    direction = reader.ReadSingle(),
                    model = reader.ReadInt32()
                };


                return obj;
            }
        }
    }
    
    public struct OP_Move
    {
        public float x;
        public float y;
        public float z;
        public float direction;
        
        public byte[] GetBytes()
        {
            using(MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write(x);
                writer.Write(y);
                writer.Write(z);
                writer.Write(direction);
                
                return ms.ToArray();
            }
        }

        public static OP_Move Deserialize(byte[] data)
        {
            using(MemoryStream ms = new MemoryStream())
            using (BinaryReader reader = new BinaryReader(ms))
            {
                var obj = new OP_Move
                {
                    x = reader.ReadSingle(),
                    y = reader.ReadSingle(),
                    z = reader.ReadSingle(),
                    direction = reader.ReadSingle()
                };

                return obj;
            }
        }
    }
}