﻿using MetOfficeDataPoint.Models;
using MetOfficeDataPoint.Models.GeoCoordinate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace MetOfficeDataPoint
{
    public class MetOfficeDataPointClient
    {
        private readonly string _apiKey;

        public MetOfficeDataPointClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<SiteListResponse> GetAllSites(string resolution = "daily")
        {
            string url = "http://datapoint.metoffice.gov.uk/public/data/val/wxfcs/all/json/sitelist?res=" + resolution + "&key=" + _apiKey;

            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            SiteListResponse siteListResponse = JsonConvert.DeserializeObject<SiteListResponse>(responseString);

            return siteListResponse;
        }

        public async Task<AvailableTimeStampsResponse> GetAvailableTimestamps(string resolution = "3hourly")
        {
            string url = "http://datapoint.metoffice.gov.uk/public/data/val/wxfcs/all/json/capabilities?res=" + resolution + "&key=" + _apiKey;

            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            AvailableTimeStampsResponse siteListResponse = JsonConvert.DeserializeObject<AvailableTimeStampsResponse>(responseString);

                return siteListResponse;
        }

        public async Task<ForecastResponse3Hourly> GetForecasts3Hourly(string time = null, int locationId = 0)
        {
            string url = "http://datapoint.metoffice.gov.uk/public/data/val/wxfcs/all/json/";

            if (locationId > 0)
            {
                url += locationId.ToString() + "?res=3hourly" + "&key=" + _apiKey;
            }
            else
            {
                url += "all?res=3hourly" + "&key=" + _apiKey;
            }

            if (!string.IsNullOrEmpty(time))
            {
                url += "&time=" + time;
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            ForecastResponse3Hourly forecastResponse = JsonConvert.DeserializeObject<ForecastResponse3Hourly>(responseString);

                return forecastResponse;
            
        }

        public async Task<ForecastResponseDaily> GetForecastsDaily(int locationId = 0)
        {
            string url = "http://datapoint.metoffice.gov.uk/public/data/val/wxfcs/all/json/";

            if (locationId > 0)
            {
                url += locationId.ToString() + "?res=daily" + "&key=" + _apiKey;
            }
            else
            {
                url += "all?res=daily" + "&key=" + _apiKey;
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            ForecastResponseDaily forecastResponse = JsonConvert.DeserializeObject<ForecastResponseDaily>(responseString);

                return forecastResponse;
            
        }

        public async Task<ForecastResponse3Hourly> GetHistoricalObservations(string resolution = "hourly", int locationId = 0)
        {
            string url = "http://datapoint.metoffice.gov.uk/public/data/val/wxobs/all/json/";

            if (locationId > 0)
            {
                url += locationId.ToString() + "?res=" + resolution + "&key=" + _apiKey;
            }
            else
            {
                url += "all/?res=" + resolution + "&key=" + _apiKey;
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            ForecastResponse3Hourly forecastResponse = JsonConvert.DeserializeObject<ForecastResponse3Hourly>(responseString);

                return forecastResponse;
            
        }

        public async Task<Location> GetClosestSite(GeoCoordinate coordinate)
        {
            // Create client
            MetOfficeDataPointClient client = new MetOfficeDataPointClient(_apiKey);

            // Get all sites
            SiteListResponse siteListResponse = await client.GetAllSites();

            // Find nearest coordinates
            GeoCoordinate nearest = siteListResponse.Locations.Location.Select(x => new GeoCoordinate(x.Latitude, x.Longitude)).OrderBy(x => x.GetDistanceTo(coordinate)).First();

            // Find site matching nearest coordinates
            Location location = siteListResponse.Locations.Location.Find(x => x.Latitude == nearest.Latitude && x.Longitude == nearest.Longitude);

            return location;
        }
    }
}
