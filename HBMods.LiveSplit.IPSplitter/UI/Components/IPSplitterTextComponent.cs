namespace LiveSplit.UI.Components
{
    public class IPSplitterTextComponent : InfoTextComponent
    {
        public IPSplitterComponentSettings Settings { get; set; }

        public IPSplitterTextComponent(IPSplitterComponentSettings settings)
            : base("", "")
        {
            Settings = settings;
        }

        public override void PrepareDraw(Model.LiveSplitState state, LayoutMode mode)
        {
            NameMeasureLabel.Font = Settings.Font1;
            ValueLabel.Font = Settings.Font1;
            NameLabel.Font = Settings.Font1;
        }
    }
}
