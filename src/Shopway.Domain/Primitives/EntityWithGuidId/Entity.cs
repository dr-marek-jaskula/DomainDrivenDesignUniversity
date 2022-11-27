//namespace Shopway.Domain.Primitives.EntityWithGuidId;

//public abstract class Entity : IEquatable<Entity>
//{
//    protected Entity(Guid id)
//    {
//        Id = id;
//    }

//    protected Entity()
//    {
//    }

//    public Guid Id { get; private init; }

//    public static bool operator ==(Entity? first, Entity? second)
//    {
//        return first is not null
//            && second is not null
//            && first.Equals(second);
//    }

//    public static bool operator !=(Entity? first, Entity? second)
//    {
//        return !(first == second);
//    }

//    //Entity is identified by a unique id
//    public bool Equals(Entity? other)
//    {
//        if (other is null)
//        {
//            return false;
//        }

//        if (other.GetType() != GetType())
//        {
//            return false;
//        }

//        return other.Id == Id;
//    }

//    public override bool Equals(object? obj)
//    {
//        if (obj is null)
//        {
//            return false;
//        }

//        if (obj.GetType() != GetType())
//        {
//            return false;
//        }

//        if (obj is not Entity entity)
//        {
//            return false;
//        }

//        return entity.Id == Id;
//    }

//    //Hash multiplied by a prime number. This prime number must differ from ones that are specified below (primes)
//    public override int GetHashCode()
//    {
//        return Id.GetHashCode() * 41;
//    }

//    /*
//     * These primes are used by .NET so we should use one that is differed, for instance 41:
  
//       public static readonly int[] primes = {
//       3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
//       1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
//       17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
//       187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
//       1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};

//       for more details see stackoverflow: "How to pick prime numbers to calculate the hash code?"
//     */
//}