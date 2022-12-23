using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Graphics3
{
   public partial class MainWindow : Window
   {
      private void saveButton_Click(object sender, RoutedEventArgs e)
      {
         SaveFileDialog saveFileDialog = new SaveFileDialog();
         saveFileDialog.Filter = "Points files(*.pt2d)|*.pt2d";
         saveFileDialog.ShowDialog();

         string filename = saveFileDialog.FileName;
         if (filename == "")
         {
            MessageBox.Show("File name cannot be empty");
            return;
         }

         string FileContent = "";

         foreach (Point2D data in pointManager.SelectedPoints)
         {
            FileContent += $"{data.X} {data.Y}\n";
         }

         File.WriteAllText(filename, FileContent);

         pointManager.Files.Add(filename);
      }

      private void loadButton_Click(object sender, RoutedEventArgs e)
      {
         var openFileDialog = new OpenFileDialog();

         openFileDialog.Filter = "Points files(*.pt2d)|*.pt2d";
         openFileDialog.ShowDialog();
         string filename = openFileDialog.FileName;
         if (filename == "")
         {
            MessageBox.Show("File name cannot be empty");
            return;
         }
         if (pointManager.Files.Contains(filename))
         {
            MessageBox.Show("File is already opened");
            return;
         }
         string FileContent = File.ReadAllText(filename);
         var words = FileContent.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

         if (words.Length == 0)
         {
            MessageBox.Show("File is empty");
            return;
         }
         try
         {
            var pointSet = new ObservableCollection<Point2D>();
            for (int i = 0; i < words.Length; i += 2)
            {
               double x, y;
               x = double.Parse(words[i]);
               y = double.Parse(words[i + 1]);
               pointSet.Add(new Point2D(x, y));
            }

            Random rand = new Random();
            pointManager.Files.Add(filename);
            pointManager.PointSets[filename] = pointSet;
            pointManager.PointColors[filename] = new SolidColorBrush(Color.FromArgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255), 0));
            pointManager.SelectedPoints = pointSet;
            DataGrid1.ItemsSource = pointManager.SelectedPoints;
         }
         catch
         {
            MessageBox.Show($"File {filename} is corrupted");
         }
      }

      private void addButton_Click(object sender, RoutedEventArgs e) =>
         pointManager.SelectedPoints.Add(new Point2D(0, 0));

      private void clearButton_Click(object sender, RoutedEventArgs e)
      {
         pointManager.SelectedFile = "";
         pointManager.SelectedPoints = null;
         pointManager.Files.Clear();
         //viewModel.Files.Add("new file");
         pointManager.PointSets.Clear();
         pointManager.PointColors.Clear();
         pointManager.SelectedPoints = new ObservableCollection<Point2D>()
         {
               new Point2D(1d, 1d),
               new Point2D(2d, 4d),
               new Point2D(3d, 9d),
               new Point2D(4d, 16d)
         };
         DataGrid1.ItemsSource = pointManager.SelectedPoints;
         ListBox1.ItemsSource = pointManager.Files;
      }

      private void deleteFilesButton_Click(object sender, RoutedEventArgs e)
      {
         
         switch (MessageBox.Show($"Delete this ({ListBox1.SelectedItems.Count}) files?", 
                                 $"Removing files ({ListBox1.SelectedItems.Count})", MessageBoxButton.OKCancel))
         {
            case MessageBoxResult.OK: 
            {
               List<string> strings = new List<string>();
               foreach (var item in ListBox1.SelectedItems)
                  strings.Add(item as string);

               foreach (var file in strings)
               {
                  pointManager.Files.Remove(file);
                  pointManager.PointSets.Remove(file);
                  pointManager.PointColors.Remove(file);
               }
               break;
            }
            case MessageBoxResult.Cancel:
               return;
            case MessageBoxResult.None:
               return;
         }

      }

      private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         int c;
         if ((c = ListBox1.SelectedItems.Count) > 0)
         {
            pointManager.SelectedFile = ListBox1.SelectedItems[c - 1].ToString();
            pointManager.SelectedPoints = pointManager.PointSets[pointManager.SelectedFile];
            DataGrid1.ItemsSource = pointManager.SelectedPoints;
         }
         DrawGraphs();

      }

   }
}
