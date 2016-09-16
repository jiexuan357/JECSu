﻿namespace JECSU.CodeGeneration
{

    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using System.Reflection;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// Various functionality that speeds up various things in system.
    /// </summary>
    public static class CodeGenerator
    {
        [MenuItem("JECSU/CompileEverything")]
        public static void CompileEverything()
        {
            GenerateComponentFactory();
            GenerateEntityConstructor();
        }


        [MenuItem("JECSU/ResetEverything")]
        public static void ResetEverything()
        {
            ResetComponentFactory();
            ResetEntityConstructor();
        }
        /// <summary>
        /// Resets component factory to empty state in case you have compile problem
        /// </summary>
        [MenuItem("JECSU/ComponentFactory/Reset")]
        public static void ResetComponentFactory()
        {
            string path = getfile("ComponentFactory.cs");
            File.WriteAllText(path,string.Empty);
            using (FileStream fs = new FileStream(path,
                                           FileMode.OpenOrCreate,
                                           FileAccess.ReadWrite,
                                           FileShare.None))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("/*This is generated file.");
                    sw.WriteLine("This class speeds up component creation in a safe manner.");
                    sw.WriteLine();
                    sw.WriteLine("===============================================================================================*/");
                    sw.WriteLine();

                    sw.WriteLine("namespace JECSU");
                    sw.WriteLine("{");
                    sw.WriteLine("using System;");

                    sw.WriteLine("\tpublic static class ComponentFactory");
                    sw.WriteLine("\t{");

                    sw.WriteLine("\t\tpublic static IComponent MakeNew<T>() where T : BaseComponent");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t\tType t = typeof(T);");
                    sw.WriteLine("\t\t\treturn MakeNew(t);");
                    sw.WriteLine("\t\t}");
                    

                    sw.WriteLine("\t\tpublic static IComponent MakeNew(Type t)");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t return null;");
                    sw.WriteLine("\t\t}");
                    sw.WriteLine("\t}");
                    sw.WriteLine("}");

                    sw.WriteLine("//EOF");
                }
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Component factory speeds up component creation by avoiding the use of Activator by generating constructor calls.
        /// </summary>
        [MenuItem("JECSU/ComponentFactory/Compile")]
        public static void GenerateComponentFactory()
        {
            string path = getfile("ComponentFactory.cs");
            File.WriteAllText(path,string.Empty);
            var componentTypes = FindAllDerivedTypes<BaseComponent>();
            using (FileStream fs = new FileStream(path,
                                           FileMode.OpenOrCreate,
                                           FileAccess.ReadWrite,
                                           FileShare.None))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("/*This is generated file.");
                    sw.WriteLine("This class speeds up component creation in a safe manner.");
                    sw.WriteLine();
                    sw.WriteLine("===============================================================================================*/");
                    sw.WriteLine();


                    sw.WriteLine("namespace JECSU");
                    sw.WriteLine("{");
                    sw.WriteLine("using System;");
                    sw.WriteLine("using System.Collections.Generic;");
                    sw.WriteLine("\tpublic static class ComponentFactory");
                    sw.WriteLine("\t{");
                    sw.WriteLine("\t\tstatic int lookup;");

                    sw.WriteLine("\t\tpublic static IComponent MakeNew<T>() where T : BaseComponent");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t\tType t = typeof(T);");
                    sw.WriteLine("\t\t\treturn MakeNew(t);");
                    sw.WriteLine("\t\t}");


                    sw.WriteLine("\t\tpublic static IComponent MakeNew(Type t)");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t\tif (!@switch.ContainsKey(t))return null;");
                    sw.WriteLine("\t\t\t@switch[t].Invoke();");
                    sw.WriteLine("\t\t\tswitch (lookup)");
                    sw.WriteLine("\t\t\t{");

                    int count = 0;
                    foreach (var type in componentTypes)
                    {
                        output_case(sw, type, count);
                        count++;
                    }
                    sw.WriteLine("\t\t\t default:");
                    sw.WriteLine("\t\t\t\t return null;");

                    sw.WriteLine("\t\t\t}");
                    sw.WriteLine("\t\t}");

                    sw.WriteLine("\t\tstatic Dictionary<Type, Action> @switch = new Dictionary<Type, Action> {");

                    count = 0;
                    foreach (var type in componentTypes)
                    {
                        output_dict(sw, type, count);
                        count++;
                    }
                    sw.WriteLine("\t\t};");
                    sw.WriteLine("\t}");
                    sw.WriteLine("}");

                    sw.WriteLine("//EOF");
                }
            }
            AssetDatabase.Refresh();

        }


        /// <summary>
        /// Component factory speeds up component creation by avoiding the use of Activator by generating constructor calls.
        /// </summary>
        [MenuItem("JECSU/EntityConstructor/Reset")]
        public static void ResetEntityConstructor()
        {
             string path = getfile("EntityConstructorFactory.cs");
            File.WriteAllText(path, string.Empty);

            using (FileStream fs = new FileStream(path,
                                           FileMode.OpenOrCreate,
                                           FileAccess.ReadWrite,
                                           FileShare.None))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("/*This is generated file.");
                    sw.WriteLine("This class speeds up component creation in a safe manner.");
                    sw.WriteLine();
                    sw.WriteLine("===============================================================================================*/");
                    sw.WriteLine();


                    sw.WriteLine("namespace JECSU");
                    sw.WriteLine("{");
                    sw.WriteLine("using System;");
                    sw.WriteLine("using System.Collections.Generic;");

                    sw.WriteLine("\tpublic partial class EntityConstructor");
                    sw.WriteLine("\t{");

                    sw.WriteLine("\t\tpublic static void AssignFromTemplate(IComponent component, Dictionary<string, string> values)");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t}");

                    sw.WriteLine("\t}");
                    sw.WriteLine("}");
                }
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Component factory speeds up component creation by avoiding the use of Activator by generating constructor calls.
        /// </summary>
        [MenuItem("JECSU/EntityConstructor/Compile")]
        public static void GenerateEntityConstructor()
        {
            
            string path = getfile("EntityConstructorFactory.cs");
            File.WriteAllText(path, string.Empty);
            var componentTypes = FindAllDerivedTypes<BaseComponent>();
            using (FileStream fs = new FileStream(path,
                                           FileMode.OpenOrCreate,
                                           FileAccess.ReadWrite,
                                           FileShare.None))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("/*This is generated file.");
                    sw.WriteLine("This class speeds up component creation in a safe manner.");
                    sw.WriteLine();
                    sw.WriteLine("===============================================================================================*/");
                    sw.WriteLine();


                    sw.WriteLine("namespace JECSU");
                    sw.WriteLine("{");
                    sw.WriteLine("using System;");
                    sw.WriteLine("using System.Collections.Generic;");

                    sw.WriteLine("\tpublic partial class EntityConstructor");
                    sw.WriteLine("\t{");
                    sw.WriteLine("\t\tstatic int lookup;");

                    sw.WriteLine("\t\tstatic Dictionary<Type, Action> @switch = new Dictionary<Type, Action> {");

                    int count = 0;
                    foreach (var type in componentTypes)
                    {
                        output_dict(sw, type, count);
                        count++;
                    }
                            
                    sw.WriteLine("\t\t};");

                    sw.WriteLine("\t\tpublic static void AssignFromTemplate(IComponent component, Dictionary<string, string> values)");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\tif (@switch.ContainsKey(component.type))");
                    sw.WriteLine("\t\t\t@switch[component.type].Invoke();");
                    sw.WriteLine("\t\tswitch (lookup)");
                    sw.WriteLine("\t\t{");

                    count = 0;
                    foreach (var type in componentTypes)
                    {
                        output_case_assign_from_template(sw, type, count);
                        count++;
                    }

                    sw.WriteLine("\t\t}");
                    sw.WriteLine("\t}");

                    count = 0;
                    foreach (var type in componentTypes)
                    {
                        write_method_assign_from_template(sw, type);
                        count++;
                    }

                    sw.WriteLine("\t}");
                    sw.WriteLine("}");


                }
            }
            AssetDatabase.Refresh();
        }

        public static List<Type> FindAllDerivedTypes<T>()
        {
            return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
        }

        public static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
        {
            var derivedType = typeof(T);
            return assembly
                .GetTypes()
                .Where(t =>
                    t != derivedType &&
                    derivedType.IsAssignableFrom(t)
                    ).ToList();

        }

        static void output_case_assign_from_template(StreamWriter sw, Type t, int number)
        {
            sw.WriteLine("\t\t\t case {0}:", number);
            sw.WriteLine("\t\t\t\t" + get_method_name_assign_from_template(t) + "((" + cleanname(t) + ") component, values);");
            sw.WriteLine("\t\t\t break;");
        }

        static void write_method_assign_from_template(StreamWriter sw, Type t)
        {
            sw.WriteLine("\tstatic void " + get_method_name_assign_from_template(t) + "(" + cleanname(t) + " comp, Dictionary<string,string> values) {");

            var fields = t.GetFields();
            var props = t.GetProperties();

            List<FieldInfo> valid_fields = new List<FieldInfo>();
            List<PropertyInfo> valid_props = new List<PropertyInfo>();

            foreach (var f in fields)
            {
                var attributes = f.GetCustomAttributes(false);
                if (check_attribute_ignore(attributes) && check_supported_type(f.FieldType))
                    valid_fields.Add(f);
            }

            foreach (var p in props)
            {
                var attributes = p.GetCustomAttributes(false);
                if (check_attribute_ignore(attributes) && check_supported_type(p.PropertyType))
                    valid_props.Add(p);
            }

            if (valid_fields.Count == 0 && valid_props.Count == 0)
            {
                sw.WriteLine("\t}");
                sw.WriteLine("");
            }
            else
            {
                sw.WriteLine("\t\tforeach (var p in values){");
                bool first = false;
                if(valid_props.Count > 0)
                {
                    foreach (var vprop in valid_props)
                    {
                        if (!first)
                        {
                            sw.WriteLine("\t\t\tif(p.Key == \"" + vprop.Name + "\" ){");
                            first = true;
                        }
                        else
                        {
                            sw.WriteLine("\t\t\telse if(p.Key == \"" + vprop.Name + "\" ){");
                        }

                        sw.WriteLine("\t\t\t\tcomp." + vprop.Name + " = " + get_convertor_prop(vprop));

                        sw.WriteLine("\t\t\t}");
                    }
                    
                }
                if(valid_fields.Count > 0)
                {
                    foreach (var vfield in valid_fields)
                    {
                        if (!first)
                        {
                            sw.WriteLine("\t\t\tif(p.Key == \"" + vfield.Name + "\" ){");
                            first = true;
                        }
                        else
                        {
                            sw.WriteLine("\t\t\telse if(p.Key == \"" + vfield.Name + "\" ){");
                        }

                        sw.WriteLine("\t\t\t\tcomp." + vfield.Name + " = " + get_convertor_field(vfield));

                        sw.WriteLine("\t\t\t}");
                    }   
                }

                sw.WriteLine("\t\t}");
                sw.WriteLine("\t}");
                sw.WriteLine("");
            }
        }

        static Dictionary<Type, int> supported_value_types = new Dictionary<Type, int>
        {
            { typeof(int) , 1 },
            { typeof(float), 1 },
            { typeof(long), 1 },
            { typeof(bool), 1 },
            { typeof(double), 1 },
            { typeof(Vector2), 1 },
            { typeof(Vector3), 1 },
            { typeof(Color), 1 },
            { typeof(string), 1 }

        };

        static bool check_supported_type(Type t)
        {
            if (supported_value_types.ContainsKey(t))
                return true;
            else
                return false;
        }

        static bool check_attribute_ignore(object[] attrs)
        {
            for (int i = 0; i < attrs.Length; i++)
            {
                if (attrs[i].GetType() == typeof(TemplateIgnore))
                    return false;
            }
            return true;
        }

        static string get_convertor_prop(PropertyInfo pinfo)
        {
            if (pinfo.PropertyType == typeof(int))
                return "Convert.ToInt16(p.Value);";
            else if (pinfo.PropertyType == typeof(float))
                return "Convert.ToSingle(p.Value);";
            else if (pinfo.PropertyType == typeof(string))
                return "p.Value;";
            else if (pinfo.PropertyType == typeof(Vector2))
                return "convertV2(p.Value);";
            else if (pinfo.PropertyType == typeof(Vector3))
                return "convertV3(p.Value);";
            else if (pinfo.PropertyType == typeof(double))
                return "Convert.ToDouble(p.Value);";
            else if (pinfo.PropertyType == typeof(bool))
                return "convertBool(p.Value);";
            else if (pinfo.PropertyType == typeof(long))
                return "Convert.ToInt64(p.Value);";
            else if (pinfo.PropertyType == typeof(Color))
                return "convertColor(p.Value);";

            else return "Error type not supported!";
        }

        static string get_convertor_field(FieldInfo finfo)
        {
            if (finfo.FieldType == typeof(int))
                return "Convert.ToInt16(p.Value);";
            else if (finfo.FieldType == typeof(float))
                return "Convert.ToSingle(p.Value);";
            else if (finfo.FieldType == typeof(string))
                return "p.Value;";
            else if (finfo.FieldType == typeof(Vector2))
                return "convertV2(p.Value);";
            else if (finfo.FieldType == typeof(Vector3))
                return "convertV3(p.Value);";
            else if (finfo.FieldType == typeof(double))
                return "Convert.ToDouble(p.Value);";
            else if (finfo.FieldType == typeof(bool))
                return "convertBool(p.Value);";
            else if (finfo.FieldType == typeof(Color))
                return "convertColor(p.Value);";
            else if (finfo.FieldType == typeof(long))
                return "Convert.ToInt64(p.Value);";
            else return "Error type not supported!";
        }

        static string get_method_name_assign_from_template(Type t)
        {
            string result = "";
            if (t.FullName.Contains("+"))
            {
                result = t.FullName.Replace('+', '_');
            }
            else
                if (t.FullName.Contains("."))
            {
                result = t.FullName.Replace('.', '_');
            }
            else
                result = t.FullName;

            return result + "_assignFromTemplate";
        }

        static void output_case(StreamWriter sw, Type t, int number)
        {
            sw.WriteLine("\t\t\t case {0}:", number);
            sw.WriteLine("\t\t\t\t return new {0}();", cleanname(t));
        }

        static string cleanname(Type t)
        {
            if (t.FullName.Contains("+"))
            {
                return t.FullName.Replace('+', '.');
            }
            return t.FullName;
        }

        static void output_dict(StreamWriter sw, Type t, int number)
        {
            if (number == 0)
                sw.WriteLine("\t\t{ typeof(" + cleanname(t) + "), () => lookup = " + number + " }");
            else
                sw.WriteLine("\t\t,{ typeof(" + cleanname(t) + "), () => lookup = " + number + " }");
        }

        static string getfile(string file)
        {
            //First we try to find already existing .cs file project wide, and if we dont find it, we create a new one.
            string path = "";
            List<string> files = Directory.GetFiles(Application.dataPath + "/JECSU/", "*.cs", SearchOption.AllDirectories).ToList();
            foreach (string st in files)
            {
                if (st.Contains(file))
                {
                    string[] pathArr = st.Split('\\');
                    if (pathArr[pathArr.Length - 1].Equals(file))
                    {
                        path = st;
                    }
                    break;
                }
            }
            if (path == "")
            {
                path = Application.dataPath + "/JECSU/" + file;
            }
            return path;
        }
    }
}