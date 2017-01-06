/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aliyun.Api.LOG.Common.Utilities
{
    /// <summary>
    /// Description of EnumUtils.
    /// </summary>
    internal static class EnumUtils
    {
        private static IDictionary<Enum, StringValueAttribute> _stringValues =
            new Dictionary<Enum, StringValueAttribute>();

        public static string GetStringValue(this Enum value)
        {
            string output = null;
            Type type = value.GetType();

            if (_stringValues.ContainsKey(value))
            {
                output = (_stringValues[value] as StringValueAttribute).Value;
            }
            else
            {
                FieldInfo fi = type.GetField(value.ToString());
                StringValueAttribute[] attrs =
                    fi.GetCustomAttributes(typeof (StringValueAttribute),
                                           false) as StringValueAttribute[];
                if (attrs.Length > 0)
                {
                    output = attrs[0].Value;
                    // Put it in the cache.
                    lock(_stringValues)
                    {
                        // Double check
                        if (!_stringValues.ContainsKey(value))
                        {
                            _stringValues.Add(value, attrs[0]);
                        }
                    }
                }
                else
                {
                    return value.ToString();
                }
            }

            return output;
        }
    }
}
