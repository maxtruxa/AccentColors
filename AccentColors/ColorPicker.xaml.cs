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
using System.Windows.Shapes;

namespace AccentColors
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        public ColorPicker(Color startColor)
        {
            InitializeComponent();

            this.SelectedColor = Color.FromRgb(startColor.R, startColor.G, startColor.B);

            this.TextBoxColor.Text = "#" +
                this.SelectedColor.R.ToString("X2") +
                this.SelectedColor.G.ToString("X2") +
                this.SelectedColor.B.ToString("X2");
            this.TextBoxColor.SelectAll();
        }

        public Color SelectedColor { get; private set; }

        private void TextBoxColor_TextChanged(Object sender, TextChangedEventArgs e)
        {
            try
            {
                Color color = (Color)ColorConverter.ConvertFromString(this.TextBoxColor.Text);
                color.A = 0xFF;
                this.Background = new SolidColorBrush(color);
            }
            catch (FormatException) { } // It's okay to have an invalid formatted string while typing.
        }

        private void ButtonDiscard_Click(Object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ButtonUse_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                Color color = (Color)ColorConverter.ConvertFromString(this.TextBoxColor.Text);
                color.A = 0xFF;
                this.SelectedColor = color;

                this.DialogResult = true;
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show(this, "'" + this.TextBoxColor.Text + "' is neither a color code nor a color name.", "Error");
            }
        }
    }
}
