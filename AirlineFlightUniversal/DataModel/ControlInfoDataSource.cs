﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace AirlineFlightUniversal.DataModel
{
    public class ControlInfoDataItem
    {
        public ControlInfoDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Content = content;
            this.Docs = new ObservableCollection<ControlInfoDocLink>();
            this.RelatedControls = new ObservableCollection<string>();
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public string Content { get; private set; }
        public ObservableCollection<ControlInfoDocLink> Docs { get; private set; }
        public ObservableCollection<string> RelatedControls { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public class ControlInfoDocLink
    {
        public ControlInfoDocLink(string title, string uri)
        {
            this.Title = title;
            this.Uri = uri;
        }
        public string Title { get; private set; }
        public string Uri { get; private set; }
    }


    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class ControlInfoDataGroup
    {
        public ControlInfoDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        //public ObservableCollection<ControlInfoDataItem> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with content read from a static json file.
    /// 
    /// ControlInfoSource initializes with data read from a static json file included in the 
    /// project.  This provides sample data at both design-time and run-time.
    /// </summary>
    public sealed class ControlInfoDataSource
    {
        private static ControlInfoDataSource _controlInfoDataSource = new ControlInfoDataSource();
        private static readonly object _lock = new object();

        private ObservableCollection<ControlInfoDataGroup> _groups = new ObservableCollection<ControlInfoDataGroup>();
        public ObservableCollection<ControlInfoDataGroup> Groups
        {
            get { return this._groups; }
        }

        public static async Task<IEnumerable<ControlInfoDataGroup>> GetGroupsAsync()
        {
            await _controlInfoDataSource.GetControlInfoDataAsync();

            return _controlInfoDataSource.Groups;
        }

        public static async Task<ControlInfoDataGroup> GetGroupAsync(string uniqueId)
        {
            await _controlInfoDataSource.GetControlInfoDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _controlInfoDataSource.Groups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<ControlInfoDataItem> GetItemAsync(string uniqueId)
        {
            await _controlInfoDataSource.GetControlInfoDataAsync();
            // Simple linear search is acceptable for small data sets
            //var matches = _controlInfoDataSource.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            //if (matches.Count() > 0) return matches.First();
            return null;
        }

        private async Task GetControlInfoDataAsync()
        {
            lock (_lock)
            {
                if (this._groups.Count != 0)
                    return;
            }

            Uri dataUri = new Uri("ms-appx:///DataModel/ControlInfoData.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);

            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Groups"].GetArray();

            lock (_lock)
            {
                foreach (JsonValue groupValue in jsonArray)
                {
                    JsonObject groupObject = groupValue.GetObject();
                    ControlInfoDataGroup group = new ControlInfoDataGroup(groupObject["UniqueId"].GetString(),
                                                                          groupObject["Title"].GetString(),
                                                                          groupObject["Subtitle"].GetString(),
                                                                          groupObject["ImagePath"].GetString(),
                                                                          groupObject["Description"].GetString());


                    //foreach (JsonValue itemValue in groupObject["Items"].GetArray())
                    //{
                    //    JsonObject itemObject = itemValue.GetObject();
                    //    var item = new ControlInfoDataItem(itemObject["UniqueId"].GetString(),
                    //                                            itemObject["Title"].GetString(),
                    //                                            itemObject["Subtitle"].GetString(),
                    //                                            itemObject["ImagePath"].GetString(),
                    //                                            itemObject["Description"].GetString(),
                    //                                            itemObject["Content"].GetString());

                    //    group.Items.Add(item);
                    //}
                if (!this.Groups.Any(g => g.Title == group.Title))
                        this.Groups.Add(group);
                }
            }
        }
    }
}
