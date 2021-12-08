using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Reflection;
using System.Globalization;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace WartungsLOG.MyConverter
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if(parameter is byte[] bytearray)
            //{
            //bytearray.
            if (value != null) { 
                byte[] bytearray = (byte[])value;

                // Problem with Exception: ~ stream already closed
                //var ms = new MemoryStream(bytearray);
                //Debug.WriteLine($"XXXX: MyConverter: should be okay size: byte[] {bytearray.Length} memorystream: {ms.Length}");
                //return ImageSource.FromStream(() => ms);
                //return ImageSource.FromStream(() => new MemoryStream(bytearray)); // maybe

                Debug.WriteLine($"XXXX: MyConverter: should be okay size: byte[] {bytearray.Length}");
                SKBitmap skbitmap = SKBitmap.Decode(bytearray);
                return (SKBitmapImageSource)skbitmap;



            }
            Debug.WriteLine("XXXX: MyConverter: oh it's null");
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
