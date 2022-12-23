using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphics3
{
   public partial class MainWindow : Window
   {
      Vector Origin, mouseDownPos, prevMousePos;
      bool LMB_pressed = false, RMB_pressed = false;
      double PixelsByUnit = 37;
      double Zoom = 1.0, Ratio = 1.0;
      public void InitCanvas()
      {
         Origin = new Vector(canvas.ActualWidth / PixelsByUnit / 2, canvas.ActualHeight / PixelsByUnit / 2);
         mouseDownPos = new Vector(0, 0);
      }
      public void DrawGraphs()
      {
         Ratio = canvas.ActualWidth / canvas.ActualHeight;
         Title = $"Origin: {Origin.X / PixelsByUnit} {Origin.Y / PixelsByUnit} Zoom: {Zoom}";
         canvas.Children.Clear();
         double canvasR = canvas.ActualWidth,
                canvasL = 0,
                canvasU = canvas.ActualHeight,
                canvasD = 0;

         foreach (var ps in ListBox1.SelectedItems)
         {
            
            PointCollection points = new PointCollection();
            foreach (Point2D p in pointManager.PointSets[ps as string])
               //points.Add(new Point(p.X + Origin.X, p.Y + Origin.Y));
               points.Add(new Point(p.X * PixelsByUnit * Zoom + Origin.X,
                                    canvas.ActualHeight - p.Y * PixelsByUnit * Zoom - Origin.Y) );

            Polyline polyline = new Polyline
            {
               Points = points,
               StrokeThickness = 2,
               Stroke = pointManager.PointColors[ps as string],

            };
            //Canvas.SetLeft(polyline, Origin.X);
            //Canvas.SetBottom(polyline, Origin.Y);
            canvas.Children.Add(polyline);
           
         }

      }
      private void canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
      {
         Vector MP = new Vector(e.GetPosition(sender as IInputElement).X, 
                                canvas.ActualHeight - e.GetPosition(sender as IInputElement).Y);
         if (LMB_pressed = e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
         {
            Origin.X += -(prevMousePos.X - MP.X);
            Origin.Y += -(prevMousePos.Y - MP.Y);
            //DrawGraphs();
         }
         textBlock.Text = $"X: {(MP.X - Origin.X) / Zoom / PixelsByUnit}  \nY: {(MP.Y - Origin.Y) /Zoom / PixelsByUnit} ";
         prevMousePos = MP;
         DrawGraphs();
      }
      private void canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
      {
         mouseDownPos.X = e.GetPosition(sender as IInputElement).X;
         mouseDownPos.Y = e.GetPosition(sender as IInputElement).Y;

      }
      private void canvas_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
      {
         Zoom *= Math.Pow(2, e.Delta/120f/2f);
         if (Zoom < 0.01) Zoom = 0.01;
         DrawGraphs();
      }
      private void canvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
      {

      }
      private void homeButton_Click(object sender, RoutedEventArgs e)
      {
         Zoom = 1.0;
         Origin.X = 0;
         Origin.Y = 0;
         DrawGraphs();
      }
   }
}
