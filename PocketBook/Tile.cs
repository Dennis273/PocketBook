using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Media.Imaging;
using System;

namespace PocketBook
{
    class Tile
    {
        public static void TileNotificate(float cost, float budget) {

            string source = "ms-appx:///Assets/blue.png";
            
            string content = $@"
<tile>
    <visual>
 
        <binding template='TileMedium' color='#ff0000'>
            <text hint-style='subtitle'>今日消费:</text>
            <image src='{source}' placement='background'/>
            <text hint-style='captionSubtle'>已消费:{cost}</text>
            <text hint-style='captionSubtle'>日均消费:{budget}</text>
        </binding>
 
        <binding template='TileWide'>
            <text hint-style='subtitle'>今日消费:</text>
            <image src='{source}' placement='background'/>
            <text hint-style='bodySubtle' hint-align='center'>已消费:{cost}</text>
            <text hint-style='bodySubtle' hint-align='center'>日均消费:{budget}</text>
        </binding>
        
        <binding template='TileLarge'>
            <text hint-style='subtitle'>今日消费:</text>
            <image src='{source}' placement='background'/>
            <text hint-style='bodySubtle' hint-align='center'>已消费:{cost}</text>
            <text hint-style='bodySubtle' hint-align='center'>日均消费:{budget}</text>
        </binding>
        
         <binding template='TileSmall'>
            <image src='{source}' placement='background'/>
            <text hint-style='captionSubtle'>{cost}</text>
            <text hint-style='captionSubtle'>{budget}</text>
        </binding>
    </visual>
</tile>";
            // Load the string into an XmlDocument
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);

            // Then create the tile notification
            var notification = new TileNotification(doc);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }
    }
}