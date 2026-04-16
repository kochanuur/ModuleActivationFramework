using AppMain.Manager;
using MoAF.Abstractions.Container;

namespace AppMain
{
    public partial class MainWindow : Form
    {
        private AppModuleManager moduleManager;

        public MainWindow(IContainerProvider container)
        {
            InitializeComponent();

            moduleManager = container.Resolve<AppModuleManager>();
        }

        private async void MainWindow_Shown(object sender, EventArgs e)
        {
            await LoadAllParkIds();
            GetThemeParkName();
        }

        private async Task<int> LoadAllParkIds()
        {
            int result = await moduleManager.LoadAllParkIds();
            if (result != 0)
            {
                MessageBox.Show("テーマパークIDの取得に失敗しました");
            }
            return 0;
        }

        private void GetThemeParkName()
        {
            var list = moduleManager.GetThemeParkName();

            comboBoxThemeParkName.Items.Clear();
            foreach (var item in list)
            {
                comboBoxThemeParkName.Items.Add(item);
            }
            comboBoxThemeParkName.SelectedIndex = 0;
        }

        private async void buttonGetWaitTime_Click(object sender, EventArgs e)
        {
            textBoxWaitTime.Text = "";

            var dict = await moduleManager.GetWaitTimeList(comboBoxThemeParkName.SelectedIndex);
            foreach (var item in dict)
            {
                textBoxWaitTime.Text += item.Key + " : " + item.Value + "\r\n";
            }
        }
    }
}
