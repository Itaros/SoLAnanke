using SignsOfLife.Entities;

namespace Runtime.ExampleMod
{
    public class AliveGoldenChicken : LivingEntity
    {
        public AliveGoldenChicken(long id) : base(0, 0)
        {
            _livingEntityType = (LivingEntityType) id;
        }
    }
}