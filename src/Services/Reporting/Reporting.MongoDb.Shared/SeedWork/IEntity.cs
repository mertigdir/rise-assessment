namespace Reporting.MongoDb.Shared.SeedWork
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
