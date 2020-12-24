<Query Kind="Program" />

void Main()
{
		var t = new List<Test>(){new Test(){A="A1",B="B1"},new Test(){Id=3,A="A2",B="B2"},new Test(){Id= 2 ,A="A3",B="B3"}}.AsQueryable();
		var x = OOrderBy(t,"Id");
		x.Dump();
}

        public IQueryable<T> OOrderBy<T>(IQueryable<T> query, string property, bool isDesc = false) where T : class
        {
            Type entityType = typeof(T);
            if (string.IsNullOrEmpty(property))
            {
                return query;
            }
            //針對欄位不管傳入大小寫，
            var thisField = entityType.GetProperty(property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (thisField is null || (thisField.PropertyType.IsGenericType && thisField.PropertyType.GetGenericTypeDefinition() == (typeof(IEnumerable<>))))
            {
                return query;
            }
            var param = Expression.Parameter(entityType, "o");
            var member = Expression.Property(param, property);
			Expression conversion = Expression.Convert(member, typeof(object));
            var lambda = Expression.Lambda<Func<T,object>>(conversion, param);
			
            //因為是泛型Mehtod要呼叫MakeGenericMethod決定泛型型別
            return isDesc ? query.OrderByDescending(lambda)
                          : query.OrderBy(lambda);
		//	return query;
        }

// Define other methods, classes and namespaces here
public class Test
{
	public int? Id {get;set;}
	public string A{get;set;}
	public string B{get;set;}
}