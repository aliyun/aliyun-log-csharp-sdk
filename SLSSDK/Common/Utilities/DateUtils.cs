/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using System.Globalization;

namespace Aliyun.Api.LOG.Common.Utilities
{
    /// <summary>
    /// Description of DateUtils.
    /// </summary>
    internal static class DateUtils
    {
        private static DateTime _1970StartDateTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
        private const string _rfc822DateFormat = "ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T";
        private const string _iso8601DateFormat = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";

        /// <summary>
        /// Formats an instance of <see cref="DateTime" /> to a GMT string.
        /// </summary>
        /// <param name="dt">The date time to format.</param>
        /// <returns></returns>
        public static string FormatRfc822Date(DateTime dt)
        {
            return dt.ToUniversalTime().ToString(_rfc822DateFormat,
                               CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats a GMT date string to an object of <see cref="DateTime" />.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ParseRfc822Date(String dt)
        {
            Debug.Assert(!string.IsNullOrEmpty(dt));
            return DateTime.SpecifyKind(
                DateTime.ParseExact(dt,
                                    _rfc822DateFormat,
                                    CultureInfo.InvariantCulture),
                DateTimeKind.Utc);
        }

        /// <summary>
        /// Formats a date to a string in the format of ISO 8601 spec.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string FormatIso8601Date(DateTime dt)
        {
            return dt.ToUniversalTime().ToString(_iso8601DateFormat,
                               CultureInfo.CreateSpecificCulture("en-US"));
        }

        public static uint TimeSpan() { 
            return (uint)Math.Round((DateTime.Now - _1970StartDateTime).TotalSeconds, MidpointRounding.AwayFromZero);
        }
    }
}
