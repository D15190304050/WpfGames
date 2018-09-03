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

namespace Sokoban
{
    /// <summary>
    /// EndPoint.xaml 的交互逻辑
    /// </summary>
    public partial class EndPoint : UserControl
    {
        private static readonly Dictionary<EndPointColors, BitmapImage> endPointImages;

        public static DependencyProperty EndPointColorProperty;

        public EndPointColors EndPointColor
        {
            get { return (EndPointColors)GetValue(EndPointColorProperty); }
            set { SetValue(EndPointColorProperty, value); }
        }

        static EndPoint()
        {
            endPointImages = new Dictionary<EndPointColors, BitmapImage>
            {
                { EndPointColors.Beige,  new BitmapImage(new Uri("Images/EndPoint_Beige.png",  UriKind.Relative)) },
                { EndPointColors.Black,  new BitmapImage(new Uri("Images/EndPoint_Black.png",  UriKind.Relative)) },
                { EndPointColors.Blue,   new BitmapImage(new Uri("Images/EndPoint_Blue.png",   UriKind.Relative)) },
                { EndPointColors.Brown,  new BitmapImage(new Uri("Images/EndPoint_Brown.png",  UriKind.Relative)) },
                { EndPointColors.Gray,   new BitmapImage(new Uri("Images/EndPoint_Gray.png",   UriKind.Relative)) },
                { EndPointColors.Purple, new BitmapImage(new Uri("Images/EndPoint_Purple.png", UriKind.Relative)) },
                { EndPointColors.Red,    new BitmapImage(new Uri("Images/EndPoint_Red.png",    UriKind.Relative)) },
                { EndPointColors.Yellow, new BitmapImage(new Uri("Images/EndPoint_Yellow.png", UriKind.Relative)) },
                { EndPointColors.Null, new BitmapImage() }
            };

            EndPointColorProperty = DependencyProperty.Register("EndPointColor", typeof(EndPointColors), typeof(EndPoint), new FrameworkPropertyMetadata(EndPointColors.Null, new PropertyChangedCallback(OnEndPointColorChanged)));
        }

        public EndPoint()
        {
            InitializeComponent();
        }

        private static void OnEndPointColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EndPoint endPoint = (EndPoint)sender;
            EndPointColors color = endPoint.EndPointColor;

            endPoint.imgEndPoint.Source = endPointImages[color];
        }
    }
}
