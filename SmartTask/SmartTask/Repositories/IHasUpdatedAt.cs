namespace SmartTask.Repositories;

public interface IHasUpdatedAt
{
    DateTime? UpdatedAt { get; set; }
}
