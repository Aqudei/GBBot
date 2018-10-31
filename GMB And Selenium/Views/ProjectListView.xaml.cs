using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GMB_And_Selenium.Views
{
    /// <summary>
    /// Interaction logic for ProjectListView.xaml
    /// </summary>
    public partial class ProjectListView : UserControl
    {
        public ProjectListView()
        {
            InitializeComponent();
        }

        private void Projects_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.ToUpper().Contains("NOTIFY"))
                e.Cancel = true;

            if (e.PropertyName.ToUpper().Contains("BOOLEAN"))
                e.Cancel = true;
        }

        private void ImportProjects_Copy_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
