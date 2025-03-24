namespace PaParchar.Domain.Entities
{
    public abstract class _BaseEntity<ID> where ID : notnull
    {
        public ID Id { get; private set; } = default!;
    }
}
