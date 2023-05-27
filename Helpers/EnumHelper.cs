using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snippets.Helpers;

public static class EnumHelpers
{
	// Using a tuple to determine whether the conversion was successful.
	public static (bool, TEnum) GetEnumFromString<TEnum>(string text) where TEnum : struct
	{
		if (!typeof(TEnum).GetTypeInfo().IsEnum)
		{
			// throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
			return (false, null);
		}
		return (true, (TEnum)Enum.Parse(typeof(TEnum), text));
	}
}
