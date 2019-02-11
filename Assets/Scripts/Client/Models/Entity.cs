using Net;

namespace Client.Models
{
    public class Entity : INetworkPacket
    {
        private string name;
        private string last_name;

        private float _posX;
        private float _posY;
        private float _posZ;

        private short _race;
        private short _class;
        private byte _gender;

        private int texture_face;
        private int texture_skin;

        private int texture_helm;
        private int texture_chest;
        private int texture_shoulders;
        private int texture_arms;
        private int texture_bracer_left;
        private int texture_bracer_right;
        private int texture_gloves;
        private int texture_belt;
        private int texture_legs;
        private int texture_boots;

        private int main_hand;
        private int off_hand;
        
        public byte[] Serialize()
        {
            throw new System.NotImplementedException();
        }
    }
}