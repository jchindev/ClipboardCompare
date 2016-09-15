using System.Windows;

using ClipboardCompare.Core;

namespace ClipboardCompare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ComparisonManager _comparisonManager;

        public MainWindow()
        {
            InitializeComponent();

            _comparisonManager = new ComparisonManager();
        }

        private void btnClip1_Click(object sender, RoutedEventArgs e)
        {
            _comparisonManager.Clip1Text = Clipboard.GetText();
            imgClip1.Visibility = string.IsNullOrEmpty(_comparisonManager.Clip1Text) ? Visibility.Hidden : Visibility.Visible;
        }

        private void btnClip2_Click(object sender, RoutedEventArgs e)
        {
            _comparisonManager.Clip2Text = Clipboard.GetText();
            imgClip2.Visibility = string.IsNullOrEmpty(_comparisonManager.Clip2Text) ? Visibility.Hidden : Visibility.Visible;
        }

        private void btnSwap_Click(object sender, RoutedEventArgs e)
        {
            var tempText = _comparisonManager.Clip1Text;
            _comparisonManager.Clip1Text = _comparisonManager.Clip2Text;
            _comparisonManager.Clip2Text = tempText;
        }

        private void btnCompare_Click(object sender, RoutedEventArgs e)
        {
            _comparisonManager.OpenComparisonProcess();
        }
    }
}
