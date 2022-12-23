using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Graphics3
{
   public class PointManager : INotifyPropertyChanged
   {
      private ObservableCollection<Point2D> selectedPoints = new ObservableCollection<Point2D>();
      private ObservableCollection<string> files;
      public Dictionary<string, ObservableCollection<Point2D>> PointSets = new Dictionary<string, ObservableCollection<Point2D>>();
      public Dictionary<string, SolidColorBrush> PointColors = new Dictionary<string, SolidColorBrush>();
      public ObservableCollection<string> Files
      {
         get => files;
         set
         {
            files = value;
            OnPropertyChanged();
         }
      }

      public string SelectedFile { get; set; } = "";
      public ObservableCollection<Point2D> SelectedPoints
      {
         get => selectedPoints;
         set
         {
            selectedPoints = value;
            OnPropertyChanged();

         }
      }
      public PointManager()
      {
         selectedPoints = new ObservableCollection<Point2D>
            {
               new Point2D(1d, 1d),
               new Point2D(2d, 4d),
               new Point2D(3d, 9d),
               new Point2D(4d, 16d)
            };
         files = new ObservableCollection<string>();
      }
      public event PropertyChangedEventHandler PropertyChanged;
      public void OnPropertyChanged([CallerMemberName] string prop = "") =>
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
   }
   public class Point2D : INotifyPropertyChanged
   {
      private double x, y;
      public double X
      {
         get => x;
         set
         {
            x = value;
            OnPropertyChanged();
         }
      }
      public double Y
      {
         get => y;
         set
         {
            y = value;
            OnPropertyChanged();
         }
      }
      public Point2D(double x, double y)
      {
         this.x = x;
         this.y = y;
      }
      public event PropertyChangedEventHandler PropertyChanged;
      public void OnPropertyChanged([CallerMemberName] string prop = "") =>
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
   }
}
