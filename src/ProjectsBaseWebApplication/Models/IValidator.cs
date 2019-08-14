namespace ProjectsBaseWebApplication.Models
{
    public interface IValidator<TEntity> where TEntity : class
    {
        bool Validate(TEntity entity);
    }
}
