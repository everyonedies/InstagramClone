using System;
using System.Drawing;
using System.IO;
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

        public void SetProfilePhoto(AppUser user, Image image, string imageExt)
        {
            string fileName = PhotoProcessing(user, image, imageExt, 152, 152);
            user.Picture = $"/images/Users/{user.Alias}/" + fileName;
            unitOfWork.SaveAsync();
        }

        public void AddNewPost(AppUser user, Image image, string imageExt)
        {
            var userWithItems = unitOfWork.Users.GetByAliasWithItems(user.Alias);
            string fileName = PhotoProcessing(userWithItems, image, imageExt, 270, 270);

            Post post = new Post
            {
                Picture = $"/images/Users/{userWithItems.Alias}/" + fileName,
                Date = DateTime.Now
            };

            userWithItems.Posts.Add(post);
            unitOfWork.SaveAsync();
        }

        private string PhotoProcessing(AppUser user, Image image, string imageExt, int w, int h)
        {
            var uploadDirectory = Path.Combine(hostingEnvironment.WebRootPath, $"images\\Users\\{user.Alias}\\");

            var fileName = Guid.NewGuid().ToString();
            var fileNameExt = fileName + imageExt;
            var savePath = Path.Combine(uploadDirectory, fileNameExt);

            Image resized = CutImage(image);
            Image scaled = ScaleImage(resized, w, h);

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            scaled.Save(savePath);
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
