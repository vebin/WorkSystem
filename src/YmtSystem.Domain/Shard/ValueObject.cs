//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================

namespace YmtSystem.Domain.Shard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using YmtSystem.CrossCutting.FastReflection;
    using YmtSystem.CrossCutting.Utility;
   
    /// <summary>
    /// 领域值对象基类.    
    /// </summary>
    /// <typeparam name="TValueObject">The type of this value object</typeparam>
    public class ValueObject<TValueObject> : IEquatable<TValueObject>
        where TValueObject : ValueObject<TValueObject>
    {
        /// <summary>
        /// 由于值类型对象比较时默认比较所有属性值，这可能产生性能问题；子类若重写此方法，则在执行比较时只比较该方法集合中的属性值。        
        /// </summary>
        /// <remarks>注意：添加到此集合的元素必须要显示或隐式实现IComparable接口</remarks>
        /// <returns></returns>
        protected virtual IEnumerable<object> EqualityComponents()
        {
            return Enumerable.Empty<object>();
        }
        #region IEquatable and Override Equals operators

        /// <summary>
        /// <see cref="M:System.Object.IEquatable{TValueObject}"/>
        /// </summary>
        /// <param name="other"><see cref="M:System.Object.IEquatable{TValueObject}"/></param>
        /// <returns><see cref="M:System.Object.IEquatable{TValueObject}"/></returns>
        public bool Equals(TValueObject other)
        {
            if ((object)other == null)
                return false;

            if (Object.ReferenceEquals(this, other))
                return true;
           
            //如果GetEqualityComponents存在元素，则只比较这些元素，否则全部比较
            if (this.EqualityComponents().Any())
            {
                // if (object.ReferenceEquals(this, other)) return true;
                // if (object.ReferenceEquals(null, other)) return false;
                if (this.GetType() != other.GetType()) return false;
                return this.EqualityComponents().SequenceEqual(other.EqualityComponents());
            }
            //compare all public properties
            PropertyInfo[] publicProperties = this.GetType().GetProperties();

            if ((object)publicProperties != null
                &&
                publicProperties.Any())
            {
                return publicProperties.All(p =>
                {
                    var left = p.FastGetValue(this);//.GetValue(this, null);
                    var right = p.FastGetValue(other);//.GetValue(other, null);


                    if (typeof(TValueObject).IsAssignableFrom(left.GetType()))
                    {
                        //check not self-references...
                        return Object.ReferenceEquals(left, right);
                    }
                    else
                        return left.Equals(right);


                });
            }
            else
                return true;
        }
        /// <summary>
        /// <see cref="M:System.Object.Equals"/>
        /// </summary>
        /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
        /// <returns><see cref="M:System.Object.Equals"/></returns>
        public override bool Equals(object obj)
        {
            if ((object)obj == null)
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            ValueObject<TValueObject> item = obj as ValueObject<TValueObject>;

            if ((object)item != null)
                return Equals((TValueObject)item);
            else
                return false;

        }
        /// <summary>
        /// <see cref="M:System.Object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="M:System.Object.GetHashCode"/></returns>
        public override int GetHashCode()
        {
            //如果GetEqualityComponents有元素则只计算这些元素的hash值
            //否则计算全部属性值
            if (this.EqualityComponents().Any())
            {
                return HashCodeHelper.CombineHashCodes(this.EqualityComponents());
            }

            int hashCode = 31;
            bool changeMultiplier = false;
            int index = 1;

            //compare all public properties
            PropertyInfo[] publicProperties = this.GetType().GetProperties();


            if ((object)publicProperties != null
                &&
                publicProperties.Any())
            {
                foreach (var item in publicProperties)
                {
                    object value = item.GetValue(this, null);

                    if ((object)value != null)
                    {

                        hashCode = hashCode * ((changeMultiplier) ? 59 : 114) + value.GetHashCode();

                        changeMultiplier = !changeMultiplier;
                    }
                    else
                        hashCode = hashCode ^ (index * 13);//only for support {"a",null,null,"a"} <> {null,"a","a",null}
                }
            }

            return hashCode;
        }

        public static bool operator ==(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);

        }

        public static bool operator !=(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            return !(left == right);
        }

        #endregion
    }
}
