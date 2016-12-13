using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace weatherForRA0UEK
{
    public class Info
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Elevation { get; set; }
        public string IATA { get; set; }
        public string ICAO { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public string State { get; set; }
    }
    
    public class Translations
    {
        public string Altimeter { get; set; }
        public string Clouds { get; set; }
        public string Dewpoint { get; set; }
        public string Other { get; set; }
        public string Temperature { get; set; }
        public string Visibility { get; set; }
        public string Wind { get; set; }
    }

    public class Units
    {
        public string Altimeter { get; set; }
        public string Altitude { get; set; }
        public string Temperature { get; set; }
        public string Visibility { get; set; }
        public string WindSpeed { get; set; }
    }

    public class Weather
    {
        public string Altimeter { get; set; }
        public List<object> CloudList { get; set; }
        public string Dewpoint { get; set; }
        public string FlightRules { get; set; }
        public Info Info { get; set; }
        public List<object> OtherList { get; set; }
        public string RawReport { get; set; }
        public string Remarks { get; set; }
        public List<object> RunwayVisList { get; set; }
        public string Station { get; set; }
        public string Temperature { get; set; }
        public string Time { get; set; }
        public Translations Translations { get; set; }
        public Units Units { get; set; }
        public string Visibility { get; set; }
        public string WindDirection { get; set; }
        public string WindGust { get; set; }
        public string WindSpeed { get; set; }
        public List<object> WindVariableDir { get; set; }
    }
}
