namespace GameMain.Scripts.MyUI
{
    public class TextUIDynamic: UIItemDynamic
    {
        public override void OnShow(object data)
        {
            base.OnShow(data);
            var text = GetComponent<TMPro.TextMeshProUGUI>();
            text.text = null;
        }

        public override void UpdateItem(object data)
        {
            base.UpdateItem(data);
            var text = GetComponent<TMPro.TextMeshProUGUI>();
            text.text = data as string;
        }
    }
}