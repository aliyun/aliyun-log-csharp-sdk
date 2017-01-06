/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using System.IO;

namespace Aliyun.Api.LOG.Common.Utilities
{
    /// <summary>
    /// Description of IOUtils.
    /// </summary>
    internal static class IOUtils
    {
        private const int _bufferSize = 1024 * 4;
        
        public static void WriteTo(this Stream src, Stream dest){
            if (dest == null)
                throw new ArgumentNullException("dest");
            
            byte[] buffer = new byte[_bufferSize];
            int bytesRead = 0;
            while((bytesRead = src.Read(buffer, 0, buffer.Length)) > 0)
            {
                dest.Write(buffer, 0, bytesRead);
            }
            dest.Flush();
        }
        
        /// <summary>
        /// Write a stream to another
        /// </summary>
        /// <param name="orignStream">The stream you want to write from</param>
        /// <param name="destStream">The stream written to</param>
        /// <param name="maxLength">The max length of the stream to write</param>
        /// <returns>The actual length written to destStream</returns>
        public static long WriteTo(this Stream orignStream, Stream destStream, long maxLength)
        {
            const int buffSize = 1024;
            byte[] buff = new byte[buffSize];
            long alreadyRead = 0;
            
            int readCount = 0;
            while (alreadyRead < maxLength)
            {
                readCount = orignStream.Read(buff, 0, buffSize);
                if (readCount <= 0) { break; }
               
                if (alreadyRead + readCount > maxLength)
                {
                        readCount = (int) (maxLength - alreadyRead);
                }
                alreadyRead += readCount;
                destStream.Write(buff, 0, readCount);
            }
            destStream.Flush();
            return alreadyRead;
        }
    }
}
