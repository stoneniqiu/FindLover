using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NoteSystem.Helper
{
    /// <summary>
    /// 复制对象类
    /// </summary>
    public class ObjectCopier
    {
        /// <summary>
        /// 复制对象属性
        /// </summary>
        /// <param name="dstObject">目标对象</param>
        /// <param name="srcObject">源对象</param>
        public static void CopyProperties(Object dstObject, object srcObject)
        {
            if (dstObject == null)
            {
                return;
            }
            if (srcObject == null)
            {
                return;
            }
            if (srcObject.GetType() != dstObject.GetType())
            {
                throw new Exception("目标对象与源对象类型不一致，不能复制");
            }
            try
            {
                //   clone   properties 
                PropertyDescriptorCollection srcPdc = TypeDescriptor.GetProperties(srcObject);
                PropertyDescriptorCollection dstPdc = TypeDescriptor.GetProperties(dstObject);

                for (int i = 0; i < srcPdc.Count; i++)
                {
                    dstPdc[srcPdc[i].Name].SetValue(dstObject, srcPdc[i].GetValue(srcObject));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 复制对象属性，只能复制公共属性
        /// </summary>
        /// <param name="dstObject">目标对象</param> 
        /// <param name="propertyName">需要修改的属性</param>
        /// <param name="propertyValue">源对象</param>
        public static void CopyProperty(Object dstObject, string propertyName, object propertyValue)
        {
            if (dstObject == null)
            {
                return;
            }
            try
            {
                PropertyDescriptorCollection dstPdc = TypeDescriptor.GetProperties(dstObject);

                PropertyDescriptor dstPd = dstPdc.Find(propertyName, true);
                if (dstPd == null)
                {
                    throw new ArgumentNullException("需要修改的对象属性不存在");
                }
                //设置值为空时，需要判断被设置的属性是否允许置空
                if (propertyValue == null)
                {
                    if (!dstPd.PropertyType.IsValueType)
                    {
                        dstPd.SetValue(dstObject, propertyValue);
                        return;
                    }
                    throw new Exception("需要修改的属性类型不能置null");
                }
                //枚举类型
                if (dstPd.PropertyType.IsEnum && propertyValue.GetType().IsValueType)
                {
                    dstPd.SetValue(dstObject, propertyValue);
                    return;
                }
                var value = propertyValue;
                //如果是值类型，则尝试转换
                if (dstPd.PropertyType.IsValueType)
                {
                    value = Convert.ChangeType(propertyValue, dstPd.PropertyType);
                }
                //判断转换后是否类型一致
                if (!IsType(dstPd.PropertyType, value.GetType()))
                {
                    throw new Exception("需要修改的属性类型不一致");
                }
                dstPd.SetValue(dstObject, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 类型匹配
        /// </summary>
        /// <param name="type">需要匹配的类型</param>
        /// <param name="valueType">需要匹配的对象类型</param>
        /// <returns>匹配成功返回true，否则返回false</returns>
        public static bool IsType(Type type, Type valueType)
        {
            if (type.ToString() == valueType.ToString())
                return true;
            if (valueType.ToString() == "System.Object")
                return false;

            return IsType(type, valueType.BaseType);
        }

        /// <summary>
        /// 将object转换为指定type类型的值
        /// </summary>
        /// <param name="value">需要转换的值</param>
        /// <param name="type">需要转换的类型</param>
        /// <returns>转换后的值</returns>
        public static object ConvertType(object value, Type type)
        {
            object obj;
            try
            {
                switch (type.ToString())
                {
                    case "System.String":
                        obj = Convert.ToString(value);
                        break;
                    case "System.DateTime":
                        obj = Convert.ToDateTime(value);
                        break;
                    case "System.Double":
                        obj = Convert.ToDouble(value);
                        break;
                    case "System.Decimal":
                        obj = Convert.ToDecimal(value);
                        break;
                    case "System.Boolean":
                        obj = Convert.ToBoolean(value);
                        break;
                    case "System.Byte":
                        obj = Convert.ToByte(value);
                        break;
                    case "System.Char":
                        obj = Convert.ToChar(value);
                        break;
                    case "System.Int16":
                        obj = Convert.ToInt16(value);
                        break;
                    case "System.Int32":
                        obj = Convert.ToInt32(value);
                        break;
                    case "System.Int64":
                        obj = Convert.ToInt64(value);
                        break;
                    case "System.SByte":
                        obj = Convert.ToSByte(value);
                        break;
                    case "System.Single":
                        obj = Convert.ToSingle(value);
                        break;
                    case "System.UInt16":
                        obj = Convert.ToUInt16(value);
                        break;
                    case "System.UInt32":
                        obj = Convert.ToUInt32(value);
                        break;
                    case "System.UInt64":
                        obj = Convert.ToUInt64(value);
                        break;
                    default:
                        obj = value;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return obj;
        }

        /// <summary>
        /// 复制对象属性，允许拷贝私有属性
        /// </summary>
        /// <param name="dstObject">目标对象</param> 
        /// <param name="propertyName">需要修改的属性</param>
        /// <param name="propertyValue">源对象</param>
        /// <param name="isCopyNonPublic">是否拷贝私有属性,需要拷贝私有属性置true</param>
        public static void CopyProperty(Object dstObject, string propertyName, object propertyValue, bool isCopyNonPublic)
        {
            if (dstObject == null)
            {
                return;
            }
            try
            {
                Type t = dstObject.GetType();
                BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
                if (isCopyNonPublic)
                {
                    flags |= BindingFlags.NonPublic;
                }
                PropertyInfo pI = t.GetProperty(propertyName, flags);
                if (pI == null)
                {
                    throw new ArgumentNullException("需要修改的对象属性不存在");
                }
                //设置值为空时，需要判断被设置的属性是否允许置空
                if (propertyValue == null)
                {
                    if (!dstObject.GetType().IsValueType)
                    {
                        pI.SetValue(dstObject, null, null);
                        return;
                    }
                    throw new Exception("需要修改的属性类型不能置null");
                }
                //枚举类型
                if (pI.PropertyType.IsEnum && propertyValue.GetType().IsValueType)
                {
                    pI.SetValue(dstObject, propertyValue, null);
                    return;
                }
                var value = propertyValue;
                //如果是值类型，则尝试转换
                if (pI.PropertyType.IsValueType)
                {
                    value = Convert.ChangeType(propertyValue, pI.PropertyType);
                }
                //判断转换后是否类型一致
                if (!IsType(pI.PropertyType, value.GetType()))
                {
                    throw new Exception("需要修改的属性类型不一致");
                }
                pI.SetValue(dstObject, value, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Perform a deep Copy of the object对象深拷贝.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException(@"The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

      
    }
}