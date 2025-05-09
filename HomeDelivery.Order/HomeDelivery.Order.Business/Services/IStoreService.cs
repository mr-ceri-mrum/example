using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.DataAccess.DataAccess;
using Microsoft.AspNetCore.Http;

namespace HomeDelivery.Order.Business.Services;

public interface IStoreService
{
    Task<bool> StorePhotoForOrder(Guid orderId);
    Task<bool> StorePhotoForDish(Guid dishId);
    Task<bool> StorePhotoForOrderRequest (Guid dishId);
}

public class StoreService(IHttpContextAccessor httpContextAccessor, IOrderDal orderDal, IDishesDal dishesDal,
    IMessagesRepository messagesRepository) 
    : IStoreService
{
    
    public async Task<bool> StorePhotoForOrder(Guid orderId)
    {
        try
        {
            var order = await orderDal
                .GetAsync(x => x.Id == orderId, 
                    inc => inc.Dish);
            
            if (order == null) return false;
            var files = httpContextAccessor.HttpContext!.Request.Form.Files;
            if (!files.Any())
                return false;
            var fileName = Guid.NewGuid().ToString();
            
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "Avatars");
            
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            
            var filePaths = new List<string>();
            string relativeFolderPath = string.Empty;
            
            foreach (var file in files)
            {       
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".jpg" && extension != ".png")
                    return false;
                
                var saveFilePath = Path.Combine(folderPath, $"{fileName}{extension}");
                relativeFolderPath = Path.Combine("wwwroot", "files", "Avatars",  $"{fileName}{extension}");
                await using var stream = new FileStream(saveFilePath, FileMode.Create);
                await file.CopyToAsync(stream);
                
                filePaths.Add(saveFilePath);
            }

            if (order.Dish == null) return false;
            
            order.Dish.ImgUrl = relativeFolderPath;
            
            await orderDal.UpdateAsync(order);
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }

    public async Task<bool> StorePhotoForDish(Guid dishId)
    {
        try
        {
            var dish = await dishesDal
                .GetAsync(x => x.Id == dishId);
            
            if (dish == null) return false;
            var files = httpContextAccessor.HttpContext!.Request.Form.Files;
            if (!files.Any())
                return false;
            var fileName = Guid.NewGuid().ToString();
            
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "Avatars");
            
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            
            var filePaths = new List<string>();
            string relativeFolderPath = string.Empty;
            
            foreach (var file in files)
            {       
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".jpg" && extension != ".png")
                    return false;
                
                var saveFilePath = Path.Combine(folderPath, $"{fileName}{extension}");
                relativeFolderPath = Path.Combine("wwwroot", "files", "Avatars",  $"{fileName}{extension}");
                await using var stream = new FileStream(saveFilePath, FileMode.Create);
                await file.CopyToAsync(stream);
                
                filePaths.Add(saveFilePath);
            }
            
            dish.ImgUrl = relativeFolderPath;
            await dishesDal.UpdateAsync(dish);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> StorePhotoForOrderRequest(Guid orderId)
    {
        var order = await orderDal
            .GetAsync(x => x.Id == orderId);
            
        if (order == null) return false;
        var files = httpContextAccessor.HttpContext!.Request.Form.Files;
        if (!files.Any())
            return false;
        var fileName = Guid.NewGuid().ToString();
            
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "Avatars");
            
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
            
        var filePaths = new List<string>();
        string relativeFolderPath = string.Empty;
            
        foreach (var file in files)
        {       
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".jpg" && extension != ".png")
                return false;
                
            var saveFilePath = Path.Combine(folderPath, $"{fileName}{extension}");
            relativeFolderPath = Path.Combine("wwwroot", "files", "Avatars",  $"{fileName}{extension}");
            await using var stream = new FileStream(saveFilePath, FileMode.Create);
            await file.CopyToAsync(stream);
                
            filePaths.Add(saveFilePath);
        }
            
        order.ImgUrl = relativeFolderPath;
        
        await orderDal.UpdateAsync(order);
        return true;
    }
}
