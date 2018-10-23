using ASM.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ASM.Views
{
    public sealed partial class ListMusic : Page
    {
        private ObservableCollection<Song> listSong;
        public static string tokenKey = null;
        //private int _currentIndex;
        internal ObservableCollection<Song> ListSong { get => listSong; set => listSong = value; }
        private Song currentSong;
        //private bool _isPlaying = false;

        public ListMusic()
        {
            this.InitializeComponent();
            ReadToken();
            this.currentSong = new Song();
            this.ListSong = new ObservableCollection<Song>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic iBi1orSXvZisyF48jdnnOYmZURdkyz3rv1Jg0KBhFLsJOa90Tlu6DxxBKPYjm4b6");
            HttpResponseMessage responseMessage = httpClient.GetAsync(Services.ServiceUrl.GET_SONG).Result;
            string content = responseMessage.Content.ReadAsStringAsync().Result;
            Debug.WriteLine(content);
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                ObservableCollection<Song> songResponse = JsonConvert.DeserializeObject<ObservableCollection<Song>>(content);
                foreach (var song in songResponse)
                {
                    this.ListSong.Add(song);
                }
                Debug.WriteLine("Success");
            }
            else
            {
                Entity.ErrorResponse errorResponse = JsonConvert.DeserializeObject<Entity.ErrorResponse>(content);
                foreach (var key in errorResponse.error.Keys)
                {
                    if (this.FindName(key) is TextBlock textBlock)
                    {
                        textBlock.Text = errorResponse.error[key];
                    }
                }
            }
        }
        public static async void ReadToken()
        {
            if (tokenKey == null)
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.GetFileAsync("token.txt");
                string content = await FileIO.ReadTextAsync(file);
                Token account_token = JsonConvert.DeserializeObject<Token>(content);
                Debug.WriteLine("token la: " + account_token.token);
                tokenKey = account_token.token;
            }
        }
        public async void GetSong()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + tokenKey);
            var response = httpClient.GetAsync(Services.ServiceUrl.GET_SONG);
            var result = await response.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(result);
            ObservableCollection<Song> list = JsonConvert.DeserializeObject<ObservableCollection<Song>>(result);
            foreach (var songs in list)
            {
                this.ListSong.Add(songs);
            }
            Debug.WriteLine(result);



        }
        private async void btn_add(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            this.currentSong.name = this.txt_name.Text;
            this.currentSong.thumbnail = this.txt_thumbnail.Text;
            this.currentSong.description = this.txt_description.Text;
            this.currentSong.singer = this.txt_singer.Text;
            this.currentSong.author = this.txt_author.Text;
            this.currentSong.link = this.txt_link.Text;

            var jsonMusic = JsonConvert.SerializeObject(this.currentSong);
            StringContent content = new StringContent(jsonMusic, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + tokenKey);
            var response = httpClient.PostAsync(Services.ServiceUrl.REGISTER_SONG, content);
            var result = await response.Result.Content.ReadAsStringAsync();
            if (response.Result.StatusCode == HttpStatusCode.Created)
            {
                Debug.WriteLine("Success");
            }
            else
            {
                ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(result);
                if (errorResponse.error.Count > 0)
                {
                    foreach (var key in errorResponse.error.Keys)
                    {
                        var objBykey = this.FindName(key);
                        var value = errorResponse.error[key];
                        if (objBykey != null)
                        {
                            TextBlock textBlock = objBykey as TextBlock;
                            textBlock.Text = "* " + value;
                        }
                    }
                }
            }
            this.txt_name.Text = String.Empty;
            this.txt_description.Text = String.Empty;
            this.txt_singer.Text = String.Empty;
            this.txt_author.Text = String.Empty;
            this.txt_thumbnail.Text = String.Empty;
            this.txt_link.Text = String.Empty;
        }
        private void btn_reset(object sender, RoutedEventArgs e)
        {
            this.txt_name.Text = String.Empty;
            this.txt_description.Text = String.Empty;
            this.txt_singer.Text = String.Empty;
            this.txt_author.Text = String.Empty;
            this.txt_thumbnail.Text = String.Empty;
            this.txt_link.Text = String.Empty;
        }
    }
}