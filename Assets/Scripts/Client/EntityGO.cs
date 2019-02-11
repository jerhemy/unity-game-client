using UnityEngine;

namespace Client
{
    public class EntityGO : MonoBehaviour
    {
        private TextMesh nameLabel;
        void Start()
        {
            GameObject sign = new GameObject("player_label");          
            sign.transform.rotation = Camera.main.transform.rotation; // Causes the text faces camera.
            nameLabel = sign.AddComponent<TextMesh>();
            nameLabel.text = "put your text here. You can use some of the html attributes";
            nameLabel.color = new Color(0.8f, 0.8f, 0.8f);
            nameLabel.fontStyle = FontStyle.Bold;
            nameLabel.alignment = TextAlignment.Center;
            nameLabel.anchor = TextAnchor.MiddleCenter;
            nameLabel.characterSize = 0.065f;
            nameLabel.fontSize = 60;
        }
    }
}