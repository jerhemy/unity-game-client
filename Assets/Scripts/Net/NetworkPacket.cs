using System;
using System.IO;

namespace Net
{
    public struct NetworkPacket
    {
        private OP _type;
        private readonly byte[] _data;
        private readonly int _length;

        public NetworkPacket(OP type, byte[] data)
        {
            _type = type;
            _length = data.Length;
            byte[] bType = BitConverter.GetBytes((int) type);
            _data = new byte[_length + 4];
            
            Buffer.BlockCopy(bType, 0, _data, 0, 4 );     
            Buffer.BlockCopy(data, 0, _data, 2, _length );           
        }

        public NetworkPacket(byte[] data)
        {
            _type = (OP)BitConverter.ToInt32(data, 0);
            _length = data.Length - 4;
            _data = new byte[_length];
            Buffer.BlockCopy(data, 4, _data, 0, _length );           
        }
        
        public NetworkPacket(OP type)
        {
            _type = type;
            byte[] bType = BitConverter.GetBytes((int) type);
            var typeLength = sizeof(OP);
            _length = typeLength;
            _data = new byte[_length];
            Buffer.BlockCopy(bType, 0, _data, 0, typeLength );           
        }

        public OP type => _type;
        
        public int length => _length;

        public byte[] data => _data;
    }

    
    public struct OP_Login
    {
        public short type;
        
        public byte[] GetBytes()
        {
            using(MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write(type);            
                return ms.ToArray();
            }
        }

        public static OP_Login Deserialize(byte[] data)
        {
            using(MemoryStream ms = new MemoryStream())
            using (BinaryReader reader = new BinaryReader(ms))
            {
                var obj = new OP_Login
                {
                    type = reader.ReadInt16()
                };
                
                return obj;
            }
        }
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