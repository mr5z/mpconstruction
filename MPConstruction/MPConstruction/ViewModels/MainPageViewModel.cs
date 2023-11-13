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

        public MainPageViewModel(IImageApi imageApi, IToastService toastService) 
        {
            this.imageApi = imageApi;
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

                var tasks = new List<Task<ImageResponse>>();
                foreach(var p in SelectedPhotos)
                {
                    var b64 = await ToBase64(p);
                    var task = imageApi.UploadImage(b64);
                    tasks.Add(task);
                }
                await Task.WhenAll(tasks);
            }
            catch (ValidationException)
            {
                // TODO do something else other than showing Toast
            }
            catch (Exception ex)
            {
                toastService.Show("An error occurred. Please try again later.");
            }
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
                    Id = id,
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
            // TODO add property validations here
        }
    }
}
