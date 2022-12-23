using System.Windows;

namespace Graphics3
{
   /// <summary>
   /// Логика взаимодействия для MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public PointManager pointManager;
      public MainWindow()
      {
         InitializeComponent();
         pointManager = new PointManager();
         DataGrid1.ItemsSource = pointManager.SelectedPoints;
         ListBox1.ItemsSource = pointManager.Files;
         InitCanvas();
         DrawGraphs();
      }

   }
   
  

}
