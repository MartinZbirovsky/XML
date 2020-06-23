using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using Ukol.Models;

namespace Ukol
{
    public partial class MainWindow : Window
    {
        private OpenFileDialog openFile;

        public MainWindow()
        {
            InitializeComponent();
        }

        //create default XML with data
        private void btnCreateXML_Click(object sender, RoutedEventArgs e)
        {
            XmlData.CreateXmlWithCars();
            if (File.Exists("cars.xml"))
            {
                btnCreateXML.Content = "XML vytvoreno";
                btnCreateXML.Background = Brushes.Green;
                fileName.Text = Path.GetFullPath("cars.xml");
            }
        }
        //load XML file and display data in carListView and grCarList 
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            openFile = new OpenFileDialog();

            var cars = new List<Car>();

            if (openFile.ShowDialog() == true)
            {
                fileName.Text = openFile.FileName;
                cars = XmlData.LoadXml(openFile.FileName);

                carListView.ItemsSource = cars;
                btnAddCar.IsEnabled = true;
                btnInfo.Content = "";
                btnDeleteCar.IsEnabled = true;
            }

            grCarList.ItemsSource =
                (from c in XmlData.ViewCars(cars)
                group c by c.ModelName into g
                select new  
                    {
                        ModelName = g.Key,
                        dph = g.Select(c => c.DPH),
                        priceNoDPH = g.Sum(c => c.PriceNoDPH),
                        priceWithDPH = g.Sum(c => c.PriceWithDph)
                }).OrderByDescending(x=>x.priceWithDPH);

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(grCarList.ItemsSource);
            PropertyGroupDescription group = new PropertyGroupDescription("ModelName");
            view.GroupDescriptions.Add(group);
        }
        // call "AddCarToXML" and add car into XML after refresh tables
        private void btnAddCar_Click(object sender, RoutedEventArgs e)
        {
            var cars = new List<Car>();

            if (String.IsNullOrEmpty(nameTextBox.Text) || 
                String.IsNullOrEmpty(dateTextBox.Text) || 
                String.IsNullOrEmpty(priceTextBox.Text) || 
                String.IsNullOrEmpty(dphTextBox.Text))
            {
                btnInfo.Content = "Zadejte vsechny hodnoty";
            }
            else
            {
                XmlData.AddCarToXML(openFile.FileName, nameTextBox.Text, dateTextBox.Text, priceTextBox.Text, dphTextBox.Text);
            }
            
            cars = XmlData.LoadXml(openFile.FileName);
            carListView.ItemsSource = cars;

            grCarList.ItemsSource =
                (from c in XmlData.ViewCars(cars)
                group c by c.ModelName into g
                select new
                {
                    ModelName = g.Key,
                    dph = g.Select(c => c.DPH),
                    priceNoDPH = g.Sum(c => c.PriceNoDPH),
                    priceWithDPH = g.Sum(c => c.PriceWithDph)
                }).OrderByDescending(x => x.priceWithDPH);

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(grCarList.ItemsSource);
            PropertyGroupDescription group = new PropertyGroupDescription("ModelName");
            view.GroupDescriptions.Add(group);
        }
        //delete the model by the specified name
        private void btnDeleteCar_Click(object sender, RoutedEventArgs e)
        {
            var cars = new List<Car>();

            XDocument dokument = XDocument.Load(openFile.FileName);
                (from u in dokument.Element("cars").Elements("car")
                 where u.Element("ModelName").Value == deleteTextBox.Text
                 select u).Remove();

            dokument.Save(openFile.FileName);

            cars = XmlData.LoadXml(openFile.FileName);
            carListView.ItemsSource = cars;

            grCarList.ItemsSource =
               (from c in XmlData.ViewCars(cars)
                group c by c.ModelName into g
                select new
                {
                    ModelName = g.Key,
                    dph = g.Select(c => c.DPH),
                    priceNoDPH = g.Sum(c => c.PriceNoDPH),
                    priceWithDPH = g.Sum(c => c.PriceWithDph)
                }).OrderByDescending(x => x.priceWithDPH);

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(grCarList.ItemsSource);
            PropertyGroupDescription group = new PropertyGroupDescription("ModelName");
            view.GroupDescriptions.Add(group);
        }
    }
}
