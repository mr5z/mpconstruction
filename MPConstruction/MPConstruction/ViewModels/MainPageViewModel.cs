using MPConstruction.Models;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MPConstruction.ViewModels
{
    internal class MainPageViewModel
    {
        public ObservableCollection<Photo> SelectedPhotos { get; set; }

        public DelegateCommand AddPhotoCommand { get; set; }

        public DelegateCommand<Photo> DeletePhotoCommand { get; set; }

        public MainPageViewModel() 
        {
            SelectedPhotos = new ObservableCollection<Photo>();
            AddPhotoCommand = new DelegateCommand(AddPhoto);
            DeletePhotoCommand = new DelegateCommand<Photo>(DeletePhoto);
        }

        private void AddPhoto()
        {
            var id = SelectedPhotos.LastOrDefault()?.Id ?? 0;
            var photo = new Photo { Id = id + 1, Name = "A photo of something", Source = $"https://picsum.photos/100?_={Guid.NewGuid()}" };
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
                var photo = await MediaPicker.PickPhotoAsync();
            }
            catch
            { }

            return null;
        }
    }
}
