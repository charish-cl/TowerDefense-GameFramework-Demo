namespace GameMain.Scripts.MyUI
{
    public class UIItemDynamic: UIItemLogic
    {
        public virtual void OnShow(object data)
        {
            
        }
     
        
        //回收
        public virtual void OnRelease()
        {
            ResetUIVisibility();
            ResetUIInitialState();
        }
    }
}