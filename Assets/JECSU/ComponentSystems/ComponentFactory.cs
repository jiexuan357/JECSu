/*This is generated file.
This class speeds up component creation in a safe manner.

===============================================================================================*/

namespace JECSU
{
using System;
	public static class ComponentFactory
	{
		public static IComponent MakeNew<T>() where T : BaseComponent
		{
			Type t = typeof(T);
			return MakeNew(t);
		}
		public static IComponent MakeNew(Type t)
		{
		 return null;
		}
	}
}
//EOF
