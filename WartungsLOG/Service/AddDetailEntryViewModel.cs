using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using SkiaSharp;
using SkiaSharp.Views.Forms;

using WartungsLOG.Select;
using WartungsLOG.Core;
using WartungsLOG.Data;
using WartungsLOG.ServiceHistory;
using System.IO;
using System.Diagnostics;

//
// take/select picture and let the DataService convert the large image to thumbnail size
//


namespace WartungsLOG.Service
{
    
    //[QueryProperty(nameof(VehicleIDProperty), "para"+nameof(VehicleIDProperty))]
    [QueryProperty(nameof(ServiceHistoryIDProperty), "para" + nameof(ServiceHistoryIDProperty))]

    public class AddDetailEntryViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navService;
        private ServiceRecord _serviceRecord;
        private string _sid;
        private string _description;
        private int _km;
        private int _filesize; // size of image to be uploaded
        private ImageSource _image;
        bool _toggle = false;
        string path = Xamarin.Essentials.FileSystem.CacheDirectory;

        public Command OpenGalleryCommand { get; }
        public Command OpenCameraCommand { get; }
        public Command UploadCommand { get; }
        public ICommand NavigateToSaveCommand { get; }

        public ImageSource ImageProperty { get => _image; set => SetProperty(ref _image, value); }
        public bool ToggleUploadButtonProperty { get => _toggle; set => SetProperty(ref _toggle, value); }
        public int KmProperty { get => _km; set => SetProperty(ref _km, value); }
        public string DescriptionProperty { get => _description; set => SetProperty(ref _description, value); }

        public int FilesizeProperty { get => _filesize; set => SetProperty(ref _filesize, value); }
        
       // public string ServiceIDProperty { get => _sid; set => SetProperty(ref _sid, value); }

        public ServiceRecord ServiceRecordProperty { get => _serviceRecord; set => SetProperty(ref _serviceRecord, value); }

        public string ServiceHistoryIDProperty { get; set; }

        public class Punkt2D { public int x; public int y; }
        public Punkt2D P2DProperty { get; set; }

        public AddDetailEntryViewModel(IDataService dataSvc, INavigationService navSvc)
        {
            _dataService = dataSvc;
            _navService = navSvc;
            OpenGalleryCommand = new Command(OpenGallery);
            OpenCameraCommand = new Command(OpenCamera);
            NavigateToSaveCommand = new Command(NavigateToSave);

            Title = "neuer Eintrag im Logbuch";
            ServiceRecordProperty = new ServiceRecord();
            FilesizeProperty = 0;

        }

        private async void NavigateToSave(object arg)
        {
            // 1. save kmProperty DescriptionProperty Images
            // 2. go back to service detail page
            Debug.WriteLine("XXXX: Save Service command !!!");

            ServiceRecordProperty.RefServiceHistoryID = ServiceHistoryIDProperty;

            // small enough? limit of cosmos db < 2MiB
            if (FilesizeProperty > 1400)
            {
                await Shell.Current.DisplayAlert("Filesize", $"can't save {FilesizeProperty} KiB only <1400 ","Cancel");
            }

            var result = await Shell.Current.DisplayAlert(
            "save image",
            $"save image with {FilesizeProperty} KiB {P2DProperty.x} x {P2DProperty.y}" ,
            "Yes, Please!", "Nope!");

            if (result)
            {

                await _dataService.AddServiceEntry(ServiceRecordProperty); // DataService will convert large image to thumbnail

                // reset properties

                FilesizeProperty = 0;
                ServiceRecordProperty.ImageThumbData = null;
                ServiceRecordProperty.Description = "";
                ImageProperty = null;
                ToggleUploadButtonProperty = false;

                await _navService.GoToAsync("..");
            }
        }


        private async void OpenGallery()
        {
            var result = await MediaPicker.PickPhotoAsync();

            if (result == null) { return; }

            Debug.WriteLine("XXXX: mediapicker: Gallery: getpath" + result.FullPath);

            var stream = await result.OpenReadAsync();

            //ImageProperty = ImageSource.FromStream(() => stream);

            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            byte[] orig_bytes = ms.ToArray();

            var scaled_and_jpegencoded = Resize2Save(orig_bytes);

            
            FilesizeProperty =  scaled_and_jpegencoded.Length / 1024;
            
            Debug.WriteLine($"XXXX: jpegbytes length: {scaled_and_jpegencoded.Length} bytes {scaled_and_jpegencoded.Length/1024} Kilobytes" );


            ServiceRecordProperty.ImageThumbData = scaled_and_jpegencoded; // save in new ServiceRecord


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
                    FilesizeProperty = scaled_and_jpegencoded.Length / 1024;
                    SKBitmap skbitmap =  SKBitmap.Decode(scaled_and_jpegencoded);
                    ImageProperty = (SKBitmapImageSource)skbitmap;
                    ServiceRecordProperty.ImageThumbData = scaled_and_jpegencoded; // save in new ServiceRecord

                 
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

            if(inbytes == null || inbytes.Length == 0)
            {
                Debug.WriteLine("XXXX Resize2Save: inStream == null");
                return new byte[] { };
            }
            
            SKBitmap bitmap = SKBitmap.Decode(inbytes);
           

            Debug.WriteLine("XXXX Resize2Save: original bitmap w: " + bitmap.Width + "h: " + bitmap.Height);
         
            double dwidth = bitmap.Width, dheight = bitmap.Height;
            bool tall = bitmap.Width < bitmap.Height;

            Debug.WriteLine("XXXX Resize2Save: image is tall: " + tall);

            double setImageSizeTo = 1200; // = width for tall or height for wide images

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
                ToggleUploadButtonProperty = true;
                SKData jpeg_skdata = smallerBitmap.Encode(SKEncodedImageFormat.Jpeg, 77); // set Jpeg Quality here

                var p2d = new Punkt2D { x=(int)dwidth, y=(int)dheight };
                P2DProperty = p2d;

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
