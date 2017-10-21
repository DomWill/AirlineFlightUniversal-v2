using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AirlineFlightUniversal.Model;
using AirlineFlightUniversal.DataModel;
using System.ServiceModel;
using AirlineFlightUniversal.FlightXML3;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AirlineFlightUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IEnumerable<ControlInfoDataGroup> _groups;

        public MainPage()
        {
            this.InitializeComponent();

            try
            {
                BasicHttpBinding binding = new BasicHttpBinding();

                binding.MaxBufferSize = int.MaxValue;
                binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                binding.MaxReceivedMessageSize = int.MaxValue;
                binding.AllowCookies = true;

                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                EndpointAddress endpoint = new EndpointAddress("http://flightxml.flightaware.com/soap/FlightXML3/op");

                FlightXML3SoapClient client = new FlightXML3SoapClient(binding, endpoint);

                client.ClientCredentials.UserName.UserName = "domwill";
                client.ClientCredentials.UserName.Password = "94cbda5fc45127c0bb257d948ce2101c9641df78";

                FlightInfoStatusResponse response = client.FlightInfoStatusAsync("VA912", false, "", 1, 0).Result;

                foreach (var flight in response.FlightInfoStatusResult.flights)
                {
                    tblFlightCode.Text = flight.ident;
                    tbDepartureTime.Text = flight.estimated_departure_time.time;
                    tbOrigin.Text = flight.origin.airport_name;
                    tbDestination.Text = flight.destination.airport_name;
                    tbArrivalTime.Text = flight.estimated_arrival_time.time;
                    tbAircraft.Text = flight.aircrafttype;

                    //tblFlightCode.Text = $"{flight.ident} ({flight.aircrafttype})\t{flight.origin.airport_name}  Est Arrival {flight.estimated_arrival_time.time}";
                }
            }
            catch (Exception ex)
            {
                tblFlightCode.Text = $"Error {ex.Message}";
            }

        }

        public IEnumerable<ControlInfoDataGroup> Groups
        {
            get { return this._groups; }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _groups = await ControlInfoDataSource.GetGroupsAsync();
        }

        private void Control1_ItemClick(object sender, ItemClickEventArgs e)
        {
            tblFlightCode.Text = "You clicked " + e.ClickedItem.ToString() + ".";
        }

        private void Control1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridView gridView = sender as GridView;
            if (gridView != null)
            {
               // tblFlightCode.Text = string.Format("You have selected {0} item(s).", gridView.SelectedItems.Count);
            }
        }
    }
}
