using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using MetOfficeDataPoint;
using MetOfficeDataPoint.Models;
using MetOfficeDataPoint.Models.GeoCoordinate;
using System.IO;
using System.Globalization;

namespace MetOfficeApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region Setup of Globals
        //Random global variable for neatness
        TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
        #endregion
        #region Dictionary setup
        //Setup dictionaries
        private Dictionary<string, string> weather_codes = new Dictionary<string, string>();
        private Dictionary<string, string> uv = new Dictionary<string, string>();
        private Dictionary<string, string> visibility = new Dictionary<string, string>();
        private void GetDict()
        {
            //Setup Weather Codes
            var weather_reader = new StreamReader(File.OpenRead("Dictionaries/Weather_Codes.txt"));
            weather_codes.Clear(); //Clear dictionary, to stop exception occuring if both buttons are pressed in one session
            while (!weather_reader.EndOfStream)
            {
                var line = weather_reader.ReadLine();
                var values = line.Split(',');
                weather_codes.Add(values[0], values[1]);
            }
            //Setup Visibility Codes
            var visibility_reader = new StreamReader(File.OpenRead("Dictionaries/Visibility.txt"));
            visibility.Clear(); //Clear dictionary, to stop exception occuring if both buttons are pressed in one session
            while (!visibility_reader.EndOfStream)
            {
                var line = visibility_reader.ReadLine();
                var values = line.Split(',');
                visibility.Add(values[0], values[1]);
            }
            //Setup UV Codes (not used, seemed unneeded)
            var uv_reader = new StreamReader(File.OpenRead("Dictionaries/UV.txt"));
            uv.Clear(); //Clear dictionary, to stop exception occuring if both buttons are pressed in one session
            while (!uv_reader.EndOfStream)
            {
                var line = uv_reader.ReadLine();
                var values = line.Split(',');
                uv.Add(values[0], values[1]);
            }
        }
        #endregion

        /// <summary>
        /// This function strips JSON from postcodes.io API
        /// </summary>
        /// <returns>
        /// Returns a Tuple, with latitude, longitude and the original postcode.
        /// </returns>
        private (float lat, float lon,string postcode) LongLat()
        {
            string postcode = postcodeBox.Text;
            string postcodeget = PCGet(postcode);
            if (String.IsNullOrEmpty(postcodeget))
            {
                throw new Exception("Malformed Postcode");
            }
            postcodeget = postcodeget.Replace("{\"status\":200,\"result\":", "");
            postcodeget = postcodeget.Replace("}}}", "}}");
            dynamic jsonboi = JsonConvert.DeserializeObject(postcodeget);
            float latitude = jsonboi.latitude;
            float longitude = jsonboi.longitude;
            return (latitude, longitude,postcode);
        }
        /// <summary>
        /// Script to make webrequest to postcodes API (May rework this to just be a GET function)
        /// </summary>
        /// <param name="postcode">
        /// The postcode the user enters
        /// </param>
        /// <returns>
        /// Returns the API response, or null if the postcode is incorrect or malformed.
        /// </returns>
        private string PCGet(string postcode)
        {
            var requesturi = String.Format("http://api.postcodes.io/postcodes/{0}", postcode); //Concatenates URL with postcode
            try //Tries to make GET Request (preferred over Async method due to hanging issues)
            {
                var request = (HttpWebRequest)WebRequest.Create(requesturi);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (WebException) //Run this if 404 error.
            {
                outputBox.Text = "Postcode was malformed or incorrect!";
                return null;
            }
        }
        /// <summary>
        /// Clears the output box, and runs the code for the three-hourly forecast
        /// </summary>
        private void ThreeHourlyFC_Click(object sender, EventArgs e)
        {
            outputBox.Clear();
            GetWeather(1);
        }
        /// <summary>
        /// Clears the output box, and just runs the code for the daily forecast
        /// </summary>
        private void DailyForecast_Click(object sender, EventArgs e)
        {
            outputBox.Clear();
            GetWeather(2);
        }
        /// <summary>
        /// The main function that actually gets the weather JSON from DataPoint, and passes it to the parser.
        /// </summary>
        /// <param name="choice">
        /// Identifies whether the user wants a daily forecast or a 3-hourly forecast
        /// </param>
        private void GetWeather(int choice)
        {
            //Run Dictionary setup function
            GetDict();
            //Make a new DataPoint Client, with the API key in the Settings file.
            MetOfficeDataPointClient client = new MetOfficeDataPointClient(Settings1.Default.mo_api_key);
            //Defines floats for try statement
            #region floats
            float lat;
            float lon;
            string postcode;
            #endregion
            //Attempt to get longitude and latitude values, along with the postcode. If successful, display in box.
            #region LongLatPost
            try 
            {
                lat = LongLat().lat;
                lon = LongLat().lon;
                postcode = LongLat().postcode;
            }
            catch (Exception) //This still runs, even after a weberror in the GET function, so catch it here aswell and exit.
            {
                outputBox.Text = "Malformed or Incorrect Postcode!";
                return; //Should exit this function and return to ready state
            }
            outputBox.Text = "Postcode: " + postcode; //Shows Postcode in text box
            outputBox.AppendText("\r\nLatitude: " + lat.ToString()); //Shows Latitude in text box
            outputBox.AppendText("\r\nLongitude: " + lon.ToString()); //Shows Longitude in text box
            #endregion
            //Get nearest weather site from coordinates from previous step.
            #region Find closest site
            GeoCoordinate coords = new GeoCoordinate(lat, lon);
            Location location = null;
            try
            {
                //Find closest weather site to postcode
                var locationone = client.GetClosestSite(coords); 
                location = locationone.Result;
            }
            catch(AggregateException)
            {
                outputBox.Text = "Your API Key is incorrect! Please change it, or reset it to default in the Settings dialog.";
                return;
            }



            #endregion
            //Obtain Data from the Met Office, and pass to the appropriate parser.
            #region Data Aquisition, and Passing to Parser
            //Defines forecastResponse variable as dynamic, so it can be used regardless of which type the user requests
            dynamic forecastResponse = null; 
            //Is choice Three Hourly?
            if (choice.Equals(1)) 
            {
                try
                {
                    var weatheresponse = client.GetForecasts3Hourly(null, location.ID);
                    forecastResponse = new ForecastResponse3Hourly();
                    forecastResponse = weatheresponse.Result;
                    HourlyParse(forecastResponse); //Run parsing for 3-hourly data
                }
                catch (WebException) //Catch incorrect API Key
                {
                    outputBox.Text = "Your API Key is incorrect! Please change it, or reset it to default in the Settings dialog.";
                }
                
            }
            //Is choice Daily?
            else if (choice.Equals(2)) 
            {
                try
                {
                    var weatheresponse = client.GetForecastsDaily(location.ID);
                    forecastResponse = new ForecastResponseDaily();
                    forecastResponse = weatheresponse.Result;
                    DailyParse(forecastResponse); //Run parsing for Daily data
                }
                catch (WebException) //Catch incorrect API Key
                {
                    outputBox.Text = "Your API Key is incorrect! Please change it, or reset it to default in the Settings dialog.";
                }
                

            }
            #endregion
        }   
        /// <summary>
        /// Parse data, if request is for 3-hourly forecast. Display neatly on screen
        /// </summary>
        /// <param name="forecastResponse">
        /// forecastResponse contains the library data for the parser to work with.
        /// </param>
        private void HourlyParse(dynamic forecastResponse)
        {
            //Run this code for each location in the request - (not used, but could be if all data was requested)
            foreach (var item in forecastResponse.SiteRep.DV.Location)
            {
                //Print Designation information (Site Name, ID, Request Type)
                #region Designations
                outputBox.AppendText("\r\n" + textInfo.ToTitleCase(item.LocationName.ToLower()) + " - Met Office Site ID:" + item.LocationId);
                outputBox.AppendText("\r\n3-Hourly " + forecastResponse.SiteRep.DV.Type);
                #endregion
                //Run this code for each day.
                foreach (var period in item.Period)
                {
                    //Parse date into dd/mm/yyyy, and print
                    DateTime datadate = DateTime.Parse(period.Value);
                    outputBox.AppendText("\r\n" + datadate.ToShortDateString());
                    //Run this code for every 3 hour interval
                    foreach (var rep in period.Rep)
                    {
                        //Work out time of day based on how many minutes after midnight
                        TimeSpan minafter = TimeSpan.FromMinutes(rep.MinutesAfterMidnight);
                        string DayTime = string.Format("{0}:0{1}", minafter.Hours, minafter.Minutes);
                        //Print time of day, Weather type, Visibility, Temperature and "Feels like" Temperature
                        outputBox.AppendText("\r\n" + DayTime + " - " + weather_codes[rep.WeatherType.ToString()]+" - Visibility "+visibility[rep.Visibility.ToString()]+"  "+rep.Temperature.ToString()+"C, Feels like "+rep.FeelsLikeTemperature+"C");
                    }
                }
            }
            //Work out a nice way to print the "Correct as of" date and time, and print it.
            DateTime fetched = DateTime.Parse(forecastResponse.SiteRep.DV.DataDate);
            outputBox.AppendText("\r\nCorrect as of - " + fetched.ToLongDateString() + " " + fetched.ToLongTimeString());
        }
        /// <summary>
        /// Parse data, if request if for daily forecast. Display neatly on screen.
        /// </summary>
        /// <param name="forecastResponse">
        /// forecastResponse contains the library data for the parser to work with.
        /// </param>
        private void DailyParse(dynamic forecastResponse)
        {
            //Run this code for each location in the request - (not used, but could be if all data was requested)
            foreach (var item in forecastResponse.SiteRep.DV.Location)
            {
                //Print Designation information (Site Name, ID, Request Type)
                #region Designations
                outputBox.AppendText("\r\n" + textInfo.ToTitleCase(item.LocationName.ToLower()) + " - Met Office Site ID:" + item.LocationId);
                outputBox.AppendText("\r\nDaily " + forecastResponse.SiteRep.DV.Type);
                #endregion
                //Run this code for each day
                foreach (var period in item.Period)
                {
                    //Parse date into dd/mm/yyyy
                    DateTime datadate = DateTime.Parse(period.Value);
                    //Print date, max temperature and low temperature
                    outputBox.AppendText("\r\n" + datadate.ToShortDateString()+"   "+period.Rep.Day.DayMaximumTemperature.ToString()+"C/"+period.Rep.Night.NightMaximumTemperature.ToString()+"C");
                    //Print Weather type, Visibility and "Feels like" Temperature for the Daytime
                    outputBox.AppendText("\r\nDay - "+ weather_codes[period.Rep.Day.WeatherType.ToString()] + " - Visibility " + visibility[period.Rep.Day.Visibility.ToString()]+" - Feels like "+period.Rep.Day.FeelsLikeDayMaximumTemperature.ToString()+"C");
                    //Print Weather type, Visibility and "Feels like" Temperature for the Nighttime
                    outputBox.AppendText("\r\nNight - " + weather_codes[period.Rep.Night.WeatherType.ToString()] + " - Visibility " + visibility[period.Rep.Night.Visibility.ToString()]+" - Feels like "+period.Rep.Night.FeelsLikeNightMaximumTemperature.ToString()+"C");
                }
            }
            //Work out a nice way to print the "Correct as of" date and time, and print it.
            DateTime fetched = DateTime.Parse(forecastResponse.SiteRep.DV.DataDate);
            outputBox.AppendText("\r\nCorrect as of - " + fetched.ToLongDateString() + " " + fetched.ToLongTimeString());
        }
        /// <summary>
        /// Run if user clicks the 'About' button. Displays about box.
        /// </summary>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }
        /// <summary>
        /// Run if user clicks the "DataPoint API Key" button under "Settings". Displays settings input.
        /// </summary>
        private void dataPointAPIKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 datapoint = new Form2();
            datapoint.Show();
        }
        /// <summary>
        /// Run if user click the "Exit" button. Exits program.
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
