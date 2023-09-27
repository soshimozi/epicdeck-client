using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EpiDeckClient.Controls
{
    /// <summary>
    /// Interaction logic for LedControl.xaml
    /// </summary>
    public partial class LedControl : UserControl
    {
        public LedControl()
        {
            InitializeComponent();
        }

        // Dependency Property for LED Color
        public static readonly DependencyProperty LedColorProperty = DependencyProperty.Register(
            "LedColor",
        typeof(Color),
            typeof(LedControl),
            new PropertyMetadata(Colors.Red));

        public Color LedColor
        {
            get => (Color)GetValue(LedColorProperty);
            set => SetValue(LedColorProperty, value);
        }
    }
}
