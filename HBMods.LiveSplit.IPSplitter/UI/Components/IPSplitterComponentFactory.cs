using LiveSplit.Model;
using System;

namespace LiveSplit.UI.Components
{
    public class IPSplitterComponentFactory : IComponentFactory
    {
        public string ComponentName => "IP Splitter";

        public string Description => "Allows Control via IP Connections (UDP/TCP)";

        public ComponentCategory Category => ComponentCategory.Control;

        public IComponent Create(LiveSplitState state) => new IPSplitterComponent(state);

        public string UpdateName => ComponentName;

        public string XMLURL => "http://livesplit.org/update/Components/update/HBMods.LiveSplit.IPSplitter.xml";

        public string UpdateURL => "http://livesplit.org/update/";

        public Version Version => Version.Parse("1.0.0");
    }
}
