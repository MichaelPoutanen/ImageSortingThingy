using System.Collections.Generic;
using System.Linq;
using ImageSortingThingy.Database;
using ImageSortingThingy.Helpers;
using ImageSortingThingy.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageSortingThingy.Services;

public class ImageStorageService
{
    private readonly ImageToolDbContext _dbContext;

    public ImageStorageService(ImageToolDbContext dbContext)
    {
        _dbContext = dbContext;

        _dbContext.Database.Migrate();
    }

    /// <summary>
    /// This method adds the image file to the database.
    /// It is called after the meta data has been calculated.
    /// </summary>
    /// <param name="imageFileModel">Meta data of the image file</param>
    /// <returns></returns>
    public GlobalDefinitions.GeneralErrorCodes AddImage(ImageFileListEntryModel imageFileModel)
    {
        if (_dbContext.ImageFileModels.Any(x => x.Crc32Hash == imageFileModel.Crc32Hash))
            return GlobalDefinitions.GeneralErrorCodes.ImageAlreadyExists;

        _dbContext.ImageFileModels.Add(imageFileModel);
        _dbContext.SaveChanges();

        return GlobalDefinitions.GeneralErrorCodes.NoError;
    }

    /// <summary>
    /// Check if image file exist in DB
    /// </summary>
    /// <param name="hash">CRC32 Hash of file</param>
    /// <returns>true if exists, false if not</returns>
    public bool DoesImageExistInDatabase(string hash)
    {
        return _dbContext.ImageFileModels.Any(x => x.Crc32Hash == hash);
    }

    /// <summary>
    /// Return all image file models
    /// </summary>
    /// <returns></returns>
    public List<ImageFileListEntryModel> GetAllImages()
    {
        return _dbContext.ImageFileModels.ToList();
    }

    /// <summary>
    /// Return an image file by its hash
    /// </summary>
    /// <param name="hash">CRC32 hash, as calculated by the CRC32 helper</param>
    /// <returns>The model or null if it doesn't exist</returns>
    public ImageFileListEntryModel? GetImageByHash(string hash)
    {
        return _dbContext.ImageFileModels.FirstOrDefault(x => x.Crc32Hash == hash);
    }

    /// <summary>
    /// Delete the entry in the database
    /// </summary>
    /// <param name="hash">File hash</param>
    public void RemoveImage(string hash)
    {
        ImageFileListEntryModel? image = _dbContext.ImageFileModels.FirstOrDefault(x => x.Crc32Hash == hash);
        if (image != null)
        {
            _dbContext.ImageFileModels.Remove(image);
            _dbContext.SaveChanges();
        }
    }
}