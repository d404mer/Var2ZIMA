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
using GIBDD.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GIBDD.Views
{
    /// <summary>
    /// Логика взаимодействия для ViewDriverWindow.xaml
    /// </summary>
    public partial class ViewDriverWindow : Window
    {
        public ViewDriverWindow(Driver driver)
        {
            InitializeComponent();

            this.DataContext = driver;
        }
    }
}
