using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Garage2.Extensions
{
	public static class LinqExtensions
	{
		/// <summary>
		/// Order by dynamic fieldnames
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="ordering"></param>
		/// <param name="ascending"></param>
		/// <returns></returns>

		public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderfield, bool ascending = true)
		{
			var type = typeof(T);
			var parameter = Expression.Parameter(type, "p");
			PropertyInfo property;
			Expression propertyAccess;
			if (orderfield.Contains('.'))
			{
				// support to be sorted on child fields.
				String[] childProperties = orderfield.Split('.');
				property = type.GetProperty(childProperties[0]);
				propertyAccess = Expression.MakeMemberAccess(parameter, property);
				for (int i = 1; i < childProperties.Length; i++)
				{
					property = property.PropertyType.GetProperty(childProperties[i]);
					propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
				}
			}
			else
			{
				property = typeof(T).GetProperty(orderfield);
				if (property == null)
					return source;
				propertyAccess = Expression.MakeMemberAccess(parameter, property);
			}
			var orderByExp = Expression.Lambda(propertyAccess, parameter);
			MethodCallExpression resultExp = Expression.Call(typeof(Queryable),
															 ascending ? "OrderBy" : "OrderByDescending",
															 new[] { type, property.PropertyType }, source.Expression,
															 Expression.Quote(orderByExp));
			//return  source.OrderBy(x => orderByExp);
			return source.Provider.CreateQuery<T>(resultExp);
		}
	}
}