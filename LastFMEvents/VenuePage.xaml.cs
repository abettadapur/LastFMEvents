using Bing.Maps;
using Callisto.Controls;
using MusicInformation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace LastFMEvents
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class VenuePage : LastFMEvents.Common.LayoutAwarePage
    {
        GigVenue currentVenue;
        public const string MAPS_KEY = "An7YFWgFf8yiMsYq3JyjFvxNIaqYKWzD2dra2JgZO8dVqPuSamWUPJbsdkckX2Gr";
        SettingsFlyout sf;
        public VenuePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            GigVenue sentVenue = navigationParameter as GigVenue;
            pageTitle.Text = sentVenue.Title;
            
            Gigs gigs = await Queries.VenueDetailsAsync(sentVenue.id);
            currentVenue = gigs.events.gigs[0].venue;
            itemListView.ItemsSource = gigs.events.gigs;
            loadingBar.Visibility = Visibility.Collapsed;
            
            
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void itemListView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Gig g = e.ClickedItem as Gig;
            sf = new SettingsFlyout();
            createSettingsFlyout(sf, g);
            sf.IsOpen = true;
        }
        private void createSettingsFlyout(SettingsFlyout sf, Gig g)
        {
            sf.HeaderText = g.title;
            sf.FlyoutWidth = SettingsFlyout.SettingsFlyoutWidth.Wide;
            StackPanel sp = new StackPanel() { Orientation = Orientation.Vertical };
            #region Venue
            sp.Children.Add(new TextBlock() { Text = "Venue", Style = this.Resources["HeaderStyle"] as Style });
            if (g.venue.url != null)
            {
                HyperlinkButton hb = new HyperlinkButton();
                hb.Content = g.venue.Title;
                hb.Tag = g.venue.id;
                hb.Style = this.Resources["HyperStyle"] as Style;
                hb.Click += hb_Click_Venue;
                sp.Children.Add(hb);
            }
            else
            {
                sp.Children.Add(new TextBlock { Text = g.venue.Title, Style = this.Resources["ContentStyle"] as Style });
            }
            sp.Children.Add(new TextBlock() { Text = "Artists", Style = this.Resources["HeaderStyle"] as Style });
            #endregion
            #region Artists
            StackPanel artistPanel = new StackPanel();
            foreach (string s in g.artists.artist)
            {
                HyperlinkButton hb = new HyperlinkButton() { Content = s, Style = this.Resources["HyperStyle"] as Style };
                hb.Click += hb_Click;
                artistPanel.Children.Add(hb);
            }
            sp.Children.Add(artistPanel);
            #endregion
            #region Street
            if (g.venue.location.street != "")
            {
                sp.Children.Add(new TextBlock() { Text = "Address", Style = this.Resources["HeaderStyle"] as Style });
                sp.Children.Add(new TextBlock() { Text = g.venue.location.street, Style = this.Resources["ContentStyle"] as Style });
            }
            #endregion
            #region City
            sp.Children.Add(new TextBlock() { Text = "City", Style = this.Resources["HeaderStyle"] as Style });
            sp.Children.Add(new TextBlock() { Text = g.venue.location.city, Style = this.Resources["ContentStyle"] as Style });
            #endregion
            #region Time
            sp.Children.Add(new TextBlock() { Text = "Time", Style = this.Resources["HeaderStyle"] as Style });
            sp.Children.Add(new TextBlock() { Text = g.startDate.Substring(0, g.startDate.Length - 3), Style = this.Resources["ContentStyle"] as Style });
            #endregion
            sp.Children.Add(new TextBlock() { Text = "More Details", Style = this.Resources["HeaderStyle"] as Style });
            sp.Children.Add(new HyperlinkButton() { Content = g.url, NavigateUri = new Uri(g.url), Style = this.Resources["HyperStyle"] as Style });

            Map m = new Map();
            Pushpin p = new Pushpin();
            MapLayer.SetPosition(p, new Location(double.Parse(g.venue.location.point.lat), double.Parse(g.venue.location.point.longt)));
            m.Children.Add(p);
            m.ZoomLevel = 15;
            m.Credentials = MAPS_KEY;
            m.ShowScaleBar = false;
            m.ShowNavigationBar = false;
            m.SetView(new Location(double.Parse(g.venue.location.point.lat), double.Parse(g.venue.location.point.longt)));
            sp.Children.Add(m);









            sf.ContentBackgroundBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            sf.Content = sp;
        }

        void hb_Click(object sender, RoutedEventArgs e)
        {
            sf.IsOpen = false;
            this.Frame.Navigate(typeof(ArtistDetailPage), new Artist() { Title = (e.OriginalSource as HyperlinkButton).Content as string });
        }
        void hb_Click_Venue(object sender, RoutedEventArgs e)
        {
            sf.IsOpen = false;
            this.Frame.Navigate(typeof(VenuePage), new GigVenue() { Title = (e.OriginalSource as HyperlinkButton).Content as string, id = (e.OriginalSource as HyperlinkButton).Tag as string });
        }
    }
}
