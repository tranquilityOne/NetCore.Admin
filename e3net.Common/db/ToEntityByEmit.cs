using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using System.Data;
using System.Dynamic;

namespace testGenerateEntity
{
    /// <summary>
    /// 类生成器
    /// </summary>
    public static class ToEntityByEmit
    {

        public static List<T> GetList<T>(DataTable dt)
        {
            List<T> lst = new List<T>();
            if (dt == null) return lst;
            DataTableEntityBuilder<T> eblist = DataTableEntityBuilder<T>.CreateBuilder(dt.Rows[0]);
            foreach (DataRow dr in dt.Rows)
            { 
                lst.Add(eblist.Build(dr));
            }
            dt.Dispose();
            dt = null;
            return lst;
        }


        /// <summary>
        /// 将DataReader数据转为Dynamic对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static dynamic DataFillDynamic(IDataReader reader)
        {
            dynamic d = new ExpandoObject();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                try
                {
                    ((IDictionary<string, object>)d).Add(reader.GetName(i), reader.GetValue(i));
                }
                catch
                {
                    ((IDictionary<string, object>)d).Add(reader.GetName(i), null);
                }
            }
            return d;
        }

        /// <summary>
        /// 获取模型对象集合
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<dynamic> DataFillDynamicList(IDataReader reader)
        {
            List<dynamic> list = new List<dynamic>();
            if (reader != null && !reader.IsClosed)
            {
                while (reader.Read())
                {
                    list.Add(DataFillDynamic(reader));
                }
                reader.Close();
            }
            return list;
        }

        public static DataTable DataReaderToDataTable(IDataReader reader)
        {
            DataTable table = new DataTable();
            int fieldCount = reader.FieldCount;
            for (int i = 0; i < fieldCount; i++)
            {
                table.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }
            table.BeginLoadData();
            object[] values = new object[fieldCount];
            while (reader.Read())
            {
                reader.GetValues(values);
                table.LoadDataRow(values, true);
            }
            table.EndLoadData();
            return table;
        }

    }

    public class DataTableEntityBuilder<T>
    {
        private static readonly MethodInfo getValueMethod = typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullMethod = typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });
        private delegate T Load(DataRow dr);

        private Load handler;
        private DataTableEntityBuilder() { }

        public T Build(DataRow dr)
        {
            return handler(dr);
        }

        public static DataTableEntityBuilder<T> CreateBuilder(DataRow dr)
        {
            DataTableEntityBuilder<T> dynamicBuilder = new DataTableEntityBuilder<T>();
            DynamicMethod method = new DynamicMethod("DynamicCreateEntity", typeof(T), new Type[] { typeof(DataRow) }, typeof(T), true);
            ILGenerator generator = method.GetILGenerator();
            LocalBuilder result = generator.DeclareLocal(typeof(T));
            generator.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            for (int i = 0; i < dr.ItemArray.Length; i++)
            {
                PropertyInfo pi = typeof(T).GetProperty(dr.Table.Columns[i].ColumnName);
                Label endIfLabel = generator.DefineLabel();
                if (pi != null && pi.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);
                    generator.Emit(OpCodes.Unbox_Any, pi.PropertyType);
                    generator.Emit(OpCodes.Callvirt, pi.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);
            dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));
            return dynamicBuilder;
        }
    }


    /// <summary>
    /// 使用了委脱
    /// </summary>
    public static class ToEntityByEmit2
    {
        public static List<T> ToList<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            if (dt == null) return list;
            DataTableEntityBuilder<T> eblist = DataTableEntityBuilder<T>.CreateBuilder(dt.Rows[0]);
            foreach (DataRow info in dt.Rows)
                list.Add(eblist.Build(info));
            dt.Dispose();
            dt = null;
            return list;
        }
        public class DataTableEntityBuilder<T>
        {
            private static readonly MethodInfo getValueMethod = typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(int) });
            private static readonly MethodInfo isDBNullMethod = typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });
            private delegate T Load(DataRow dataRecord);
            private Load handler;
            private DataTableEntityBuilder() { }
            public T Build(DataRow dataRecord)
            {
                return handler(dataRecord);
            }
            public static DataTableEntityBuilder<T> CreateBuilder(DataRow dataRow)
            {
                DataTableEntityBuilder<T> dynamicBuilder = new DataTableEntityBuilder<T>();
                DynamicMethod method = new DynamicMethod("DynamicCreateEntity", typeof(T), new Type[] { typeof(DataRow) }, typeof(T), true);
                ILGenerator generator = method.GetILGenerator();
                LocalBuilder result = generator.DeclareLocal(typeof(T));
                generator.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
                generator.Emit(OpCodes.Stloc, result);
                for (int index = 0; index < dataRow.ItemArray.Length; index++)
                {
                    PropertyInfo propertyInfo = typeof(T).GetProperty(dataRow.Table.Columns[index].ColumnName);
                    Label endIfLabel = generator.DefineLabel();
                    if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                    {
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldc_I4, index);
                        generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                        generator.Emit(OpCodes.Brtrue, endIfLabel);
                        generator.Emit(OpCodes.Ldloc, result);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldc_I4, index);
                        generator.Emit(OpCodes.Callvirt, getValueMethod);
                        generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                        generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                        generator.MarkLabel(endIfLabel);
                    }
                }
                generator.Emit(OpCodes.Ldloc, result);
                generator.Emit(OpCodes.Ret);
                dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));
                return dynamicBuilder;
            }
        }
    }
}
