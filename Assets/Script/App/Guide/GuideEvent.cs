namespace App.Guide
{
    public struct GuideEvent
    {
        public int Index;
        public int State;
        public object Something;
    }

    public enum GuideLayer
    {
        GuideBackLayer,
        GuideMiddelLayer,
        GuideFrontLayer,
    }

    public enum HandState
    {
        All,
        HoleAndTarget,
        Hole,
    }
}