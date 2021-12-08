using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text;
using Xamarin.Forms;

using WartungsLOG.Core;
using WartungsLOG.Data;
using WartungsLOG.ServiceHistory;
using SkiaSharp;
using System.Diagnostics;
using Xamarin.Essentials;
using System.IO;
using SkiaSharp.Views.Forms;

namespace WartungsLOG.Select
{
    public class AddVehicleViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navService;
        private string _kennzeichen, _description;
        private string _filesize; // save size of image to be uploaded
        private ImageSource _image; // vehicle image
        private byte[] _thumbnail_bytes; // image as byte[]

        public ICommand NavigateToNewVehicleCommand { get; }
        public ICommand AddNewVehicleCommand { get; }
        public ICommand OpenGalleryCommand { get; }
        public ICommand OpenCameraCommand { get; }

        public string KennzeichenProperty { get => _kennzeichen; set => SetProperty(ref _kennzeichen, value); }
        public string DescriptionProperty { get => _description; set => SetProperty(ref _description, value); }

        public ImageSource ImageProperty { get => _image; set => SetProperty(ref _image, value); }
        public string FilesizeProperty { get => _filesize; set => SetProperty(ref _filesize, value); }

        public AddVehicleViewModel(IDataService dataSvc, INavigationService navSvc)
        {
            _dataService = dataSvc;
            _navService = navSvc;
            
            AddNewVehicleCommand = new Command(AddVehicle);
            OpenGalleryCommand = new Command(OpenGallery);
            OpenCameraCommand = new Command(OpenCamera);

            Title = "Add Vehicle";
        }


        public override async Task Initialize()
        {
            
        }

        private async void AddVehicle(object arg) 
        {
            Vehicle v = new Vehicle { Description = DescriptionProperty, Kennzeichen = KennzeichenProperty, ImageThumbData = _thumbnail_bytes }; // fill in ID in WebAPI

            string keinbild="";
            if (v.ImageThumbData == null)
            {
                keinbild = "Achtung kein Bild! ";
            }

            var result = await Shell.Current.DisplayAlert(
            "ADD KFZ",
            "KFZ hinzufügen " + keinbild + FilesizeProperty,
            "Yes, Please!", "Nope!");

            if (result)
            {


                await _dataService.AddVehicle(v);

                await _navService.GoToAsync(".."); // "go back"
                
            }

        }

        private async void OpenGallery()
        {
            var result = await MediaPicker.PickPhotoAsync();

            if (result == null) { return; }

            Debug.WriteLine("XXXX: mediapicker: Gallery: getpath" + result.FullPath);

            var stream = await result.OpenReadAsync();

            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            byte[] orig_bytes = ms.ToArray();

            var scaled_and_jpegencoded = Resize2Save(orig_bytes);

            FilesizeProperty = "Filesize: " + scaled_and_jpegencoded.Length / 1024 + " KiBytes";

            Debug.WriteLine($"XXXX: jpegbytes length: {scaled_and_jpegencoded.Length} bytes {scaled_and_jpegencoded.Length / 1024} Kilobytes");

            _thumbnail_bytes = scaled_and_jpegencoded; 

            Debug.WriteLine("XXX OnGallery: before setting ImageProperty ");
 
            SKBitmap skbitmap = SKBitmap.Decode(scaled_and_jpegencoded);
            ImageProperty = (SKBitmapImageSource)skbitmap;


        }

        private async void OpenCamera()
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync();
                if (result != null)
                {
                    Debug.WriteLine("XXXX: CapturePhoto: Cam: getpath: " + result.FullPath);
                    var stream = await result.OpenReadAsync();
                    MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);
                    byte[] orig_bytes = ms.ToArray();
                    byte[] scaled_and_jpegencoded = Resize2Save(orig_bytes);
                    FilesizeProperty = "Filesize: " + scaled_and_jpegencoded.Length / 1024 + " KiBytes";
                    SKBitmap skbitmap = SKBitmap.Decode(scaled_and_jpegencoded);
                    ImageProperty = (SKBitmapImageSource)skbitmap;
                    _thumbnail_bytes = scaled_and_jpegencoded; // save in new ServiceRecord
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("XXXX: exception while taking pic " + e.Message);
            }
        }

        private byte[] Resize2Save(byte[] inbytes)
        {
            Debug.WriteLine("XXXX Resize2Save: resize start: " + System.DateTime.Now + " " + System.DateTime.Now.Millisecond);

            if (inbytes == null || inbytes.Length == 0)
            {
                Debug.WriteLine("XXXX Resize2Save: inStream == null");
                return new byte[] { };
            }

            SKBitmap bitmap = SKBitmap.Decode(inbytes);
            Debug.WriteLine("XXXX Resize2Save: original bitmap w: " + bitmap.Width + "h: " + bitmap.Height);

            double dwidth = bitmap.Width, dheight = bitmap.Height;
            bool tall = bitmap.Width < bitmap.Height;

            Debug.WriteLine("XXXX Resize2Save: image is tall: " + tall);

            double setImageSizeTo = 400; // = width for tall or height for wide images

            if (tall)
            {
                while (dwidth > setImageSizeTo)
                {
                    //dwidth /= 1.1; dheight /= 1.1;
                    double ratio = dheight / dwidth;
                    dwidth = setImageSizeTo;
                    dheight = ratio * dwidth;
                    Debug.WriteLine("XXXX: calc1 w: " + dwidth + " int: " + (int)dwidth + " h: " + dheight + "int:" + (int)dheight);
                }
            }
            else
            {
                while (dheight > setImageSizeTo)
                {
                    //dwidth /= 1.1; dheight /= 1.1;
                    double ratio = dwidth / dheight;
                    dheight = setImageSizeTo;
                    dwidth = ratio * dheight;
                    Debug.WriteLine("XXXX: calc2 w: " + dwidth + " int: " + (int)dwidth + " h: " + dheight + " int: " + (int)dheight);
                }
            }

            var smallerBitmap = new SKBitmap((int)dwidth, (int)dheight, bitmap.ColorType, bitmap.AlphaType);

            bool scaling_okay = bitmap.ScalePixels(smallerBitmap, SKFilterQuality.High);

            Debug.WriteLine("XXXX Resize2Save: resize end: " + System.DateTime.Now + " " + System.DateTime.Now.Millisecond);

            if (scaling_okay)
            {
                Debug.WriteLine("XXXX Resize2Save: scaling Okay");
                //ToggleUploadButtonProperty = true;
                SKData jpeg_skdata = smallerBitmap.Encode(SKEncodedImageFormat.Jpeg, 70);
                return jpeg_skdata.ToArray();
            }
            else
            {
                Debug.WriteLine("XXXX Resize2Save: scaling ERROR");
                byte[] nodata = null;
                return nodata;
            }
        }


    }
}
