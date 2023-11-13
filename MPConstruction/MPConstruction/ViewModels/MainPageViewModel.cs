using MPConstruction.Apis;
using MPConstruction.Exceptions;
using MPConstruction.Models;
using MPConstruction.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MPConstruction.ViewModels
{
    internal class MainPageViewModel
    {
        private readonly IImageApi imageApi;
        private readonly IReportApi reportApi;
        private readonly IToastService toastService;

        public DelegateCommand AddPhotoCommand { get; set; }

        public DelegateCommand<Photo> DeletePhotoCommand { get; set; }

        public DelegateCommand NextCommand { get; set; }

        public ObservableCollection<Photo> SelectedPhotos { get; set; }

        public bool IncludePhotoInGallery { get; set; }

        public string Comments { get; set; }

        public DateTime DateTime { get; set; }

        public string SelectedArea { get; set; }

        public string TaskCategory { get; set; }

        public string Tags { get; set; }

        public bool LinkToExistingEvent { get; set; }

        public string SelectedEvent { get; set; }

        public MainPageViewModel(
            IImageApi imageApi,
            IReportApi reportApi,
            IToastService toastService) 
        {
            this.imageApi = imageApi;
            this.reportApi = reportApi;
            this.toastService = toastService;

            AddPhotoCommand = new DelegateCommand(AddPhoto);
            DeletePhotoCommand = new DelegateCommand<Photo>(DeletePhoto);
            NextCommand = new DelegateCommand(Next);
            SelectedPhotos = new ObservableCollection<Photo>();
            IncludePhotoInGallery = true;
            DateTime = DateTime.Now;
            LinkToExistingEvent = true;
        }

        private async void Next()
        {
            try
            {
                Validate();
                await UploadImages();
                await SendReport();
                toastService.Show("Uploaded successfully");
                ResetPage();
            }
            catch (ValidationException)
            {
                // TODO do something else other than showing Toast
            }
            catch (Exception)
            {
                toastService.Show("An error occurred. Please try again later.");
            }
        }

        private async Task UploadImages()
        {
            var tasks = new List<Task<ImageResponse>>();
            foreach (var p in SelectedPhotos)
            {
                var b64 = await ToBase64(p);
                // TODO the base64 encoded image is too large for the API to handle or wouldn't accept if it's in the request body
                // For the sake of this demo, I discarded its value and replaced with "<placeholder>" instead
                var task = imageApi.UploadImage("<placeholder>");
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
        }

        private Task SendReport()
        {
            return reportApi.SendReport(new Report
            {
                Comment = Comments,
                DateTime = DateTime,
                LinkToExistingEvent = LinkToExistingEvent,
                SelectedArea = SelectedArea,
                SelectedEvent = SelectedEvent,
                Tags = Tags,
                TaskCategory = TaskCategory
            });
        }

        private void ResetPage()
        {
            SelectedPhotos.Clear();
            // TODO reset every properties to their default values
        }

        private async void AddPhoto()
        {
            var photo = await PickFromGallery();
            if (photo == null)
                return;

            SelectedPhotos.Add(photo);
        }

        private void DeletePhoto(Photo photo)
        {
            SelectedPhotos.Remove(photo);
        }

        private async Task<Photo> PickFromGallery()
        {
            try
            {
                await RequestPhotoPermissions();
                var photo = await MediaPicker.PickPhotoAsync();
                var id = SelectedPhotos.LastOrDefault()?.Id ?? 0;
                return new Photo
                {
                    Id = id + 1,
                    Source = photo.FullPath,
                    Ref = photo
                };
            }
            catch
            {
                toastService.Show("An error occurred. Please try again later.");
            }

            return null;
        }

        private async Task RequestPhotoPermissions()
        {
            var status1 = await Permissions.RequestAsync<Permissions.Photos>();
            if (status1 != PermissionStatus.Granted)
                throw new Exception("Photos permission not granted");
            var status2 = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (status2 != PermissionStatus.Granted)
                throw new Exception("Storage permission not granted");
        }

        private async Task<string> ToBase64(Photo photo)
        {
            var r = photo.Ref;
            using var stream = await r.OpenReadAsync();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var bytes = ms.ToArray();
            return Convert.ToBase64String(bytes);
        }

        private void Validate()
        {
            if (!SelectedPhotos.Any())
                throw new ValidationException();
            // TODO add more property validations here
        }
    }
}
