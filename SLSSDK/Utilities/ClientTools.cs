/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
using Aliyun.Api.LOG.Common.Authentication;
using Aliyun.Api.LOG.Common.Communication;
using Aliyun.Api.LOG;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;
using LZ4Sharp;
using Lz4;

namespace Aliyun.Api.LOG.Utilities
{
    internal class LogClientTools
    {
        public static byte[] CompressToZlib(byte[] buffer)
        {
            using (MemoryStream compressStream = new MemoryStream())
            {
                using (ZLibNet.ZLibStream gz = new ZLibNet.ZLibStream(compressStream, ZLibNet.CompressionMode.Compress, ZLibNet.CompressionLevel.Level6, true))
                {
                    gz.Write(buffer, 0, buffer.Length);
                    compressStream.Seek(0, SeekOrigin.Begin);
                byte[] compressBuffer = new byte[compressStream.Length];
                compressStream.Read(compressBuffer, 0, compressBuffer.Length);
                return compressBuffer;
                }
            }
        }
        public static byte[] DecompressFromZlib(Stream stream, int rawSize)
        {
            using (stream)
            {
                using (ZLibNet.ZLibStream dz = new ZLibNet.ZLibStream(stream, ZLibNet.CompressionMode.Decompress))
                {
                    byte[] buffer = new byte[rawSize];
                    dz.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            }
        }
        public static byte[] CompressToLz4(byte[] buffer)
        {
            var compressor = LZ4CompressorFactory.CreateNew();
            return compressor.Compress(buffer);
        }
        public static byte[] DecompressFromLZ4(Stream stream, int rawLength)
        {
            using (stream)
            {
                using (Lz4DecoderStream streamInner = new Lz4DecoderStream(stream))
                {
                    byte[] output = new byte[rawLength];
                    streamInner.Read(output, 0, rawLength);
                    return output;
                }
            }
        }
        public static string GetMd5Value(byte[] buffer)
        {
            return GetMd5Value(buffer,0,buffer.Length);
        }
        public static string GetMd5Value(byte[] buffer,int offset, int count)
        {
            MD5 hash = MD5.Create(LogConsts.NAME_MD5);
            StringBuilder returnStr = new StringBuilder();
            byte[] md5hash = hash.ComputeHash(buffer, offset, count);
            if (md5hash != null)
            {
                for (int i = 0; i < md5hash.Length; i++)
                {
                    returnStr.Append(md5hash[i].ToString("X2").ToUpper());
                }
            }
            return returnStr.ToString();
        }
        
        public static void ResponseErrorCheck(ServiceResponse response, ServiceCredentials credentials)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                String requestId = "";
                response.Headers.TryGetValue(LogConsts.NAME_HEADER_REQUESTID, out requestId);
                JObject body = ParserResponseToJObject(response.Content);
                throw new LogException(body[LogConsts.NAME_ERROR_CODE].ToString(), body[LogConsts.NAME_ERROR_MESSAGE].ToString(), requestId);
            }
        }
        internal class KeyValueComparer : IComparer<KeyValuePair<string, string>>
        {
            public int Compare(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
            {
                int rtu = String.Compare(x.Key, y.Key, StringComparison.Ordinal);
                return rtu == 0 ? String.Compare(x.Value, y.Value, StringComparison.Ordinal) : rtu;
            }
        }
        private static String MapEnumMethodToString(HttpMethod httpMethod)
        {
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    return LogConsts.NAME_HTTP_GET;
                case HttpMethod.Post:
                    return LogConsts.NAME_HTTP_POST;
                case HttpMethod.Put:
                    return LogConsts.NAME_HTTP_PUT;
                case HttpMethod.Head:
                    return LogConsts.NAME_HTTP_HEAD;
                case HttpMethod.Delete:
                    return LogConsts.NAME_HTTP_DELETE;
                case HttpMethod.Options:
                    return LogConsts.NAME_HTTP_OPTIONS;
                default:
                    Debug.Assert(false, "invalid http method");
                    return "";
            }
        }
        private static string GetRequestString(IEnumerable<KeyValuePair<string, string>> parameters, String kvDelimiter, String separator)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            if (parameters != null)
            {
                bool isFirst = true;
                foreach (var p in parameters)
                {
                    if (!isFirst)
                    {
                        stringBuilder.Append(separator);
                    }
                    isFirst = false;
                    stringBuilder.Append(p.Key);
                    if (p.Value != null)
                    {
                        stringBuilder.Append(kvDelimiter).Append(p.Value);
                    }
                }
            }
            return stringBuilder.ToString();
        }
        internal static String BuildHeaderSigStr(IDictionary<String, String> headers)
        {
            List<KeyValuePair<string, string>> headerLst = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, string> pair in headers)
            {
                if (pair.Key.StartsWith("x-log-") && pair.Key.CompareTo(LogConsts.NAME_HEADER_DATE) != 0)
                    headerLst.Add(new KeyValuePair<String, String>(pair.Key.Trim().ToLower(), pair.Value.Trim()));
            }
            headerLst.Sort(new KeyValueComparer());
            StringBuilder reqUri = new StringBuilder();
            reqUri.Append(LogClientTools.GetValeFromDic(headers, LogConsts.NAME_HEADER_MD5)).Append("\n")
                .Append(LogClientTools.GetValeFromDic(headers, LogConsts.NAME_HEADER_CONTENTTYPE)).Append("\n")
                .Append(LogClientTools.GetValeFromDic(headers, LogConsts.NAME_HEADER_DATE)).Append("\n")
                .Append(GetRequestString(headerLst, ":", "\n"));
            return reqUri.ToString();
        }
        internal static String SigInternal(String source,String accessKeyId, String accessKey)
        {
            ServiceSignature signAlthm = ServiceSignature.Create();
            return LogConsts.PREFIX_VALUE_HEADER_AUTH + accessKeyId + ":" + signAlthm.ComputeSignature(accessKey, source);
        }
        public static String Signature(IDictionary<String, String> headers, String accessKeyId, String accessKey)
        {
            return SigInternal(BuildHeaderSigStr(headers), accessKeyId, accessKey);
        }
        public static String Signature(IDictionary<String,String> headers,IDictionary<String,String> paramDic,HttpMethod method, String resource,String accessKeyId,String accessKey)
        {
            List<KeyValuePair<string, string>> paramLst = new List<KeyValuePair<string, string>>(paramDic);
            
            paramLst.Sort(new KeyValueComparer());
            
            StringBuilder reqUri = new StringBuilder();
            reqUri.Append(MapEnumMethodToString(method)).Append("\n")
                .Append(BuildHeaderSigStr(headers)).Append("\n")
                .Append(resource)
                .Append((paramLst != null && paramLst.Count > 0) ? ("?" + GetRequestString(paramLst, "=", "&")) : (""));
            return SigInternal(reqUri.ToString(), accessKeyId, accessKey);
        }
        internal static String GetValeFromDic(IDictionary<String, String> dic, String keyName)
        {
            String value = null;
            if (dic.TryGetValue(keyName, out value))
                return value;
            return "";
        }
        public static JArray ParserResponseToJArray(Stream response)
        {
            using (response)
            {
                StreamReader sr = null;
                String json = null;
                try
                {
                    sr = new StreamReader(response, Encoding.UTF8);
                }
                catch (Exception e)
                {
                    if (sr != null)
                        sr.Close();
                    throw new LogException("LOGBadResponse", "The response from the server is empty", e);
                }
                try
                {
                    json = sr.ReadToEnd();
                }
                catch (IOException e)
                {
                    throw new LogException("LOGBadResponse", "Io exception happened when parse the response data : ", e);
                }
                catch (OutOfMemoryException e)
                {
                    throw new LogException("LOGBadResponse", "There is not enough memory to continue the execution of parsing the response data : ", e);
                }
                finally
                {
                    sr.Close();
                }
                try
                {
                    JArray obj = JArray.Parse(json);
                    return obj;
                }
                catch (Exception e)
                {
                    throw new LogException("LOGBadResponse", "The response is not valid json string : " + json, e);
                }
            }
        }
        public static JObject ParserResponseToJObject(Stream response)
        {
            using (response)
            {
                StreamReader sr = null;
                String json = null;
                try
                {
                    sr = new StreamReader(response, Encoding.UTF8);
                }
                catch (Exception e)
                {
                    if (sr != null)
                        sr.Close();
                    throw new LogException("LOGBadResponse", "The response from the server is empty", e);
                }
                try
                {
                    json = sr.ReadToEnd();
                }
                catch (IOException e)
                {
                    throw new LogException("LOGBadResponse", "Io exception happened when parse the response data : ", e);
                }
                catch (OutOfMemoryException e)
                {
                    throw new LogException("LOGBadResponse", "There is not enough memory to continue the execution of parsing the response data : ", e);
                }
                finally
                {
                    sr.Close();
                }
                try
                {
                    JObject obj = JObject.Parse(json);
                    return obj;
                }
                catch (Exception e)
                {
                    throw new LogException("LOGBadResponse", "The response is not valid json string : " + json, e);
                }
            }
        }
    }
}
