namespace CarroRobo.Domain.Extensions
{
	using System;
	using System.ComponentModel;
	using System.Reflection;

	/// <summary>
	/// Metodos de extensão
	/// </summary>
	public static class ExtensionMethods
	{
		/// <summary>
		/// Obtem descrição de um enum
		/// </summary>
		/// <param name="value">enum que deseja a descrição</param>
		/// <returns>string da descrição</returns>
		public static string GetDescription(this Enum value)
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if (name != null)
			{
				FieldInfo field = type.GetField(name);
				if (field != null)
				{
					DescriptionAttribute attr =
						Attribute.GetCustomAttribute(field,
						                             typeof(DescriptionAttribute)) as DescriptionAttribute;
					if (attr != null)
					{
						return attr.Description;
					}
				}
			}
			return null;
		}
	}
}