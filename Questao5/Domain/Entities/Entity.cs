namespace Questao5.Domain.Entities
{
    public abstract class Entity
    {
        public abstract string Id { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
