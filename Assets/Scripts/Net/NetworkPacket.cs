using System;
using System.IO;
using ReliableNetcode;

namespace Net
{
    public struct NetworkPacket
    {
        private const int typeSize = sizeof(OP);
        private readonly OP _type;
        private readonly byte[] _data;
        private readonly int _size;
        private QosType _qosType;
        
        public NetworkPacket(OP type, byte[] data)
        {
            _type = type;
            
            var t = (int) _type;
            
            if (t <= 4000)
            {
                _qosType = QosType.Reliable;
            }
            else
            {
                _qosType = t <= 8000 ? QosType.UnreliableOrdered : QosType.Unreliable;
            }
            
            _size = data.Length;
            var bType = BitConverter.GetBytes((int) type);
            _data = new byte[_size + typeSize];
            
            Buffer.BlockCopy(bType, 0, _data, 0, typeSize );     
            Buffer.BlockCopy(data, 0, _data, 2, _size );           
        }
        
        public NetworkPacket(OP type)
        {
            _type = type;
            
            var t = (int) _type;
            
            if (t <= 4000)
            {
                _qosType = QosType.Reliable;
            }
            else
            {
                _qosType = t <= 8000 ? QosType.UnreliableOrdered : QosType.Unreliable;
            }

            var bType = BitConverter.GetBytes((int) type);
            var typeLength = sizeof(OP);
            _size = typeSize;
            _data = new byte[_size];
            Buffer.BlockCopy(bType, 0, _data, 0, _size );           
        }
        
        public NetworkPacket(INetworkPacket obj)
        {
            _type = OP.ClientConnect;

            var t = (int) _type;
            
            if (t <= 4000)
            {
                _qosType = QosType.Reliable;
            }
            else
            {
                _qosType = t <= 8000 ? QosType.UnreliableOrdered : QosType.Unreliable;
            }
            
            _data = obj.Serialize();
            _size = _data.Length;
        }


        
        public static NetworkPacket Decode(byte[] data)
        {
            var type = (OP)BitConverter.ToInt32(data, 0);
            var length = data.Length - 4;
            var payload = new byte[length];       
            Buffer.BlockCopy(data, 4, payload, 0, length );  
            
            return new NetworkPacket(type, payload);
        }

        public OP type => _type;
        
        public int Size => _size;

        public byte[] data => _data;

        public QosType qosType
        {
            get
            {
                var t = (int) _type;
                if (t <= 4000)
                {
                    return QosType.Reliable;
                }

                return t <= 8000 ? QosType.UnreliableOrdered : QosType.Unreliable;
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