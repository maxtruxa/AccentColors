using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AccentColors
{
    public static class ColorExtender
    {
        public static Boolean IsSimilarTo(this Color color, Color baseColor, Byte tolerance = 0x20)
        {
            Int32 colorR = color.R;
            Int32 colorG = color.G;
            Int32 colorB = color.B;
            Int32 baseColorR = baseColor.R;
            Int32 baseColorG = baseColor.G;
            Int32 baseColorB = baseColor.B;
            return
                (colorR - tolerance) <= baseColorR && (colorR + tolerance) >= baseColorR &&
                (colorG - tolerance) <= baseColorG && (colorG + tolerance) >= baseColorG &&
                (colorB - tolerance) <= baseColorB && (colorB + tolerance) >= baseColorB;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean Initializing { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Get available colors.
            var names = AccentColorSet.ActiveSet.GetAllColorNames();
            this.AllColors = new Dictionary<String, Color>();
            foreach (var name in names)
            {
                this.AllColors.Add(name, AccentColorSet.ActiveSet[name]);
            }
            this.DisplayedColors = new ObservableCollection<KeyValuePair<String, Color>>();

            // Setup the data context.
            this.DataContext = this;

            // Initialize color picker button.
            this.SetButtonColor(this.ButtonFilterColor, Colors.White);

            // Initialize tolerance values for comparison.
            for (Int32 i = 0; i <= 0xFF; i++)
                this.ComboBoxTolerance.Items.Add((Byte)i);
            this.ComboBoxTolerance.SelectedIndex = 0x20;

            this.UpdateDisplayedColors();
        }

        private void CheckBoxFilter_Checked(Object sender, RoutedEventArgs e)
        {
            this.UpdateDisplayedColors();
        }

        private void ButtonFilterColor_Click(Object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ColorPicker picker = new ColorPicker(((SolidColorBrush)button.Background).Color)
            {
                Owner = Application.Current.MainWindow,
                ShowInTaskbar = false
            };
            if (picker.ShowDialog() == true)
            {
                this.SetButtonColor(button, picker.SelectedColor);
            }
        }

        private void ComboBoxTolerance_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (this.IsInitialized)
            {
                this.UpdateDisplayedColors();
            }
        }

        private void SetButtonColor(Button button, Color color)
        {
            var c = new KeyValuePair<String, Color>("", color);
            var fconverter = new ColorToForegroundConverter();
            var bconverter = new ColorToBackgroundConverter();
            button.Foreground = (Brush)fconverter.Convert(c, typeof(SolidColorBrush), null, System.Globalization.CultureInfo.CurrentCulture);
            button.Background = (Brush)bconverter.Convert(c, typeof(SolidColorBrush), null, System.Globalization.CultureInfo.CurrentCulture);

            this.UpdateDisplayedColors();
        }

        private void UpdateDisplayedColors()
        {
            this.DisplayedColors.Clear();
            if (this.CheckBoxFilter.IsChecked == true)
            {
                Color baseColor = ((SolidColorBrush)this.ButtonFilterColor.Background).Color;
                Byte tolerance = (Byte)this.ComboBoxTolerance.SelectedValue;

                //var collection = from c in this.AllColors where c.Value.IsSimilarTo(baseColor) select c;
                //this.DisplayedColors = collection.ToDictionary(t => t.Key, t => t.Value);

                foreach (var color in this.AllColors)
                {
                    if (color.Value.IsSimilarTo(baseColor, tolerance))
                        this.DisplayedColors.Add(color);
                }
            }
            else
            {
                foreach (var color in this.AllColors)
                {
                    this.DisplayedColors.Add(color);
                }
            }
        }

        public Dictionary<String, Color> AllColors { get; private set; }
        public ObservableCollection<KeyValuePair<String, Color>> DisplayedColors { get; private set; }
    }

    public class ColorToBackgroundConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            var entry = (KeyValuePair<String, Color>)value;
            return new SolidColorBrush(entry.Value);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class ColorToForegroundConverter : IValueConverter
    {
        private Boolean IsBrightColor(Color color)
        {
            Byte limit = 0x7F;
            return
                color.A < limit ||
                (color.R > limit && color.G > limit) ||
                (color.R > limit && color.B > limit) ||
                (color.G > limit && color.B > limit);
        }
        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            var entry = (KeyValuePair<String, Color>)value;
            Color color = entry.Value;
            if (this.IsBrightColor(color))
                return new SolidColorBrush(Colors.Black);
            else
                return new SolidColorBrush(Colors.White);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
