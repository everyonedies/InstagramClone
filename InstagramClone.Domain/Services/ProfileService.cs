using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using Microsoft.AspNetCore.Hosting;

namespace InstagramClone.Domain.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IUnitOfWork unitOfWork;

        public ProfileService(IHostingEnvironment hostingEnvironment, IUnitOfWork unitOfWork)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.unitOfWork = unitOfWork;
        }

        public async Task SetProfilePhoto(AppUser user, Image image, string imageExt)
        {
            string fileName = PhotoProcessing(user, image, imageExt, 152, 152);
            user.Picture = $"/images/Users/{user.Alias}/" + fileName;
            await unitOfWork.SaveAsync();
        }

        public async Task AddNewPost(AppUser user, Image image, string imageExt)
        {
            var userWithItems = await unitOfWork.Users.GetByAliasWithItems(user.Alias);
            string fileNamePicView = SavePhoto(userWithItems, image, imageExt);
            string fileNamePicPreview = PhotoProcessing(userWithItems, image, imageExt, 270, 270);

            Post post = new Post
            {
                PictureView = $"/images/Users/{userWithItems.Alias}/" + fileNamePicView,
                PicturePreview = $"/images/Users/{userWithItems.Alias}/" + fileNamePicPreview,
                Date = DateTime.Now,
            };

            userWithItems.Posts.Add(post);
            await unitOfWork.SaveAsync();
        }

        public async Task<bool> DeletePost(AppUser user, int postId)
        {
            Post postWithItems = await unitOfWork.Posts.GetByIdWithItems(postId);
            bool check = postWithItems.User.Alias == user.Alias;

            if (check)
            {
                await unitOfWork.Posts.DeleteAsync(postWithItems);
                return true;
            }
            else return false;
        }

        private string SavePhoto(AppUser user, Image image, string imageExt)
        {
            var (savePath, fileNameExt) = PreparePhoto(user, imageExt);
            ImageFormat format = ImageFormat.Jpeg;
            switch (imageExt)
            {
                case ".png": format = ImageFormat.Png; break;
                case ".gif": format = ImageFormat.Gif; break;
            }
            image.Save(savePath, format);
            return fileNameExt;
        }

        private (string savePath, string fileNameExt) PreparePhoto(AppUser user, string imageExt)
        {
            var uploadDirectory = Path.Combine(hostingEnvironment.WebRootPath, $"images\\Users\\{user.Alias}\\");

            var fileName = Guid.NewGuid().ToString();
            var fileNameExt = fileName + imageExt;
            var savePath = Path.Combine(uploadDirectory, fileNameExt);

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            return (savePath, fileNameExt);
        }

        private string PhotoProcessing(AppUser user, Image image, string imageExt, int w, int h)
        {
            var (savePath, fileNameExt) = PreparePhoto(user, imageExt);

            Image resized = CutImage(image);
            Image scaled = ScaleImage(resized, w, h);
            ImageFormat format = ImageFormat.Jpeg;
            switch (imageExt)
            {
                case ".png": format = ImageFormat.Png; break;
                case ".gif": format = ImageFormat.Gif; break;
            }
            scaled.Save(savePath, format);
            return fileNameExt;
        }

        private Image CutImage(Image image)
        {
            Bitmap img = (Bitmap)image;

            int val = Math.Min(image.Width, image.Height);
            Bitmap resized = null;

            if (image.Width > image.Height)
            {
                resized = img.Clone(new Rectangle((image.Width - image.Height) / 2, 0, val, val), image.PixelFormat);
            }
            else
            {
                resized = img.Clone(new Rectangle(0, 0, val, val), image.PixelFormat);
            }

            return resized;
        }

        private Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
    }
}
