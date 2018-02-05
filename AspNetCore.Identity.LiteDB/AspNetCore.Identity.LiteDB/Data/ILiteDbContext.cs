using LiteDB;

namespace AspNetCore.Identity.LiteDB.Data
{
    public interface ILiteDbContext
    {
        LiteDatabase LiteDatabase { get; set; }
    }
}