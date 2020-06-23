using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Ukol.Models
{
    public static class XmlData
    {
        //Create a new default XML file with cars
        public static void CreateXmlWithCars()
        {
            List<Car> carsList = new List<Car>() {
                new Car("Škoda Oktávia", new DateTime(2010, 12, 2), 500000, 20),
                new Car("Škoda Felicia", new DateTime(2000, 12, 3), 210000, 20),
                new Car("Škoda Fabia", new DateTime(2010, 12, 4), 350000, 20),
                new Car("Škoda Oktávia", new DateTime(2010, 12, 4), 500000, 20),
                new Car("Škoda Oktávia", new DateTime(2010, 12, 5), 500000, 20),
                new Car("Škoda Fabia", new DateTime(2010, 12, 5), 350000, 20),
                new Car("Škoda Fabia", new DateTime(2010, 12, 6), 350000, 20),
                new Car("Škoda Forman", new DateTime(2000, 12, 4), 100000, 19),
                new Car("Škoda Favorit", new DateTime(2000, 12, 5), 80000, 19),
                new Car("Škoda Forman", new DateTime(2000, 12, 6), 100000, 19),
                new Car("Škoda Felicia", new DateTime(2000, 12, 3), 210000, 19),
                new Car("Škoda Felicia", new DateTime(2000, 12, 2), 210000, 19),
                new Car("Škoda Oktávia", new DateTime(2010, 12, 7), 500000, 20),
            };

            XDocument doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("cars",
                    carsList.Select(u => new XElement("car",
                        new XElement("ModelName", u.ModelName),
                        new XElement("DateOfSale", u.DateOfSale.ToShortDateString()),
                        new XElement("Price", u.Price),
                        new XElement("DPH", u.DPH))))
                );

            try {
                doc.Save("cars.xml");

            }
            catch (Exception ex){
                System.Windows.MessageBox.Show("Error =" + (ex));
            }
        }
        //load XML file based on the name and pass collection to ViewList
        public static List<Car> LoadXml(string fileName)
        {
            List<Car> cars = new List<Car>();

            XDocument doc = XDocument.Load(fileName);

            foreach (XElement u in doc.Element("cars").Elements("car"))
            {
                cars.Add(new Car(
                    u.Element("ModelName").Value,
                    DateTime.Parse(u.Element("DateOfSale").Value),
                    double.Parse(u.Element("Price").Value),
                    double.Parse(u.Element("DPH").Value))
                );
            }
            return cars;
        }
        //adds car to loaded file
        public static void AddCarToXML(string fileName, string carName, string carDate, string carPrice, string carDph)
        {
            XDocument doc = XDocument.Load(fileName);
            doc.Element("cars").Add
                    (
                        new XElement("car",
                        new XElement("ModelName", carName),
                        new XElement("DateOfSale", Convert.ToDateTime(carDate).ToString("yyyy/MM/dd")),
                        new XElement("Price", double.Parse(carPrice)),
                        new XElement("DPH", double.Parse(carDph)))
                    );
            doc.Save(fileName);
        }
        //calculate "DPH" value for each car and return new collection for ViewList
        public static List<CarViewModel> ViewCars(List<Car> cars) 
        {
            var carsView = new List<CarViewModel>();

            foreach (var c in cars)
            {
                carsView.Add(new CarViewModel
                {
                    ModelName = c.ModelName,
                    DPH = c.DPH,
                    PriceNoDPH = c.Price,
                    PriceWithDph = c.Price + (c.Price / 100 * c.DPH)
                });
            }
            return carsView;
        }
    }
}
