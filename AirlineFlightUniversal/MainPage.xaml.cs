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
