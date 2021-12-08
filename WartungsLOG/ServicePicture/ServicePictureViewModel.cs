using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using WartungsLOG.Core;
using WartungsLOG.Data;
using WartungsLOG.Select;
using WartungsLOG.ServiceHistory;
using Xamarin.Forms;
using System.IO;
using Xamarin.Essentials;
using System.Diagnostics;
using WartungsLOG.Fonts;

namespace WartungsLOG.ServicePicture
{
    [QueryProperty(nameof(ServiceIDProperty), "paraServiceID")]
    public class ServicePictureViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private string _sID;
        private ServicePictureRecord _servicepicturerecord;
        private ImageSource _image;

        public string ServiceIDProperty { get => _sID; set => SetProperty(ref _sID, value); }
        public ServicePictureRecord ServicePictureRecordProperty { get => _servicepicturerecord; set => SetProperty(ref _servicepicturerecord, value);}
        public ImageSource ImageProperty { get => _image; set => SetProperty(ref _image, value); }

        public ICommand ShareMeCommand { get; }

        public ServicePictureViewModel(IDataService ds)
        {
            _dataService = ds;
            Title = "";
            ShareMeCommand = new Command(shareme);
        }
        private async Task LoadDataAsync()
        {
            ServicePictureRecordProperty = await _dataService.GetServicePictureAsync(ServiceIDProperty);
            Logging.logMe($"got ServicePicture: {ServicePictureRecordProperty.ServicePictureID} {ServicePictureRecordProperty.Description}");
            string s = ServicePictureRecordProperty.Description;
            int maxLength = 15;
            Title = "pic";
            if(s !=null)
                Title = s.Length < maxLength ? s : s.Substring(0, maxLength);
        }
        public override async Task Initialize()
        {
            await LoadDataAsync();
            await base.Initialize();
        }

        public async void shareme(object arg)
        {
            string picname = Title;
            picname=picname.Replace("/", ""); picname =picname.Replace("\\","");
            //string picname = "share";
            var file = Path.Combine(FileSystem.CacheDirectory, picname + ".jpg");
            if(ServicePictureRecordProperty.ImageData == null)
            {
                Debug.WriteLine("XXXX: ServicePictureViewModel shareme(): SPRecordProperty was null");
                return;
            }
            File.WriteAllBytes(file, ServicePictureRecordProperty.ImageData);

            await Share.RequestAsync(
                new ShareFileRequest
                        {
                        Title = ServicePictureRecordProperty.Description,
                        File = new ShareFile(file)
                        }
                );
        }

    }
}
