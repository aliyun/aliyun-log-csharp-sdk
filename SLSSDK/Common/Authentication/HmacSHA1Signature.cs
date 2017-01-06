/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Aliyun.Api.LOG.Common.Authentication
{
    internal class HmacSHA1Signature : ServiceSignature
    {
        private Encoding _encoding = Encoding.UTF8;

        public override string SignatureMethod
        {
            get { return "HmacSHA1"; }
        }

        public override string SignatureVersion
        {
            get { return "1"; }
        }

        public HmacSHA1Signature()
        {
        }

        protected override string ComputeSignatureCore(string key, string data)
        {
            Debug.Assert(!string.IsNullOrEmpty(data));

            using (KeyedHashAlgorithm algorithm = KeyedHashAlgorithm.Create(
                this.SignatureMethod.ToString().ToUpperInvariant()))
            {
                algorithm.Key = _encoding.GetBytes(key.ToCharArray());
                return Convert.ToBase64String(
                    algorithm.ComputeHash(_encoding.GetBytes(data.ToCharArray())));
            }
        }

    }
}
