using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MadMobile.Data.Helpers
{
	public static class LinqToXmlExpressionHelpers
	{
		public static IEnumerable<XElement> DescendantsFirstLevel(this XElement element)
		{
			return element == null 
				? new List<XElement>() 
				: element.Descendants().Where(d => d.Parent == element);
		}

		public static IEnumerable<XElement> WhereAttribute(this IEnumerable<XElement> enumerable, string attribute, string attributeValue) 
		{
			return enumerable.AsQueryable().WhereAttribute(attribute, attributeValue).AsEnumerable();
		}

		public static IQueryable<XElement> WhereAttribute(this IQueryable<XElement> queryable, string attribute, string attributeValue) 
		{
			return queryable.filterByAttribute(attribute, attributeValue);
		}

		public static IEnumerable<XElement> WhereAttributeNot(this IEnumerable<XElement> enumerable, string attribute, string attributeValue)
		{
			return enumerable.AsQueryable().excludeByAttribute(attribute, attributeValue).AsEnumerable();
		}

		public static IQueryable<XElement> WhereAttributeNot(this IQueryable<XElement> queryable, string attribute, string attributeValue) 
		{
			return queryable.excludeByAttribute(attribute, attributeValue);
		}

		public static XElement FirstByAttribute(this IEnumerable<XElement> enumerable, string attribute, string attributeValue) 
		{
			return enumerable.AsQueryable().FirstOrDefaultByAttribute(attribute, attributeValue);
		}

		public static XElement FirstByAttribute(this IQueryable<XElement> queryable, string attribute, string attributeValue) 
		{
			return queryable.filterByAttribute(attribute, attributeValue).First();
		}

		public static XElement FirstOrDefaultByAttribute(this IEnumerable<XElement> enumerable, string attribute, string attributeValue) 
		{
			return enumerable.AsQueryable().FirstOrDefaultByAttribute(attribute, attributeValue);
		}

		public static XElement FirstOrDefaultByAttribute(this IQueryable<XElement> queryable, string attribute, string attributeValue) 
		{
			return queryable.filterByAttribute(attribute, attributeValue).FirstOrDefault();
		}
		
		public static bool AnyByAttribute(this IEnumerable<XElement> queryable, string attribute, string attributeValue)
		{
			return queryable.AsQueryable().existsByAttribute(attribute, attributeValue);
		}

		public static bool AnyByAttribute(this IQueryable<XElement> queryable, string attribute, string attributeValue)
		{
			return queryable.existsByAttribute(attribute, attributeValue);
		}

		public static IQueryable<XElement> WhereElementName(this IQueryable<XElement> queryable, string elementName)
		{
			return queryable.filterByElementName(elementName);
		}

		public static IEnumerable<XElement> WhereElementName(this IEnumerable<XElement> enumerable, string elementName)
		{
			return enumerable.AsQueryable().filterByElementName(elementName);
		}

		public static XElement FirstByElementName(this IQueryable<XElement> queryable, string elementName)
		{
			return queryable.filterByElementName(elementName).First();
		}

		public static XElement FirstByElementName(this IEnumerable<XElement> enumerable, string elementName)
		{
			return enumerable.AsQueryable().filterByElementName(elementName).First();
		}

		public static XElement FirstOrDefaultByElementName(this IQueryable<XElement> queryable, string elementName)
		{
			return queryable.filterByElementName(elementName).FirstOrDefault();
		}

		public static XElement FirstOrDefaultByElementName(this IEnumerable<XElement> enumerable, string elementName)
		{
			return enumerable.AsQueryable().filterByElementName(elementName).FirstOrDefault();
		}

		public static bool AnyByElementName(this IQueryable<XElement> queryable, string elementName)
		{
			return queryable.filterByElementName(elementName).Any();
		}

		public static bool AnyByElementName(this IEnumerable<XElement> enumerable, string elementName)
		{
			return enumerable.AsQueryable().filterByElementName(elementName).Any();
		}

		private static IQueryable<XElement> filterByAttribute(this IQueryable<XElement> queryable, string attribute, string attributeValue)
		{
			return queryable.Where(d => d.Attribute(attribute) != null && d.Attribute(attribute).Value.ToLower().Contains(attributeValue.ToLower()));
		}

		private static IQueryable<XElement> excludeByAttribute(this IQueryable<XElement> queryable, string attribute, string attributeValue)
		{
			return queryable.Where(d => 
				d.Attribute(attribute) == null 
				|| (
					d.Attribute(attribute) != null 
					&& !d.Attribute(attribute).Value.ToLower().Contains(attributeValue.ToLower())
				)
			);
		}

		private static bool existsByAttribute(this IQueryable<XElement> queryable, string attribute, string attributeValue)
		{
			return queryable.Any(d => d.Attribute(attribute) != null && d.Attribute(attribute).Value.ToLower().Contains(attributeValue.ToLower()));
		}

		private static IQueryable<XElement> filterByElementName(this IQueryable<XElement> queryable, string elementName)
		{
			return queryable.Where(d => d.Name.LocalName.ToLower() == elementName.ToLower());
		}

		public static string AttributeValue(this XElement element, string attributeName)
		{
			if(element == null)
				return string.Empty;

			return element.Attribute(attributeName) == null 
				? string.Empty
				: element.Attribute(attributeName).Value.Trim();
		}

		public static string ElementValue(this XElement element)
		{
			return ElementValue(element, false);
		}

		public static string ElementValue(this XElement element, bool removeInnerElements)
		{
			if(element == null)
				return string.Empty;

			// NOTE: we want to use a cloned xelement incase the removeInnerElements is called,
			// we don't want them to actually disappear.
			var tempElement = new XElement(element);

			if(removeInnerElements)
				tempElement.Descendants().Remove();

			var value = tempElement.Value.Trim();
			return value;
		}

		public static XElement SelectBoxSelectedOption(this XElement selectBox)
		{
			if(selectBox == null)
				return null;

			return selectBox.Descendants("option")
				.FirstOrDefaultByAttribute("selected", "selected");
		}
	}
}