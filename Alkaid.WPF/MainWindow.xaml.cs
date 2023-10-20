using Alkaid.Core;
using Alkaid.Core.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace Alkaid.WPF {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        RawImage output;
        Camera MainCam;
        Scene world;


        Bitmap bitmap;
        public MainWindow() {
            InitializeComponent();
            (MainCam, world) = FileIO.Parse("./Assets/hw2_input.txt");
        }

        private void RenderBtn_Click(object sender, RoutedEventArgs e) {
            MainCam.Initialize();
            output =  MainCam.Render(world);
            bitmap = Utility.RawImageToBitmap(output);
            Utility.UpdateImageBox(RenderImgBox, bitmap);
        }
        
    }
}
