using LiveSplit.Model;
using LiveSplit.UI.Components.Network;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{
    public partial class IPSplitterComponentSettings : UserControl
    {
        public LayoutMode Mode { get; set; }

        public Color BackgroundColor { get; set; }
        public Color BackgroundColor2 { get; set; }
        public GradientType BackgroundGradient { get; set; }
        public Font Font1 { get; set; }

        public int Port { get; set; } = 1966;
        public bool IgnorePauses { get; set; } = true;

        public bool ShowInfo { get; set; } = false;

        public bool ShowInfo_Deaths { get; set; } = false;
        public bool ShowInfo_Logouts { get; set; } = false;
        public bool ShowInfo_TCPStatus { get; set; } = false;
        public bool ShowInfo_TCPPort { get; set; } = false;
        public bool ShowInfo_TCPClients { get; set; } = false;

        public bool DisplayTwoRows => false;

        public int DeathCount { get; set; }
        public int LogoutCount { get; set; }
        public int SplitCount { get; set; }


        private string _serverStatusText = "";

        public string StatusText 
        { 
            get => _serverStatusText; 
            private set
            {
                if (_serverStatusText == value) 
                    return;
                _serverStatusText = value;
                lblListenerStatus.Text = StatusText;
            }
        }


        public LiveSplitState CurrentState { get; set; }


        public IPSplitterComponentSettings()
        {
            InitializeComponent();

            Font1 = new Font("Segoe UI", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            BackgroundColor = Color.Transparent;
            BackgroundColor2 = Color.Transparent;
            BackgroundGradient = GradientType.Plain;
        }


        void TextComponentSettings_Load(object sender, EventArgs e)
        {
            txtPort.Text = Port.ToString();

            chkIgnorePauseResume.Checked = IgnorePauses;

            chkShowInfo.Checked = ShowInfo;

            chkShowDeaths.Checked = ShowInfo_Deaths;
            chkShowLogout.Checked = ShowInfo_Logouts;
            chkShowServerStatus.Checked = ShowInfo_TCPStatus;
            chkShowServerPort.Checked = ShowInfo_TCPPort;
            chkShowServerClients.Checked = ShowInfo_TCPClients;

            UpdateServerStatus();
        }


        public void SetSettings(XmlNode node)
        {
            var element = (XmlElement)node;

            Port = SettingsHelper.ParseInt(element[nameof(Port)], 1966);

            IgnorePauses = SettingsHelper.ParseBool(element[nameof(IgnorePauses)], true);
            
            ShowInfo = SettingsHelper.ParseBool(element[nameof(ShowInfo)], false);
            
            ShowInfo_Deaths = SettingsHelper.ParseBool(element[nameof(ShowInfo_Deaths)], true);
            ShowInfo_Logouts = SettingsHelper.ParseBool(element[nameof(ShowInfo_Logouts)], false);
            ShowInfo_TCPStatus = SettingsHelper.ParseBool(element[nameof(ShowInfo_TCPStatus)], false);
            ShowInfo_TCPPort = SettingsHelper.ParseBool(element[nameof(ShowInfo_TCPPort)], false);
            ShowInfo_TCPClients = SettingsHelper.ParseBool(element[nameof(ShowInfo_TCPClients)], false);
        }


        public XmlNode GetSettings(XmlDocument document)
        {
            var parent = document.CreateElement("Settings");
            CreateSettingsNode(document, parent);
            return parent;
        }



        private int CreateSettingsNode(XmlDocument document, XmlElement parent)
        {
            return SettingsHelper.CreateSetting(document, parent, "Version", "1.0") ^
                SettingsHelper.CreateSetting(document, parent, nameof(Port), Port) ^
                SettingsHelper.CreateSetting(document, parent, nameof(IgnorePauses), IgnorePauses) ^
                SettingsHelper.CreateSetting(document, parent, nameof(ShowInfo), ShowInfo) ^
                SettingsHelper.CreateSetting(document, parent, nameof(ShowInfo_Deaths), ShowInfo_Deaths) ^
                SettingsHelper.CreateSetting(document, parent, nameof(ShowInfo_Logouts), ShowInfo_Logouts) ^
                SettingsHelper.CreateSetting(document, parent, nameof(ShowInfo_TCPStatus), ShowInfo_TCPStatus) ^
                SettingsHelper.CreateSetting(document, parent, nameof(ShowInfo_TCPPort), ShowInfo_TCPPort) ^
                SettingsHelper.CreateSetting(document, parent, nameof(ShowInfo_TCPClients), ShowInfo_TCPClients);
        }


        public int GetSettingsHashCode()
        {
            return CreateSettingsNode(null, null);
        }


        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (int.TryParse(textBox.Text, out int port))
                {
                    Port = port;
                }
            }

            UpdateServerStatus();
        }


        private void chkIgnorePauseResume_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox chkBox)
                IgnorePauses = chkBox.Checked;

            UpdateServerStatus();
        }


        private void chkShowInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox chkBox)
                ShowInfo = chkBox.Checked;

            UpdateServerStatus();
        }


        private void chkShowDeaths_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox chkBox)
                ShowInfo_Deaths = chkBox.Checked;

            UpdateServerStatus();
        }


        private void chkShowLogout_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox chkBox)
                ShowInfo_Logouts = chkBox.Checked;

            UpdateServerStatus();
        }


        private void chkShowServerStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox chkBox)
                ShowInfo_TCPStatus = chkBox.Checked;

            UpdateServerStatus();
        }


        private void chkShowServerPort_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox chkBox)
                ShowInfo_TCPPort = chkBox.Checked;

            UpdateServerStatus();
        }


        private void chkShowServerClients_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox chkBox)
                ShowInfo_TCPClients = chkBox.Checked;

            UpdateServerStatus();
        }


        internal void UpdateServerStatus()
        {
            if (!ShowInfo)
                return;

            //if (_tcpServer == null)
            //{
            //    ServerStatusText = "-";
            //    return;
            //}

            StringBuilder sb = new StringBuilder();

            if (ShowInfo_Deaths)
                sb.Append($"Deaths: {DeathCount}|");

            if (ShowInfo_Logouts)
                sb.Append($"Logouts: {LogoutCount}|");

            //if (ShowInfo_TCPStatus)
            //    sb.Append($"TCP: {TcpStatus}|");

            //if (ShowInfo_TCPPort)
            //    sb.Append($"Port: {TcpClients}|");

            //if (ShowInfo_TCPClients)
            //    sb.Append($"Clients: {TcpClients}|");

            if (sb.Length == 0)
            {
                StatusText = $"-";
                return;
            }

            string status = sb.ToString();

            StatusText = status.Substring(0, status.Length - 1).Replace("|", ", ");
        }
    }
}
